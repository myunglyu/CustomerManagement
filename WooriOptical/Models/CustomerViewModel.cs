
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WooriOptical.Models
{
    public class CustomerViewModel
    {
        public Guid CustomerId { get; set; }
        
        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        // Validate common email format; allow empty (not required) but validate when provided
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters.")]
        public string? Email { get; set; }

        // Stored as digits-only, but allow common phone input formats client-side (e.g. (123) 456-7890, 123-456-7890)
        // Validation will accept optional +1 country code and common separators; final normalization occurs in controller.
        [RegularExpression(@"^(\+1[\s-]?)?(\(?\d{3}\)?[\s.-]?)\d{3}[\s.-]?\d{4}$", ErrorMessage = "Please enter a valid US phone number.")]
        public string? Phone { get; set; }

        [StringLength(200, ErrorMessage = "Street address cannot exceed 200 characters.")]
        public string? Street { get; set; }
        
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        public string? City { get; set; }
        
        [StringLength(50, ErrorMessage = "State cannot exceed 50 characters.")]
        public string? State { get; set; }
        
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Please enter a valid ZIP code (12345 or 12345-1234).")]
        public string? Zip { get; set; }
        public List<Prescription>? Prescriptions { get; set; }
        public List<Order>? Orders { get; set; }

        // Read-only display property, formats 10-digit phone as (123) 456-7890
        public string? FormattedPhone
        {
            get
            {
                if (string.IsNullOrEmpty(Phone))
                    return null;
                var digits = new string(Phone.Where(char.IsDigit).ToArray());
                if (digits.Length == 10)
                    return $"({digits.Substring(0,3)}) {digits.Substring(3,3)}-{digits.Substring(6,4)}";
                return Phone; // fallback
            }
        }

        // Read-only formatted address, e.g. "123 Main St, Seattle, WA 98101"
        public string? FormattedAddress
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Street) && string.IsNullOrWhiteSpace(City) && string.IsNullOrWhiteSpace(State) && string.IsNullOrWhiteSpace(Zip))
                    return null;

                var parts = new List<string>();
                if (!string.IsNullOrWhiteSpace(Street)) parts.Add(Street.Trim());
                var cityStateZip = new List<string>();
                if (!string.IsNullOrWhiteSpace(City)) cityStateZip.Add(City.Trim());
                if (!string.IsNullOrWhiteSpace(State)) cityStateZip.Add(State.Trim());
                var cs = string.Join(", ", cityStateZip);
                if (!string.IsNullOrWhiteSpace(cs)) parts.Add(cs);
                if (!string.IsNullOrWhiteSpace(Zip)) parts.Add(Zip.Trim());

                return string.Join(", ", parts);
            }
        }
    }
}
