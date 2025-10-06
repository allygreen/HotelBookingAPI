using HotelBooking.Core.DTOs.Requests;
using HotelBooking.Core.DTOs.Responses;

namespace HotelBooking.Application.Services.Interfaces;

public interface IHotelService
{
    public Task<List<SearchHotelResponse>> SearchHotels(string hotelName);
    
    public Task<CreateHotelResponse> AddAsync(CreateHotelRequest createHotelRequest);
}