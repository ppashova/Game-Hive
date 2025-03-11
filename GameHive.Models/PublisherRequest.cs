using GameHive.Models.enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Models
{
    public class PublisherRequest
    {
        [Key]
        public int RequestId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        [Required]
        public DateOnly Birthdate { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public RequestEnums RequestEnums { get; set; } = RequestEnums.Pending;
    }
}
