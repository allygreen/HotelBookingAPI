using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Core.Entities;

public class Booking
{
    [Key]
    public int id { get; set; }
    [Required]
    public int RoomId { get; set; }
    [Required]   
    public int CustomerName { get; set; }
    [Required]  
    public DateTime CheckIn { get; set; }
    [Required] 
    public DateTime CheckOut { get; set; }
}