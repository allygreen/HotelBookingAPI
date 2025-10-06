using HotelBooking.Application.Services.Interfaces;
using HotelBooking.Core.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<BookingController> _logger;
    public BookingController(
        IBookingService bookingService,
        ILogger<BookingController> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }
    
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> NewBookingAsync([FromBody]CreateBookingRequest createBookingRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _bookingService.BookRoomAsync(createBookingRequest);
        if (response.Success)
        {
            return Ok(response);
        }
        _logger.LogWarning("Booking failed for Room {RoomId}. Reason: {Reason}", 
            createBookingRequest.RoomId, response.Message);
        return Conflict(response);
    }
    
    [Route("details/{bookingReference}")]
    [HttpGet]
    public async Task<IActionResult> GetBookingDetailsAsync(string bookingReference)
    {
        var booking = await _bookingService.GetBookingByReferenceAsync(bookingReference);
        if (booking.Success)
        {
            return Ok(booking);
        }
        
        _logger.LogWarning("No booking found for  {bookingReference}", bookingReference);
        return NotFound(booking);
    }
    
    [HttpGet]
    [Route("availability")]
    public async Task<IActionResult> GetAvailableRoomsAsync(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate, 
        [FromQuery] int guestsCount)
    {
        if (startDate >= endDate)
        {
            return BadRequest("Start date must be before end date");
        }

        if (guestsCount <= 0)
        {
            return BadRequest("Guests count must be greater than 0");
        }

        var availableRooms = await _bookingService.GetAvailableRoomAsync(startDate, endDate, guestsCount);
        return Ok(availableRooms);
    }
}