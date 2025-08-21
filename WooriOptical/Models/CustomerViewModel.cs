
namespace WooriOptical.Models
{
    public class CustomerViewModel
    {
        public Guid CustomerId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public List<Prescription>? Prescriptions { get; set; }
        public List<Order>? Orders { get; set; }
	}
}
