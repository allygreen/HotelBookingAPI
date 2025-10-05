using HotelBooking.Core.DTOs.Requests;

namespace HotelBooking.Application.Services.Interfaces;

public interface IHotelService
{
    public Task<List<CreateHotelRequest>> SearchHotels(string hotelName);
    
    public Task AddAsync(CreateHotelRequest createHotelRequest);
}