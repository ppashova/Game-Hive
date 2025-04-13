using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Models
{
    public class SupportRequest
    {
        [Key]
        public Guid RequestId { get; set; } = new Guid();
        [Required]
        public string Subject { get; set; }
        [Required]
        public string ProblemDescription { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
