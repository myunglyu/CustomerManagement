namespace WooriOptical.Models;

public class Customer
{
    public Guid CustomerId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}

public class Prescription
{
    public Guid PrescriptionId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime DateIssued { get; set; }
    public double RSphere { get; set; }
    public double RCylinder { get; set; }
    public double RAxis { get; set; }
    public double LSphere { get; set; }
    public double LCylinder { get; set; }
    public double LAxis { get; set; }
    public double PD { get; set; }
    public double Add { get; set; }
    public string? Notes { get; set; }
}

public class Order
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
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
    public string? PayoffStatus { get; set; }
}
