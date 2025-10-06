using HotelBooking.Core.Entities;

namespace HotelBooking.Infrastructure.Repositories.Interfaces;

public interface IHotelRepository
{
    public Task<Hotel> AddAsync(Hotel hotel);
    public Task<List<Hotel>> SearchHotelNameAsync(string hotelName);
    
    
}