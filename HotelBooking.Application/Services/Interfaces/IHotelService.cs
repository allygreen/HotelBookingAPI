namespace HotelBooking.Application.Services.Interfaces;

public interface IHotelService
{
    public Task<Hote> GetHotel();
}