using AutoMapper;

namespace HotelBooking.Core.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entity to request DTO
        CreateMap<Entities.Hotel, DTOs.Requests.CreateHotelRequest>().ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms));
        CreateMap<DTOs.Requests.CreateHotelRequest, Entities.Hotel>().ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms));
        
        // entity to response dto
        CreateMap<Entities.Booking, DTOs.Responses.BookingDetails>();
        CreateMap<DTOs.Responses.BookingResponse, Entities.Booking>();

        
        // entity to request dto
        CreateMap<Entities.Booking, DTOs.Requests.CreateBookingRequest>();
        CreateMap<DTOs.Requests.CreateBookingRequest, Entities.Booking>();
        
        
        
        
        CreateMap<Entities.Room, DTOs.Requests.Room>();
        CreateMap<DTOs.Requests.Room, Entities.Room>();
    }
}