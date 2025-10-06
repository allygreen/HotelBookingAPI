using HotelBooking.Core.Entities;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories.Implementation;

public class RoomRepository : IRoomRepository
{
    private readonly HotelBookingDbContext _context;

    public RoomRepository(HotelBookingDbContext context)
    {
        _context = context;
    }
    public async Task<Room?> GetByIdAsync(int id)
    {
        return await _context.Rooms.FindAsync(id);
    }
    public Task<int> GetRoomCapacityAsync(int roomId)
    {
        var roomCapacity =  _context.Rooms.Find(roomId).Capacity;
        return Task.FromResult(roomCapacity);
    }

    public async Task<List<Room>> GetAvailableRooms(DateTime checkIn, DateTime checkOut, int capacity)
    {
        return await _context.Rooms
            .Where(room => room.Capacity >= capacity)
            .Where(room => !room.Bookings.Any(booking => 
                booking.CheckIn < checkOut && 
                booking.CheckOut > checkIn)) 
            .Include(room => room.Hotel) 
            .ToListAsync();
    }

}