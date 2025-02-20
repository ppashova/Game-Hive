using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Models
{
    public class Cart
    {
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public int GameId { get; set; }
        public DateTime DateCreated { get; set; }
        public int Quantity { get; set; }
        //Nav property
        public virtual Game Game { get; set; }
    }
}
