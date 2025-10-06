using AutoMapper;

namespace HotelBooking.Core.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Hotel mappings
        CreateMap<Entities.Hotel, DTOs.Requests.CreateHotelRequest>()
            .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms));
        CreateMap<DTOs.Requests.CreateHotelRequest, Entities.Hotel>()
            .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms));

        // Hotel to SearchHotelResponse mapping
        CreateMap<Entities.Hotel, DTOs.Responses.SearchHotelResponse>()
            .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms));
        
        // Room to RoomResponse mapping
        CreateMap<Entities.Room, DTOs.Responses.RoomResponse>();
        
        // Room mappings
        CreateMap<Entities.Room, DTOs.Requests.Room>();
        CreateMap<DTOs.Requests.Room, Entities.Room>();

        // Booking mappings
        CreateMap<Entities.Booking, DTOs.Requests.CreateBookingRequest>();
        CreateMap<DTOs.Requests.CreateBookingRequest, Entities.Booking>()
            .ForMember(dest => dest.BookingReference, opt => opt.Ignore()) // Will be set by service
            .ForMember(dest => dest.Room, opt => opt.Ignore()); // Will be set by service

        // Booking response mappings
        CreateMap<Entities.Booking, DTOs.Responses.BookingDetails>()
            .ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Room.Hotel.Name));
    }
}