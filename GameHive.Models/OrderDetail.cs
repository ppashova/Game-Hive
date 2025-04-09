using GameHive.Models.enums;
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
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public PublisherApprovalStatusEnums ApprovalStatus { get; set; } = PublisherApprovalStatusEnums.Pending;
        public DateTime? ApprovalDate { get; set; }
    }
}
