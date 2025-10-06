using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        var bookings = await _context.Bookings.ToListAsync();
        _context.Bookings.RemoveRange(bookings);
        
        var rooms = await _context.Rooms.ToListAsync();
        _context.Rooms.RemoveRange(rooms);
        
        var hotels = await _context.Hotels.ToListAsync();
        _context.Hotels.RemoveRange(hotels);
        
        await _context.SaveChangesAsync();
    }
}