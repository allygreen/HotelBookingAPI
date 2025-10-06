using HotelBooking.Core.DTOs.Requests;
using HotelBooking.Core.DTOs.Responses;

namespace HotelBooking.Application.Services.Interfaces;

public interface IBookingService
{
    public Task<BookingResponse> BookRoomAsync(CreateBookingRequest createBookingRequest);
    
    public Task<BookingResponse> GetBookingByReferenceAsync(string bookingReference);
    
    public Task<List<AvailableHotelResponse>> GetAvailableRoomAsync(DateTime checkIn, DateTime checkOut, int capacity);
}