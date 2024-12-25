using Microsoft.AspNetCore.Mvc;
using URL_Shortener.Services;
using URL_Shortener.Models;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using URL_Shortener.Hub;
using System.Text.Json;


namespace URL_Shortener.Controllers
{
    [ApiController]
    [Route("api/urls")]
    public class UrlController : ControllerBase
    {
        private readonly UrlService _urlService;
        private readonly UserService _userService;
        private readonly IHubContext<UrlUpdateHub> _hubContext;
        private readonly IHttpClientFactory _httpClientFactory;

        public UrlController(UrlService urlService, UserService userService, IHubContext<UrlUpdateHub> hubContext, IHttpClientFactory httpClientFactory)
        {
            _urlService = urlService;
            _userService = userService;
            _hubContext = hubContext;
            _httpClientFactory = httpClientFactory;
        }
        
        [HttpPost("create")]
        public async Task<IActionResult> CreateShortUrl([FromBody] CreateUrlDto createUrlDto)
        {
            if (string.IsNullOrEmpty(createUrlDto.HashedUrl) || string.IsNullOrEmpty(createUrlDto.Username))
            {
                return BadRequest("Both Username and HashedUrl are required.");
            }

            string decodedUrl = DecodeUrl(createUrlDto.HashedUrl);
            string shortUrl = await GetShortUrlFromApi(decodedUrl);


            var user = await _userService.GetUserByUsernameAsync(createUrlDto.Username);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            
            bool urlExists = await _urlService.IsUrlExistsAsync(decodedUrl);
            if (urlExists)
            {
                return BadRequest("This URL already exists.");
            }

            var urlModel = new UrlModel
            {
                OriginalUrl = decodedUrl,
                ShortUrl = shortUrl,
                CreatedBy = createUrlDto.Username,
                CreatedDate = DateTime.UtcNow,
                UserId = user.Id
            };

            await _urlService.AddUrlAsync(urlModel); 
            await _hubContext.Clients.All.SendAsync("UpdateUrls", $"New URL created");
            return Ok(new { shortUrl = shortUrl });
        }

        private async Task<string> GetShortUrlFromApi(string originalUrl)
        {

            var client = _httpClientFactory.CreateClient();


            var requestData = new
            {
                long_url = originalUrl,
                domain = "https://t.ly", 
                description = "Shortened URL for " + originalUrl,
                public_stats = true 
            };


            var requestContent = new StringContent(
                JsonSerializer.Serialize(requestData),
                Encoding.UTF8,
                "application/json"
            );


            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "da0uWlSLwpODX8M06ELmitKoakMjVIYzPD53XLjhCPzu5iBRFcbvBOWUe24a");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


            var response = await client.PostAsync("https://api.t.ly/api/v1/link/shorten", requestContent);


            if (!response.IsSuccessStatusCode)
            {

                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка API: {response.StatusCode} - {errorMessage}");
            }


            var responseContent = await response.Content.ReadAsStringAsync();


            var result = JsonSerializer.Deserialize<TlyApiResponse>(responseContent);

            
            return result?.ShortUrl;
        }
        
        public string DecodeUrl(string hashedUrl)
        {
            try
            {
                var data = Convert.FromBase64String(hashedUrl);
                return Encoding.UTF8.GetString(data);
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid base64 string.");
            }
        }

        public string GenerateShortUrl(string originalUrl)
        {
            var guid = Guid.NewGuid().ToString().Substring(0, 8);  
            return guid;
           
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUrls()
        {
            var urls = await _urlService.GetUrlsAsync(); 
            return Ok(urls);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUrlById(int id)
        {
            var url = await _urlService.GetUrlByIdAsync(id); 
            if (url == null)
            {
                return NotFound("URL not found.");
            }
            return Ok(url);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrl(int id)
        {
            var url = await _urlService.GetUrlByIdAsync(id); 
            if (url == null)
            {
                return NotFound("URL not found.");
            }

            await _urlService.DeleteUrlAsync(id); 
            await _hubContext.Clients.All.SendAsync("UpdateUrls", $"URL deleted");
            return Ok(new { message = "URL deleted successfully." });
        }
    }
}
