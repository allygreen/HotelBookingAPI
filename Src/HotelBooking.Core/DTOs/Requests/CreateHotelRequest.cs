using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Core.DTOs.Requests;

public class CreateHotelRequest
{
    [Required(ErrorMessage = "Hotel name is required")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "City is required")]
    public string City { get; set; }  
    
    [Required(ErrorMessage = "No rooms added to hotel")]
    [MinLength(6, ErrorMessage = "Hotel must have exactly 6 rooms")]
    [MaxLength(6, ErrorMessage = "Hotel must have exactly 6 rooms")]
    public List<Room> Rooms { get; set; } = new List<Room>();
}