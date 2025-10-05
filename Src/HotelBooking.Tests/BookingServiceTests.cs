using AutoMapper;
using HotelBooking.Application.Services.Implementation;
using HotelBooking.Application.Services.Interfaces;
using HotelBooking.Core.DTOs.Requests;
using HotelBooking.Core.Entities;
using HotelBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelBooking.Tests;

[TestFixture]
public class BookingServiceTests
{
    private Mock<IBookingRepository> _mockBookingRepository;
    private Mock<IRoomRepository> _mockRoomRepository;
    private Mock<ILogger<BookingService>> _mockLogger;
    private Mock<IMapper> _mockMapper;
    private BookingService _bookingService;

    [SetUp]
    public void Setup()
    {
        _mockBookingRepository = new Mock<IBookingRepository>();
        _mockRoomRepository = new Mock<IRoomRepository>();
        _mockLogger = new Mock<ILogger<BookingService>>();
        _mockMapper = new Mock<IMapper>();
        
        _bookingService = new BookingService(
            _mockBookingRepository.Object,
            _mockRoomRepository.Object,
            _mockLogger.Object,
            _mockMapper.Object);
    }

    [Test]
    public async Task BookRoom_WhenRoomExistsAndAvailable_ReturnsTrue()
    {
        // Arrange
        var request = new CreateBookingRequest
        {
            RoomId = 1,
            CustomerName = "John Doe",
            CheckIn = DateTime.Now.AddDays(1),
            CheckOut = DateTime.Now.AddDays(3),
            NumberOfGuests = 2
        };

        var room = new Core.Entities.Room { Id = 1, Capacity = 4 };
        var bookingEntity = new Booking
        {
            Id = 1
        };

        _mockRoomRepository.Setup(x => x.GetByIdAsync(request.RoomId))
            .ReturnsAsync(room);
        _mockBookingRepository.Setup(x => x.IsRoomAvailableAsync(
            request.RoomId, request.CheckIn, request.CheckOut))
            .ReturnsAsync(true);
        _mockRoomRepository.Setup(x => x.GetRoomCapacityAsync(request.RoomId))
            .ReturnsAsync(4);
        _mockMapper.Setup(x => x.Map<Booking>(request))
            .Returns(bookingEntity);
        _mockBookingRepository.Setup(x => x.AddAsync(bookingEntity))
            .ReturnsAsync(bookingEntity);

        // Act
        var result = await _bookingService.BookRoom(request);

        // Assert
        Assert.That(result.Success, Is.True);
        _mockBookingRepository.Verify(x => x.AddAsync(bookingEntity), Times.Once);
    }

    [Test]
    public async Task BookRoom_WhenRoomDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var request = new CreateBookingRequest
        {
            RoomId = 999,
            CustomerName = "John Doe",
            CheckIn = DateTime.Now.AddDays(1),
            CheckOut = DateTime.Now.AddDays(3),
            NumberOfGuests = 2
        };

        _mockRoomRepository.Setup(x => x.GetByIdAsync(request.RoomId))
            .ReturnsAsync((Core.Entities.Room)null);

        // Act
        var result = await _bookingService.BookRoom(request);

        // Assert
        Assert.That(result.Success, Is.False);
        _mockBookingRepository.Verify(x => x.AddAsync(It.IsAny<Booking>()), Times.Never);
    }

    [Test]
    public async Task BookRoom_WhenRoomNotAvailable_ReturnsFalse()
    {
        // Arrange
        var request = new CreateBookingRequest
        {
            RoomId = 1,
            CustomerName = "John Doe",
            CheckIn = DateTime.Now.AddDays(1),
            CheckOut = DateTime.Now.AddDays(3),
            NumberOfGuests = 2
        };

        var room = new Core.Entities.Room { Id = 1, Capacity = 4 };

        _mockRoomRepository.Setup(x => x.GetByIdAsync(request.RoomId))
            .ReturnsAsync(room);
        _mockBookingRepository.Setup(x => x.IsRoomAvailableAsync(
            request.RoomId, request.CheckIn, request.CheckOut))
            .ReturnsAsync(false);

        // Act
        var result = await _bookingService.BookRoom(request);

        // Assert
        Assert.That(result.Success, Is.False);
        _mockBookingRepository.Verify(x => x.AddAsync(It.IsAny<Booking>()), Times.Never);
    }

    [Test]
    public async Task BookRoom_WhenTooManyGuests_ReturnsFalse()
    {
        // Arrange
        var request = new CreateBookingRequest
        {
            RoomId = 1,
            CustomerName = "John Doe",
            CheckIn = DateTime.Now.AddDays(1),
            CheckOut = DateTime.Now.AddDays(3),
            NumberOfGuests = 6 // More than room capacity
        };

        var room = new Core.Entities.Room { Id = 1, Capacity = 4 };

        _mockRoomRepository.Setup(x => x.GetByIdAsync(request.RoomId))
            .ReturnsAsync(room);
        _mockBookingRepository.Setup(x => x.IsRoomAvailableAsync(
            request.RoomId, request.CheckIn, request.CheckOut))
            .ReturnsAsync(true);
        _mockRoomRepository.Setup(x => x.GetRoomCapacityAsync(request.RoomId))
            .ReturnsAsync(4);

        // Act
        var result = await _bookingService.BookRoom(request);

        // Assert
        Assert.That(result.Success, Is.False);
        _mockBookingRepository.Verify(x => x.AddAsync(It.IsAny<Booking>()), Times.Never);
    }
}
