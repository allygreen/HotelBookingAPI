using AutoMapper;
using HotelBooking.Application.Services.Interfaces;
using HotelBooking.Core.DTOs.Requests;
using HotelBooking.Core.DTOs.Responses;
using HotelBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Application.Services.Implementation;

public class BookingService : IBookingService
{
    private IBookingRepository _bookingRepository;
    private IRoomRepository _roomRepository;
    private ILogger<BookingService> _logger;
    private IMapper _mapper;
    public BookingService(
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        ILogger<BookingService> logger, 
        IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<BookingResponse> BookRoom(CreateBookingRequest createBookingRequest)
    {
        var room = await _roomRepository.GetByIdAsync(createBookingRequest.RoomId);
        if (room == null)
        {
            _logger.LogInformation("Room does not exist");
            return new BookingResponse()
            {
                Success = false,
                Message = "Room not found" 
            };       
        }
        
        var isAvailable = await _bookingRepository.IsRoomAvailableAsync(
            createBookingRequest.RoomId, 
            createBookingRequest.CheckIn, 
            createBookingRequest.CheckOut);
        
        if (!isAvailable)
        {
            _logger.LogInformation("Room is not available");
            return new BookingResponse()
            {
                Success = false,
                Message = "Room is not available on requested dates"           
            };               
        }
        
        var roomCapacity = await _roomRepository.GetRoomCapacityAsync(createBookingRequest.RoomId);
        if (createBookingRequest.NumberOfGuests > roomCapacity)
        {
            _logger.LogInformation("Too many guests for the room size");
            return new BookingResponse()
            {
                Success = false
            };       ;       
        }
        
        var bookingEntity = _mapper.Map<Core.Entities.Booking>(createBookingRequest);    
        bookingEntity.BookingReference = GenerateBookingRef();
        var bookRoom = await _bookingRepository.AddAsync(bookingEntity);
        _logger.LogInformation("Booking created");

        var bookingResponse = new BookingResponse()
        {
            Booking = _mapper.Map<BookingDetails>(bookRoom),
            Success = true
        };

        return bookingResponse;
    }

    public Task<BookingResponse> GetBookingByReference(string bookingReference)
    {
        
        var booking = _bookingRepository.GetByBookingReference(bookingReference);
        var bookingDetails = _mapper.Map<BookingDetails>(booking.Result);
        var bookingResponse = new BookingResponse()
        {
            Booking = bookingDetails,
            Success = true
        };
        
        return Task.FromResult(bookingResponse);
    }


    public async Task<List<AvailableHotelResponse>> GetAvailableRooms(DateTime checkIn, DateTime checkOut, int capacity)
    {
        var rooms = await _roomRepository.GetAvailableRooms(checkIn, checkOut, capacity);
        _logger.LogInformation("Finding available rooms");
        
        // Group rooms by hotel
        var hotelsWithRooms = rooms
            .GroupBy(r => r.Hotel)
            .Select(hotelGroup => new AvailableHotelResponse
            {
                HotelId = hotelGroup.Key.Id,
                HotelName = hotelGroup.Key.Name,
                HotelCity = hotelGroup.Key.City,
                AvailableRooms = hotelGroup.Select(room => new AvailableRoomResponse
                {
                    RoomId = room.Id,
                    Capacity = room.Capacity,
                    RoomType = room.RoomType
                }).ToList()
            })
            .ToList();
        
        return hotelsWithRooms;
    }
    
    private static string GenerateBookingRef(int length = 10)
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; 
        return "HB-" + new string(Enumerable.Range(0, length)
            .Select(_ => chars[Random.Shared.Next(chars.Length)]).ToArray());
    }

}