using System.ComponentModel.DataAnnotations;
using HotelBooking.Core.DTOs.Requests;

namespace HotelBooking.Core.DTOs.Responses;

public class SearchHotelResponse
{
    public string Name { get; set; }
    
    public string City { get; set; }  
    
    public List<RoomResponse> Rooms { get; set; } = new ();
}