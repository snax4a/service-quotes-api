using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.CustomerAddress
{
    public class CreateAddressRequest
    {
        [Required(ErrorMessage = "Address name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "ZIP Code is required")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }
    }
}
