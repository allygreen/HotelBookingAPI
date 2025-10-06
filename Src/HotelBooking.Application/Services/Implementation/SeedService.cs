using HotelBooking.Application.Services.Interfaces;
using HotelBooking.Core.DTOs.Requests;
using HotelBooking.Core.Enums;
using HotelBooking.Infrastructure.Repositories.Interfaces;

namespace HotelBooking.Application.Services.Implementation;

public class SeedService : ISeedService
{
    private readonly IHotelService _hotelService;
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly ISeedRepository _seedRepository;

    public SeedService(
        IHotelService hotelService,
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        IHotelRepository hotelRepository,
        ISeedRepository seedRepository)
    {
        _hotelService = hotelService;
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _hotelRepository = hotelRepository;
        _seedRepository = seedRepository;
    }

    public async Task DeleteAllDataAsync()
    {
        await _seedRepository.DeleteAllDataAsync();
    }

    public async Task SeedTestDataAsync()
    {
        // Hotel 1: Grand Plaza Hotel
        var hotel1 = new CreateHotelRequest()
        {
            Name = "The Prancing Pony",
            City = "Bree",
            Rooms = new List<Room>
            {
                new Room { Capacity = 2, RoomType = RoomType.Double },
                new Room { Capacity = 1, RoomType = RoomType.Single },
                new Room { Capacity = 2, RoomType = RoomType.Deluxe },
                new Room { Capacity = 2, RoomType = RoomType.Double },
                new Room { Capacity = 1, RoomType = RoomType.Single },
                new Room { Capacity = 2, RoomType = RoomType.Deluxe }
            }
        };
        await _hotelService.AddAsync(hotel1);

        // Hotel 2: Ocean View Resort
        var hotel2 = new CreateHotelRequest()
        {
            Name = "The Scotsman",
            City = "Edinburgh",
            Rooms = new List<Room>
            {
                new Room { Capacity = 2, RoomType = RoomType.Deluxe },
                new Room { Capacity = 1, RoomType = RoomType.Single },
                new Room { Capacity = 2, RoomType = RoomType.Double },
                new Room { Capacity = 2, RoomType = RoomType.Deluxe },
                new Room { Capacity = 1, RoomType = RoomType.Single },
                new Room { Capacity = 2, RoomType = RoomType.Double }
            }
        };
        await _hotelService.AddAsync(hotel2);

        // Hotel 3: Mountain Lodge
        var hotel3 = new CreateHotelRequest()
        {
            Name = "The Waldorf",
            City = "Glasgow",
            Rooms = new List<Room>
            {
                new Room { Capacity = 1, RoomType = RoomType.Single },
                new Room { Capacity = 2, RoomType = RoomType.Double },
                new Room { Capacity = 2, RoomType = RoomType.Deluxe },
                new Room { Capacity = 1, RoomType = RoomType.Single },
                new Room { Capacity = 2, RoomType = RoomType.Double },
                new Room { Capacity = 2, RoomType = RoomType.Deluxe }
            }
        };
        await _hotelService.AddAsync(hotel3);

        // Hotel 4: City Center Inn
        var hotel4 = new CreateHotelRequest()
        {
            Name = "The Ritz",
            City = "London",
            Rooms = new List<Room>
            {
                new Room { Capacity = 2, RoomType = RoomType.Double },
                new Room { Capacity = 2, RoomType = RoomType.Deluxe },
                new Room { Capacity = 1, RoomType = RoomType.Single },
                new Room { Capacity = 2, RoomType = RoomType.Double },
                new Room { Capacity = 2, RoomType = RoomType.Deluxe },
                new Room { Capacity = 1, RoomType = RoomType.Single }
            }
        };
        await _hotelService.AddAsync(hotel4);
        
        // Hotel 4: City Center Inn
        var hotel5 = new CreateHotelRequest()
        {
            Name = "Highland Resort",
            City = "Aviemore",
            Rooms = new List<Room>
            {
                new Room { Capacity = 2, RoomType = RoomType.Double },
                new Room { Capacity = 2, RoomType = RoomType.Deluxe },
                new Room { Capacity = 1, RoomType = RoomType.Single },
                new Room { Capacity = 2, RoomType = RoomType.Double },
                new Room { Capacity = 2, RoomType = RoomType.Deluxe },
                new Room { Capacity = 1, RoomType = RoomType.Single }
            }
        };
        await _hotelService.AddAsync(hotel5);
    }

    public async Task ResetAllDataAsync()
    {
        await DeleteAllDataAsync();
        await SeedTestDataAsync();
    }
}
