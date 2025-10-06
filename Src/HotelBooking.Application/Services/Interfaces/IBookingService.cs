using HotelBooking.Core.DTOs.Requests;
using HotelBooking.Core.DTOs.Responses;

namespace HotelBooking.Application.Services.Interfaces;

public interface IBookingService
{
    public Task<BookingResponse> BookRoom(CreateBookingRequest createBookingRequest);
    
    public Task<BookingResponse> GetBookingByReference(string bookingReference);
    
    public Task<List<AvailableHotelResponse>> GetAvailableRooms(DateTime checkIn, DateTime checkOut, int capacity);
}