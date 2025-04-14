namespace GameHive.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int GameUploadRequestsCount { get; set; }
        public int PublisherRequestsCount { get; set; }
        public int OtherRequestsCount { get; set; }
        public int TotalRequests { get; set; }
        public List<OrderSummary> RecentOrders { get; set; } = new List<OrderSummary>();
    }

    public class OrderSummary
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
