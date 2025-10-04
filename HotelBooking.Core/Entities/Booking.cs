namespace HotelBooking.Core.Models.Entities;

public class Booking
{
    public int id { get; set; }
    public int RoomId { get; set; }
    public int CustomerName { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    
}