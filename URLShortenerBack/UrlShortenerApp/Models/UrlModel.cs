using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace URL_Shortener.Models
{
    public class UrlModel
    {
        [Key]
        public int Id { get; set; } 
        
        [Required]
        [Url]
        [MaxLength(2048)]
        public string OriginalUrl { get; set; } 
        
        [Required]
        [MaxLength(50)]
        public string ShortUrl { get; set; } 
        
        [Required]
        public string CreatedBy { get; set; } 
        
        [Required]
        public DateTime CreatedDate { get; set; } 

        [ForeignKey("User")]
        public int? UserId { get; set; } 
        public virtual UserModel User { get; set; } 
    }
}
