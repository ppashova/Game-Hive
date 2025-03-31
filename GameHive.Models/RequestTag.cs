using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Models
{
    public class RequestTag
    {
        public int RequestId { get; set; }
        public GameRequest GameRequest { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
