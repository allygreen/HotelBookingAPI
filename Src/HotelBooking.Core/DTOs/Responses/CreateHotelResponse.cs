using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Core.DTOs.Responses;

public class CreateHotelResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
}