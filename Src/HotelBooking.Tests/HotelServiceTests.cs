using AutoMapper;
using HotelBooking.Application.Services.Implementation;
using HotelBooking.Application.Services.Interfaces;
using HotelBooking.Core;
using HotelBooking.Core.DTOs;
using HotelBooking.Core.DTOs.Requests;
using HotelBooking.Core.Entities;
using HotelBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Moq;
using Room = HotelBooking.Core.DTOs.Requests.Room;

namespace HotelBooking.Tests;

[TestFixture]
public class HotelServiceTests
{
    private Mock<IHotelRepository> _mockHotelRepository;
    private Mock<IMapper> _mockMapper;
    private HotelService _hotelService;

    [SetUp]
    public void Setup()
    {
        _mockHotelRepository = new Mock<IHotelRepository>();
        _mockMapper = new Mock<IMapper>();
        
        _hotelService = new HotelService(
            _mockHotelRepository.Object,
            _mockMapper.Object);
    }

    [Test]
    public async Task AddAsync_WhenValidHotel_ShouldCallRepository()
    {
        // Arrange
        var hotelDto = new CreateHotelRequest()
        {
            Name = "Grand Plaza Hotel",
            City = "New York",
            Rooms = new List<Room>()
        };

        var hotelEntity = new Core.Entities.Hotel
        {
            Id = 1,
            Name = "Grand Plaza Hotel",
            City = "New York"
        };

        _mockMapper.Setup(x => x.Map<Core.Entities.Hotel>(hotelDto))
            .Returns(hotelEntity);
        _mockHotelRepository.Setup(x => x.AddAsync(hotelEntity))
            .Returns(Task.CompletedTask);

        // Act
        await _hotelService.AddAsync(hotelDto);

        // Assert
        _mockMapper.Verify(x => x.Map<Core.Entities.Hotel>(hotelDto), Times.Once);
        _mockHotelRepository.Verify(x => x.AddAsync(hotelEntity), Times.Once);
    }

    [Test]
    public async Task SearchHotels_WhenHotelsFound_ReturnsMappedHotels()
    {
        // Arrange
        var searchTerm = "Grand";
        var hotelEntities = new List<Core.Entities.Hotel>
        {
            new Core.Entities.Hotel { Id = 1, Name = "Grand Plaza Hotel", City = "New York" },
            new Core.Entities.Hotel { Id = 2, Name = "Grand Resort", City = "Miami" }
        };

        var expectedHotels = new List<CreateHotelRequest>
        {
            new CreateHotelRequest { Name = "Grand Plaza Hotel", City = "New York" },
            new CreateHotelRequest { Name = "Grand Resort", City = "Miami" }
        };

        _mockHotelRepository.Setup(x => x.SearchHotelNameAsync(searchTerm))
            .ReturnsAsync(hotelEntities);
        _mockMapper.Setup(x => x.Map<List<CreateHotelRequest>>(hotelEntities))
            .Returns(expectedHotels);

        // Act
        var result = await _hotelService.SearchHotels(searchTerm);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("Grand Plaza Hotel"));
        Assert.That(result[1].Name, Is.EqualTo("Grand Resort"));
        
        _mockHotelRepository.Verify(x => x.SearchHotelNameAsync(searchTerm), Times.Once);
        _mockMapper.Verify(x => x.Map<List<CreateHotelRequest>>(hotelEntities), Times.Once);
    }

    [Test]
    public async Task SearchHotels_WhenNoHotelsFound_ReturnsEmptyList()
    {
        // Arrange
        var searchTerm = "NonExistentHotel";
        var emptyHotelEntities = new List<Core.Entities.Hotel>();
        var emptyHotels = new List<CreateHotelRequest>();

        _mockHotelRepository.Setup(x => x.SearchHotelNameAsync(searchTerm))
            .ReturnsAsync(emptyHotelEntities);
        _mockMapper.Setup(x => x.Map<List<CreateHotelRequest>>(emptyHotelEntities))
            .Returns(emptyHotels);

        // Act
        var result = await _hotelService.SearchHotels(searchTerm);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(0));
        
        _mockHotelRepository.Verify(x => x.SearchHotelNameAsync(searchTerm), Times.Once);
        _mockMapper.Verify(x => x.Map<List<CreateHotelRequest>>(emptyHotelEntities), Times.Once);
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

        var expectedHotels = new List<CreateHotelRequest>
        {
            new CreateHotelRequest { Name = "Hotel A", City = "City A" },
            new CreateHotelRequest { Name = "Hotel B", City = "City B" },
            new CreateHotelRequest { Name = "Hotel C", City = "City C" }
        };

        _mockHotelRepository.Setup(x => x.SearchHotelNameAsync(searchTerm))
            .ReturnsAsync(allHotelEntities);
        _mockMapper.Setup(x => x.Map<List<CreateHotelRequest>>(allHotelEntities))
            .Returns(expectedHotels);

        // Act
        var result = await _hotelService.SearchHotels(searchTerm);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
        
        _mockHotelRepository.Verify(x => x.SearchHotelNameAsync(searchTerm), Times.Once);
        _mockMapper.Verify(x => x.Map<List<CreateHotelRequest>>(allHotelEntities), Times.Once);
    }
}
