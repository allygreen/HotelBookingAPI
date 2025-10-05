using HotelBooking.Core.Entities;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories.Implementation;

public class BookingRepository : IBookingRepository
{
    public HotelBookingDbContext _context;
    public BookingRepository(HotelBookingDbContext context)
    {
        _context = context;
    }
    
    public async Task<Booking> AddAsync(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<Booking> GetByIdAsync(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        return booking;
    }
    
    public async Task<Booking> GetByBookingReference(string bookingReference)
    {
        var booking = _context.Bookings.SingleOrDefault(b=> b.BookingReference == bookingReference);
        return booking;
    }

    public async Task<List<Booking>> GetByRoomIdAsync(int roomId)
    {
        var roomBookings =  _context.Bookings.Where(r => r.RoomId == roomId);
        return roomBookings.ToList();
    }
    
    public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
    {
        var available = await _context.Bookings
            .Where(b => b.RoomId == roomId)
            .Where(b => b.CheckIn < checkOut && b.CheckOut > checkIn)  // Overlap detection
            .AnyAsync();
            
        return !available; 
        
    }
}