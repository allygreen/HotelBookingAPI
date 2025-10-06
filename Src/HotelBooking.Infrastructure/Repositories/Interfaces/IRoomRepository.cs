using HotelBooking.Core.Entities;

namespace HotelBooking.Infrastructure.Repositories.Interfaces;

public interface IRoomRepository
{
    public Task<Room?> GetByIdAsync(int id);
    public Task<int> GetRoomCapacityAsync(int roomId);
    
    public Task<List<Room>> GetAvailableRooms(DateTime checkIn, DateTime checkOut, int capacity);
    
}