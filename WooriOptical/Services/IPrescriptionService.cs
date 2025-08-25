using WooriOptical.Models;

namespace WooriOptical.Services;

public interface IPrescriptionService
{
    Task<List<Prescription?>> GetPrescriptionByCustomerIdAsync(Guid customerId);
    Task<bool> AddPrescriptionAsync(Prescription prescription);
    Task<Prescription?> GetPrescriptionByIdAsync(Guid id);
}
