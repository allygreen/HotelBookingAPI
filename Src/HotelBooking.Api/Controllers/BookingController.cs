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
        var response = _bookingService.BookRoom(createBookingRequest);
        return Ok(response);
    }
    
    [Route("hotelsearch/{hotelName}")]
    [HttpGet]
    public async Task<IActionResult> SearchHotelsByName(string hotelName)
    {
        var hotel = await _hotelService.SearchHotels(hotelName);
        return Ok(hotel);
    }
    
    [HttpPost]
    [Route("hotels")]
    public async Task<IActionResult> AddHotel(CreateHotelRequest createHotelRequest)
    {
        await _hotelService.AddAsync(createHotelRequest);
        return Ok();
    }
    
    [Route("details/{bookingReference}")]
    [HttpGet]
    public async Task<IActionResult> GetBookingDetails(string bookingReference)
    {
        var booking = await _bookingService.GetBookingByReference(bookingReference);
        return Ok(booking);
    }
    
    [Route("availability/{startDate}/{endDate}/{guestsCount}")]
    [HttpGet]
    public async Task<IActionResult> GetAvailableRoomsForDateRange(DateTime start, DateTime end, int guestsCount)
    {
        var availableRooms = _bookingService.GetAvailableRooms(start, end, guestsCount);
        return Ok(availableRooms);
    }
}