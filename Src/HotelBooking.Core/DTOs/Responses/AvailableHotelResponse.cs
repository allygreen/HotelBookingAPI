namespace HotelBooking.Core.DTOs.Responses;

public class AvailableHotelResponse
{
    public int HotelId { get; set; }
    public string HotelName { get; set; }
    public string HotelCity { get; set; }
    public List<AvailableRoomResponse> AvailableRooms { get; set; } = new();
}

