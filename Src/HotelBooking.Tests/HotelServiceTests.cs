using AutoMapper;
using HotelBooking.Application.Services.Implementation;
using HotelBooking.Application.Services.Interfaces;
using HotelBooking.Core.DTOs.Requests;
using HotelBooking.Core.DTOs.Responses;
using HotelBooking.Core.Entities;
using HotelBooking.Core.Enums;
using HotelBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelBooking.Tests;

[TestFixture]
public class HotelServiceTests
{
    private Mock<IHotelRepository> _mockHotelRepository;
    private Mock<IMapper> _mockMapper;
    private HotelService _hotelService;
    private Mock<ILogger<HotelService>> _mockLogger;


    [SetUp]
    public void Setup()
    {
        _mockHotelRepository = new Mock<IHotelRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<HotelService>>();
        
        _hotelService = new HotelService(
            _mockHotelRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object);
    }

    [Test]
    public async Task AddAsync_WhenValidHotel_ShouldCallRepository()
    {
        // Arrange
        var hotelDto = new CreateHotelRequest()
        {
            Name = "The Prancing Pony",
            City = "Bree",
            Rooms = new List<Core.DTOs.Requests.Room>
            {
                new Core.DTOs.Requests.Room { Capacity = 2, RoomType = RoomType.Double },
                new Core.DTOs.Requests.Room { Capacity = 1, RoomType = RoomType.Single },
                new Core.DTOs.Requests.Room { Capacity = 2, RoomType = RoomType.Deluxe },
                new Core.DTOs.Requests.Room { Capacity = 2, RoomType = RoomType.Double },
                new Core.DTOs.Requests.Room { Capacity = 1, RoomType = RoomType.Single },
                new Core.DTOs.Requests.Room { Capacity = 2, RoomType = RoomType.Deluxe }
            }
        };

        var hotelEntity = new Core.Entities.Hotel
        {
            Id = 1,
            Name = "The Prancing Pony",
            City = "Bree"
        };

        _mockMapper.Setup(x => x.Map<Core.Entities.Hotel>(hotelDto))
            .Returns(hotelEntity);
        _mockHotelRepository.Setup(x => x.AddAsync(hotelEntity))
            .ReturnsAsync(hotelEntity);

        // Act
        var result = await _hotelService.AddAsync(hotelDto);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Message, Is.EqualTo("Hotel created successfully"));
        _mockMapper.Verify(x => x.Map<Core.Entities.Hotel>(hotelDto), Times.Once);
        _mockHotelRepository.Verify(x => x.AddAsync(hotelEntity), Times.Once);
    }

    [Test]
    public async Task AddAsync_WhenHotelHasWrongRoomCount_ReturnsFailure()
    {
        // Arrange
        var hotelDto = new CreateHotelRequest()
        {
            Name = "The Prancing Pony",
            City = "Bree",
            Rooms = new List<Core.DTOs.Requests.Room>
            {
                new Core.DTOs.Requests.Room { Capacity = 2, RoomType = RoomType.Double },
                new Core.DTOs.Requests.Room { Capacity = 1, RoomType = RoomType.Single },
                new Core.DTOs.Requests.Room { Capacity = 2, RoomType = RoomType.Deluxe } 
            }
        };

        // Act
        var result = await _hotelService.AddAsync(hotelDto);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Is.EqualTo("Hotel must have exactly 6 rooms, but 3 rooms provided"));
        _mockMapper.Verify(x => x.Map<Core.Entities.Hotel>(It.IsAny<CreateHotelRequest>()), Times.Never);
        _mockHotelRepository.Verify(x => x.AddAsync(It.IsAny<Core.Entities.Hotel>()), Times.Never);
    }

    [Test]
    public async Task SearchHotels_WhenHotelsFound_ReturnsMappedHotels()
    {
        // Arrange
        var searchTerm = "prancing";
        var hotelEntities = new List<Core.Entities.Hotel>
        {
            new Core.Entities.Hotel { Id = 1, Name = "The Prancing Pony", City = "Bree" },
            new Core.Entities.Hotel { Id = 2, Name = "Prancing Donkey", City = "Dunfermline" }
        };

        var expectedHotels = new List<SearchHotelResponse>
        {
            new SearchHotelResponse { Name = "The Prancing Pony", City = "Bree" },
            new SearchHotelResponse { Name = "Prancing Donkey", City = "Dunfermline" }
        };

        _mockHotelRepository.Setup(x => x.SearchHotelNameAsync(searchTerm))
            .ReturnsAsync(hotelEntities);
        _mockMapper.Setup(x => x.Map<List<SearchHotelResponse>>(hotelEntities))
            .Returns(expectedHotels);

        // Act
        var result = await _hotelService.SearchHotels(searchTerm);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("The Prancing Pony"));
        Assert.That(result[1].Name, Is.EqualTo("Prancing Donkey"));
        
        _mockHotelRepository.Verify(x => x.SearchHotelNameAsync(searchTerm), Times.Once);
        _mockMapper.Verify(x => x.Map<List<SearchHotelResponse>>(hotelEntities), Times.Once);
    }

    [Test]
    public async Task SearchHotels_WhenNoHotelsFound_ReturnsEmptyList()
    {
        // Arrange
        var searchTerm = "NonExistentHotel";
        var emptyHotelEntities = new List<Core.Entities.Hotel>();
        var emptyHotels = new List<SearchHotelResponse>();

        _mockHotelRepository.Setup(x => x.SearchHotelNameAsync(searchTerm))
            .ReturnsAsync(emptyHotelEntities);
        _mockMapper.Setup(x => x.Map<List<SearchHotelResponse>>(emptyHotelEntities))
            .Returns(emptyHotels);

        // Act
        var result = await _hotelService.SearchHotels(searchTerm);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(0));
        
        _mockHotelRepository.Verify(x => x.SearchHotelNameAsync(searchTerm), Times.Once);
        _mockMapper.Verify(x => x.Map<List<SearchHotelResponse>>(emptyHotelEntities), Times.Once);
    }

    [Test]
    public async Task SearchHotels_WhenSearchTermIsEmpty_ReturnsAllHotels()
    {
        // Arrange
        var searchTerm = "";
        var allHotelEntities = new List<Core.Entities.Hotel>
        {
            new Core.Entities.Hotel { Id = 1, Name = "Hotel A", City = "City A" },
            new Core.Entities.Hotel { Id = 2, Name = "Hotel B", City = "City B" },
            new Core.Entities.Hotel { Id = 3, Name = "Hotel C", City = "City C" }
        };

        var expectedHotels = new List<SearchHotelResponse>
        {
            new SearchHotelResponse { Name = "Hotel A", City = "City A" },
            new SearchHotelResponse { Name = "Hotel B", City = "City B" },
            new SearchHotelResponse { Name = "Hotel C", City = "City C" }
        };

        _mockHotelRepository.Setup(x => x.SearchHotelNameAsync(searchTerm))
            .ReturnsAsync(allHotelEntities);
        _mockMapper.Setup(x => x.Map<List<SearchHotelResponse>>(allHotelEntities))
            .Returns(expectedHotels);

        // Act
        var result = await _hotelService.SearchHotels(searchTerm);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
        
        _mockHotelRepository.Verify(x => x.SearchHotelNameAsync(searchTerm), Times.Once);
        _mockMapper.Verify(x => x.Map<List<SearchHotelResponse>>(allHotelEntities), Times.Once);
    }

    [Test]
    public async Task AddAsync_WhenRoomHasInvalidCapacity_ReturnsFailure()
    {
        // Arrange
        var hotelDto = new CreateHotelRequest()
        {
            Name = "The Prancing Pony",
            City = "Bree",
            Rooms = new List<Core.DTOs.Requests.Room>
            {
                new Core.DTOs.Requests.Room { Capacity = 2, RoomType = RoomType.Double },
                new Core.DTOs.Requests.Room { Capacity = 1, RoomType = RoomType.Single },
                new Core.DTOs.Requests.Room { Capacity = 0, RoomType = RoomType.Deluxe }, // Invalid capacity
                new Core.DTOs.Requests.Room { Capacity = 2, RoomType = RoomType.Double },
                new Core.DTOs.Requests.Room { Capacity = 1, RoomType = RoomType.Single },
                new Core.DTOs.Requests.Room { Capacity = 2, RoomType = RoomType.Deluxe }
            }
        };

        // Act
        var result = await _hotelService.AddAsync(hotelDto);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Is.EqualTo("All rooms must have a valid capacity greater than 0"));
        _mockMapper.Verify(x => x.Map<Core.Entities.Hotel>(It.IsAny<CreateHotelRequest>()), Times.Never);
        _mockHotelRepository.Verify(x => x.AddAsync(It.IsAny<Core.Entities.Hotel>()), Times.Never);
    }
}
