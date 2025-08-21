namespace WooriOptical.Models;

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
