using AutoMapper;
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
        
        // Load the navigation properties after save
        return await _context.Bookings
            .Include(b => b.Room)
            .ThenInclude(r => r.Hotel)
            .FirstAsync(b => b.Id == booking.Id);
    }
    
    public async Task<Booking?> GetByBookingReference(string bookingReference)
    {
        var booking = await _context.Bookings
            .Include(b=> b.Room)
            .ThenInclude(r=> r.Hotel)
            .SingleOrDefaultAsync(b => b.BookingReference == bookingReference);
        return booking;
    }
    
    public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
    {
        var available = await _context.Bookings
            .Where(b => b.RoomId == roomId)
            .Where(b => b.CheckIn < checkOut && b.CheckOut > checkIn) 
            .AnyAsync();
            
        return !available; 
        
    }
}