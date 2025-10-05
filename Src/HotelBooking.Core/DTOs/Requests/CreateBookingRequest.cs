namespace HotelBooking.Core.DTOs.Requests;

public class CreateBookingRequest
{
    public int id { get; set; }
    public int RoomId { get; set; }
    public string CustomerName { get; set; }
    public int NumberOfGuests { get; set; }   
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    
}