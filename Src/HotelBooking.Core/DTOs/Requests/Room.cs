using System.ComponentModel.DataAnnotations;
using HotelBooking.Core.Enums;

namespace HotelBooking.Core.DTOs.Requests;

public class Room
{
    [Required(ErrorMessage = "Room capacity is required")]
    [Range(1, 10, ErrorMessage = "Room capacity must be between 1 and 10")]
    public int Capacity { get; set; }  
    
    [Required(ErrorMessage = "Room type is required")]
    public RoomType RoomType { get; set; } 
}