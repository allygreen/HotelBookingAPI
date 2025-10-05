namespace HotelBooking.Infrastructure.Repositories.Interfaces;

public interface ISeedRepository
{
    public Task DeleteAllDataAsync();
}