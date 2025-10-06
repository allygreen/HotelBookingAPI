namespace HotelBooking.Core.DTOs.Responses;

public class BookingResponse
{
    public bool Success { get; set; }
    
    public string? Message { get; set; }
    public BookingDetails Booking { get; set; }
}