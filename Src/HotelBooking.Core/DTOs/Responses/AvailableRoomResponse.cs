using HotelBooking.Core.Enums;

namespace HotelBooking.Core.DTOs.Responses;

public class AvailableRoomResponse
{
    public int RoomId { get; set; }
    public int Capacity { get; set; }
    public RoomType RoomType { get; set; }
}

