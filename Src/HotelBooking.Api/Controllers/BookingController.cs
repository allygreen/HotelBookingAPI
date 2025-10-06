using HotelBooking.Application.Services.Interfaces;
using HotelBooking.Core.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> _logger;
    private readonly IHotelService _hotelService;
    private readonly IBookingService _bookingService;
    public BookingController(
        ILogger<BookingController> logger, 
        IHotelService hotelService,
        IBookingService bookingService)
    {
        _logger = logger;
        _hotelService = hotelService;
        _bookingService = bookingService;
    }
    
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> NewBooking(CreateBookingRequest createBookingRequest)
    {
        var response = await _bookingService.BookRoom(createBookingRequest);
        return Ok(response);
    }
    
    [Route("details/{bookingReference}")]
    [HttpGet]
    public async Task<IActionResult> GetBookingDetails(string bookingReference)
    {
        var booking = await _bookingService.GetBookingByReference(bookingReference);
        return Ok(booking);
    }
    
    [HttpGet]
    [Route("availability")]
    public async Task<IActionResult> GetAvailableRooms(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate, 
        [FromQuery] int guestsCount)
    {
        var availableRooms = await _bookingService.GetAvailableRooms(startDate, endDate, guestsCount);
        return Ok(availableRooms);
    }
}