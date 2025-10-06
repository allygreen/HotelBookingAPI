using HotelBooking.Core.Entities;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories.Implementation;

public class HotelRepository : IHotelRepository
{
    HotelBookingDbContext _context;

    public HotelRepository(HotelBookingDbContext context)
    {
        _context = context;
    }
    
    public async Task<Hotel> AddAsync(Hotel hotel)
    {
        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();
        return hotel;
    }

    public async Task<List<Hotel>> SearchHotelNameAsync(string hotelName)
    {
        var hotels = _context.Hotels
            .Include(h => h.Rooms)
            .Where(h => h.Name.ToLower().Contains(hotelName.ToLower()));
        return await Task.FromResult(hotels.ToList());
    }
}