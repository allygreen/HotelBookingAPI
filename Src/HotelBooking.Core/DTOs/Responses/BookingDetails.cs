using HotelBooking.Core.DTOs.Requests;

namespace HotelBooking.Core.DTOs.Responses;

public class BookingDetails
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string CustomerName { get; set; }
    public int NumberOfGuests { get; set; }   
    public string BookingReference { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public Room Room { get; set; }
    public string HotelName { get; set; }
}