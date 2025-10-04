using HotelBooking.Core.Models.Entities;

public class Room
{
    public int Id { get; set; }  
    public int Capacity { get; set; }  
    public int HotelId { get; set; }   
    public Hotel Hotel { get; set; }  
    public List<Booking> Bookings { get; set; } = new List<Booking>();
}