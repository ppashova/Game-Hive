namespace GameHive.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int GameUploadRequestsCount { get; set; }
        public int PublisherRequestsCount { get; set; }
        public int OtherRequestsCount { get; set; }
        public int TotalRequests => GameUploadRequestsCount + PublisherRequestsCount + OtherRequestsCount;
    }
}
