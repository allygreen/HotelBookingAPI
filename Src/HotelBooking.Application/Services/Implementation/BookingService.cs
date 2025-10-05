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
        var roomExists = await _roomRepository.GetByIdAsync(createBookingRequest.RoomId);
        if (roomExists == null)
        {
            _logger.LogInformation("Room does not exist");
            return new BookingResponse()
            {
                Success = false
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
                Success = false
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


    public Task<List<Room>> GetAvailableRooms(DateTime checkIn, DateTime checkOut, int capacity)
    {
        var rooms = _roomRepository.GetAvailableRooms(checkIn, checkOut, capacity);
        var roomsResponse = _mapper.Map<List<Room>>(rooms.Result);
        
        return Task.FromResult(roomsResponse);
    }
    
    public static string GenerateBookingRef(int length = 6)
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; 
        return "HB-" + new string(Enumerable.Range(0, length)
            .Select(_ => chars[Random.Shared.Next(chars.Length)]).ToArray());
    }

}