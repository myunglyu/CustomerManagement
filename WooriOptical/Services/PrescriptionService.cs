using Microsoft.EntityFrameworkCore;
using WooriOptical.Models;

namespace WooriOptical.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly AppDbContext _context;

    public PrescriptionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Prescription>> GetPrescriptionByCustomerIdAsync(Guid customerId)
    {
        List<Prescription> prescriptions = await _context.Prescriptions
            .Where(p => p.CustomerId == customerId)
            .ToListAsync();

        return prescriptions;
    }

    public async Task<bool> AddPrescriptionAsync(Prescription prescription)
    {
        _context.Prescriptions.Add(prescription);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<Prescription?> GetPrescriptionByIdAsync(Guid id)
    {
        return await _context.Prescriptions.FindAsync(id);
    }
}
