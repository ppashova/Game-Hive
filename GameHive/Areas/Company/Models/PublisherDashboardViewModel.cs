﻿using GameHive.Models;

namespace GameHive.Areas.Company.Models
{
    public class PublisherDashboardViewModel
    {
        public List<Game> PublishedGames { get; set; } = new List<Game>();
        public int TotalGames { get; set; }
        public decimal TotalSales { get; set; }
        public List<GameRequest> GameRequests { get; set; } = new List<GameRequest>();
        public List<Game> TopGames { get; set; } = new List<Game>();

        public List<OrderDetail> PendingApprovals { get; set; }
        public int TotalPendingApprovals => PendingApprovals?.Count ?? 0;
        public int TotalApprovedOrders { get; set; }
        public int TotalRejectedOrders { get; set; }
    }
}
