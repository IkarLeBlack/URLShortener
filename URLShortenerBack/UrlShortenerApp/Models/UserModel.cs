using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace URL_Shortener.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; } 
        
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } 
        
        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } 
        
        [Required]
        public int RoleId { get; set; } 
        public virtual RoleModel Role { get; set; } 

        public virtual ICollection<UrlModel> Urls { get; set; } 
    }
}
