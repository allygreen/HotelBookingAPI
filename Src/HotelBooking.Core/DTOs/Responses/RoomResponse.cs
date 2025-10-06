using HotelBooking.Core.Enums;

namespace HotelBooking.Core.DTOs.Responses;

public class RoomResponse
{
    public int Id { get; set; }
    
    public int Capacity { get; set; }  
    
    public RoomType RoomType { get; set; } 
}