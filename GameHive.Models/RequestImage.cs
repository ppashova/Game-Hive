using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Models
{
    public class RequestImage
    {
        public int RequestId { get; set; }
        public GameRequest Request { get; set; }
        public string ImageUrl { get; set; }
    }
}
