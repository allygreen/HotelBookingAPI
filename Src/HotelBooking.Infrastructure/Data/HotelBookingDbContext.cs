using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Data;

public class HotelBookingDbContext : DbContext
{
    public HotelBookingDbContext(DbContextOptions<HotelBookingDbContext> options) : base(options)
    {
    }
    
    public DbSet<Core.Entities.Booking> Bookings { get; set; }
    public DbSet<Core.Entities.Hotel> Hotels { get; set; }
    public DbSet<Core.Entities.Room> Rooms { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Core.Entities.Booking>();
        modelBuilder.Entity<Core.Entities.Hotel>();
        modelBuilder.Entity<Core.Entities.Room>();

    }

}