using HotelBooking.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SeedDataController : ControllerBase
{
    private readonly ISeedService _seedService;
    
    public SeedDataController(ISeedService seedService)
    {
        _seedService = seedService;
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteTestDataAsync()
    {
        await _seedService.DeleteAllDataAsync();
        return Ok("All test data deleted successfully.");
    }

    [HttpPost]
    public async Task<IActionResult> SeedTestDataAsync()
    {
        await _seedService.SeedTestDataAsync();
        return Ok("Test data seeded successfully with 5 hotels and 30 rooms total.");
    }

    [HttpPost]
    [Route("reset")]
    public async Task<IActionResult> ResetAllDataAsync()
    {
        await _seedService.ResetAllDataAsync();
        return Ok("Database reset and reseeded successfully.");
    }
    
}