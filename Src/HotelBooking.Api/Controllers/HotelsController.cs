using HotelBooking.Application.Services.Interfaces;
using HotelBooking.Core.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;
    private readonly ILogger<HotelsController> _logger;
    public HotelsController(IHotelService hotelService, ILogger<HotelsController> logger)
    {
        _hotelService = hotelService;
        _logger = logger;
    }
    
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> AddHotelAsync([FromBody]CreateHotelRequest createHotelRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _hotelService.AddAsync(createHotelRequest);
        return Ok(response);
    }
    
    [Route("search/{hotelName}")]
    [HttpGet]
    public async Task<IActionResult> SearchHotelsByNameAsync(string hotelName)
    {
        var hotel = await _hotelService.SearchHotels(hotelName);
        if (hotel.Count == 0)
        {
            _logger.LogWarning("No hotel found for {hotelName}", hotelName);
            return NotFound();
        }
        return Ok(hotel);
    }
}