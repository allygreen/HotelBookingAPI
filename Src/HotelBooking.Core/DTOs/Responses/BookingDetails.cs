namespace HotelBooking.Core.DTOs.Responses;

public class BookingDetails
{
    public int id { get; set; }
    public int RoomId { get; set; }
    public string CustomerName { get; set; }
    public int NumberOfGuests { get; set; }   
    public string BookingReference { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
}