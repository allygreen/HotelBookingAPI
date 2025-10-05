using AutoMapper;
using HotelBooking.Application.Services.Interfaces;
using HotelBooking.Core.DTOs.Requests;
using HotelBooking.Infrastructure.Repositories.Interfaces;

namespace HotelBooking.Application.Services.Implementation;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;
    public HotelService(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }
    
    public async Task AddAsync(CreateHotelRequest createHotelRequest)
    {
        var mappedHotel = _mapper.Map<Core.Entities.Hotel>(createHotelRequest);
        await _hotelRepository.AddAsync(mappedHotel);
    }
    
    public async Task<List<CreateHotelRequest>> SearchHotels(string hotelName)
    {
        var hotels = await _hotelRepository.SearchHotelNameAsync(hotelName);

        var returnHotels = _mapper.Map<List<CreateHotelRequest>>(hotels);
        return returnHotels;
    }
}