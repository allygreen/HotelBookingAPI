using HotelBooking.Application.Services.Interfaces;
using HotelBooking.Core.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;
    public HotelsController(
        ILogger<BookingController> logger, 
        IHotelService hotelService)
    {
        _hotelService = hotelService;
    }
    
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> AddHotel(CreateHotelRequest createHotelRequest)
    {
        var response = await _hotelService.AddAsync(createHotelRequest);
        return Ok(response);
    }
    
    [Route("search/{hotelName}")]
    [HttpGet]
    public async Task<IActionResult> SearchHotelsByName(string hotelName)
    {
        var hotel = await _hotelService.SearchHotels(hotelName);
        if (hotel.Count == 0)
        {
            return NotFound();
        }
        return Ok(hotel);
    }
}