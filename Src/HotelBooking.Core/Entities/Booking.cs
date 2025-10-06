using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Core.Entities;

public class Booking
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int RoomId { get; set; }
    [Required]
    public Room Room { get; set; }
    [Required]   
    public string CustomerName { get; set; }
    [Required]
    public string NumberOfGuests { get; set; }
    [Required]  
    public string BookingReference { get; set; }
    public DateTime CheckIn { get; set; }
    [Required] 
    public DateTime CheckOut { get; set; }
}