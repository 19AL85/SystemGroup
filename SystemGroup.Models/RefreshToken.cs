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
    public class RefreshToken
    {
        [Key]
        public string Token { get; set; }

        [Required]
        public string IdentityUserId { get; set; }
        [Required]
        [ForeignKey("IdentityUserId")]
        public virtual IdentityUser IdentityUser { get; set; }
    }
}
