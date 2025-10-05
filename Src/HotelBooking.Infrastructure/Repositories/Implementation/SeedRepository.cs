using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Repositories.Interfaces;

namespace HotelBooking.Infrastructure.Repositories.Implementation;

public class SeedRepository : ISeedRepository
{
    private readonly HotelBookingDbContext _context;

    public SeedRepository(HotelBookingDbContext context)
    {
        _context = context;
    }
    
    public async Task DeleteAllDataAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
    }
}