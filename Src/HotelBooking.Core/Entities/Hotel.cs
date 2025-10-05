using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Core.Entities;

public class Hotel
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string City { get; set; }  
    public List<Room> Rooms { get; set; } = new List<Room>();
}