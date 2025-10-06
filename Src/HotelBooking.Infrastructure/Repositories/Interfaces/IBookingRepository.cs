using HotelBooking.Core.Entities;

namespace HotelBooking.Infrastructure.Repositories.Interfaces;

public interface IBookingRepository
{
    public Task<Booking> AddAsync(Booking booking);
    
    public Task<Booking?> GetByBookingReference(string bookingReference); 

    public Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut);
    
}