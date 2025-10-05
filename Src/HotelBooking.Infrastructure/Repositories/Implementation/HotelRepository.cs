using HotelBooking.Core.Entities;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Repositories.Interfaces;

namespace HotelBooking.Infrastructure.Repositories.Implementation;

public class HotelRepository : IHotelRepository
{
    HotelBookingDbContext _context;

    public HotelRepository(HotelBookingDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(Hotel hotel)
    {
        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Hotel>> SearchHotelNameAsync(string hotelName)
    {
        var hotels = _context.Hotels
            .Where(h => h.Name.ToLower() == hotelName.ToLower());
        return await Task.FromResult(hotels.ToList());
    }
}