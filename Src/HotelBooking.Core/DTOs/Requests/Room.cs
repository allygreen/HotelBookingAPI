using HotelBooking.Core.Enums;

namespace HotelBooking.Core.DTOs.Requests;

public class Room
{
    public int Id { get; set; }  
    public int Capacity { get; set; }  
    public int HotelId { get; set; }   
    public RoomType RoomType { get; set; } 
    public CreateHotelRequest Hotels { get; set; }  
    public List<CreateBookingRequest> Bookings { get; set; } = new List<CreateBookingRequest>();
}