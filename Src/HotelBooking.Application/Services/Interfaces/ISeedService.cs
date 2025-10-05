namespace HotelBooking.Application.Services.Interfaces;

public interface ISeedService
{
    Task DeleteAllDataAsync();
    Task SeedTestDataAsync();
    Task ResetAllDataAsync();
}
