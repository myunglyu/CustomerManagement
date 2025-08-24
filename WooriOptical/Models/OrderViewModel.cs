
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WooriOptical.Models
{
    public class OrderViewModel
    { 
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid PrescriptionId { get; set; }
        public double Height { get; set; }
        public string? Frame { get; set; }
        public string? FramePrice { get; set; }
        public string? Lens { get; set; }
        public string? LensPrice { get; set; }
        public double TotalAmount { get; set; }
        public string? Discount { get; set; }
        public string? FinalAmount { get; set; }
        public string? Deposit { get; set; }
        public string? Balance { get; set; }
        public string? BalancePaid { get; set; }
        public string? PayoffStatus { get; set; }
    }
}
