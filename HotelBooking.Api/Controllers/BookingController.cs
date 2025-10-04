using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> _logger;
    public BookingController(ILogger<BookingController> logger)
    {
        _logger = logger;
    }
    
    [HttpPost]
    [Route("/")]
    public async Task<IActionResult> NewBooking()
    {
        return Ok();
    }
    
    [Route("/hotelsearch/{hotelName}")]
    [HttpGet]
    public async Task<IActionResult> GetHotel(string hotelName)
    {
        return Ok(hotelName);
    }
    
    [Route("{controller}/details")]
    [HttpGet]
    public async Task<IActionResult> GetBookingDetails()
    {
        return Ok();
    }
    
    [Route("/availability/{startDate}/{endDate}")]
    [HttpGet]
    public async Task<IActionResult> GetAvailableRoomsForDateRange()
    {
        return Ok();
    }
}