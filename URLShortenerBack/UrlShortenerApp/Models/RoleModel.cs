using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace URL_Shortener.Models
{
    public class RoleModel
    {
        [Key]
        public int Id { get; set; } 
        
        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; } 

        public virtual ICollection<UserModel> Users { get; set; } 
    }
}
