using AutoMapper;
using HotelBooking.Application.Services.Interfaces;
using HotelBooking.Core.DTOs.Requests;
using HotelBooking.Core.DTOs.Responses;
using HotelBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Application.Services.Implementation;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<HotelService> _logger;
    public HotelService(IHotelRepository hotelRepository, IMapper mapper, ILogger<HotelService> logger)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<CreateHotelResponse> AddAsync(CreateHotelRequest createHotelRequest)
    {
        if (createHotelRequest.Rooms.Count != 6)
        {
            _logger.LogWarning("Hotel must have exactly 6 rooms, but {Count} rooms provided for Hotel {.Name}", createHotelRequest.Rooms.Count, createHotelRequest.Name);
            return new CreateHotelResponse()
            {
                Success = false,
                Message = $"Hotel must have exactly 6 rooms, but {createHotelRequest.Rooms.Count} rooms provided"
            };
        }

        var invalidRooms = createHotelRequest.Rooms.Where(r => r.Capacity <= 0).ToList();
        if (invalidRooms.Any())
        {
            _logger.LogWarning("All rooms must have a valid capacity greater than 0, but {Rooms} rooms have invalid capacity", invalidRooms.Count);
            return new CreateHotelResponse()
            {
                Success = false,
                Message = "All rooms must have a valid capacity greater than 0"
            };
        }

        var mappedHotel = _mapper.Map<Core.Entities.Hotel>(createHotelRequest);
        var createdHotel = await _hotelRepository.AddAsync(mappedHotel);
        _logger.LogInformation("Hotel created");
        return new CreateHotelResponse()
        {
            Success = true,
            Message = "Hotel created successfully",
            Name = createdHotel.Name
        };
    }
    
    public async Task<List<SearchHotelResponse>> SearchHotels(string hotelName)
    {
        var hotels = await _hotelRepository.SearchHotelNameAsync(hotelName);
        _logger.LogInformation("Hotel searched");
        var returnHotels = _mapper.Map<List<SearchHotelResponse>>(hotels);
        return returnHotels;
    }
}