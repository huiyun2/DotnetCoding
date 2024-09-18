namespace DotnetCoding.Core.Models
{
    public class ApprovalQueue
    {
        public string RequestReason { get; set; }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Reason { get; set; }
        public DateTime RequestDate { get; set; }
        public string State { get; set; } // Create, Update, Delete
    }

}
