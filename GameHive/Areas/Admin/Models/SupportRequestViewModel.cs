namespace GameHive.Areas.Admin.Models
{
    public class SupportRequestViewModel
    {
        public string RequestId { get; set; }
        public string Subject { get; set; }
        public string ProblemDescription { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
    }
}
