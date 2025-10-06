using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.Swift;

namespace HotelBooking.Core.DTOs.Requests;

public class CreateBookingRequest : IValidatableObject
{
    [Range(1, int.MaxValue, ErrorMessage = "RoomId must be greater than 0")]
    public int RoomId { get; set; }
    [Required(ErrorMessage = "CustomerName is required")]
    public string CustomerName { get; set; }
    [Range(1, 10, ErrorMessage = "Number of guests must be greater than 0")]
    public int NumberOfGuests { get; set; }   
    [Required(ErrorMessage = "CheckIn is required")]
    public DateTime CheckIn { get; set; }
    [Required(ErrorMessage = "CheckOut is required")]
    public DateTime CheckOut { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CheckIn >= CheckOut)
        {
            yield return new ValidationResult(
                "Check-in date must be before check-out date",
                new[] { nameof(CheckIn), nameof(CheckOut) });
        }
        
        if (CheckIn < DateTime.Today)
        {
            yield return new ValidationResult(
                "Check-in date must be in the future",
                new[] { nameof(CheckIn) });
        }
        
        if (CheckOut < DateTime.Today)
        {
            yield return new ValidationResult(
                "Check-out date must be in the future",
                new[] { nameof(CheckOut) });
        }
    }

}