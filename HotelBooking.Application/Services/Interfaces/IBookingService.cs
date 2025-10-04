namespace HotelBooking.Application.Services.Interfaces;

public interface IBookingService
{
    public Task<bool> BookRoom(int roomId, int customerId, DateTime checkIn, DateTime checkOut);
}