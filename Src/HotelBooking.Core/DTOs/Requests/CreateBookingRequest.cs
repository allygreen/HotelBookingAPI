using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Core.DTOs.Requests;

public class CreateBookingRequest
{
    [Required(ErrorMessage = "RoomId is required")]
    public int RoomId { get; set; }
    [Required(ErrorMessage = "CustomerName is required")]
    public string CustomerName { get; set; }
    [Required(ErrorMessage = "NumberOfGuests is required")]
    public int NumberOfGuests { get; set; }   
    [Required(ErrorMessage = "CheckIn is required")]
    public DateTime CheckIn { get; set; }
    [Required(ErrorMessage = "CheckOut is required")]
    public DateTime CheckOut { get; set; }
    
}