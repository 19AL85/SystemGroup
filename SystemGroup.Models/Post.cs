using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemGroup.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = "";
        [Required]
        public string Text { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public string IdentityUserId { get; set; }
        [Required]
        [ForeignKey("IdentityUserId")]
        public virtual IdentityUser IdentityUser { get; set; }
        
        
    }
}
