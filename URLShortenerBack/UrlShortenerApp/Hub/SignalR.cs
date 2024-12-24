using Microsoft.AspNetCore.SignalR;
using URL_Shortener.Models;

namespace URL_Shortener.Hub
{
    public class UrlUpdateHub : Microsoft.AspNetCore.SignalR.Hub
    {

        public async Task SendUpdatedUrls(List<UrlModel> updatedUrls)
        {
            await Clients.All.SendAsync("ReceiveUpdatedUrls", updatedUrls);
        }
    }
}