using System.ComponentModel.DataAnnotations;
using HotelBooking.Core.Enums;

namespace HotelBooking.Core.Entities;
public class Room
{
    [Key]
    public int Id { get; set; }  
    [Required]
    public int Capacity { get; set; }  
    [Required] 
    public int HotelId { get; set; }   
    [Required]
    public Hotel Hotel { get; set; }  
    [Required]
    public RoomType RoomType { get; set; } 
    public List<Booking> Bookings { get; set; } = new List<Booking>();
}