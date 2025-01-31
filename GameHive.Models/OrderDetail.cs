using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Models
{
    public class OrderDetail
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        
    }
}
