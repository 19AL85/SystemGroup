using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemGroup.Models.Requests
{
    public class PostRequest
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = "";
        [Required]
        public string Text { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
