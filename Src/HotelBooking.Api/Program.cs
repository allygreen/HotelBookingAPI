using HotelBooking.Application.Services.Implementation;
using HotelBooking.Application.Services.Interfaces;
using HotelBooking.Core.Mappings;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Repositories.Implementation;
using HotelBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<ISeedRepository, SeedRepository>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ISeedService, SeedService>();

builder.Services.AddDbContext<HotelBookingDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();           // serves swagger.json
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelBooking API V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HotelBookingDbContext>();
    context.Database.EnsureCreated();
    context.Database.Migrate();
}

app.MapGet("/health", () => new { status = "OK", timestamp = DateTime.UtcNow });


app.Run();
