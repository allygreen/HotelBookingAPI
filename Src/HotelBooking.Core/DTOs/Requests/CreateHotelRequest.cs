namespace HotelBooking.Core.DTOs.Requests;

public class CreateHotelRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }  
    public List<Room> Rooms { get; set; } = new List<Room>();
}