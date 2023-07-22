using System.ComponentModel.DataAnnotations;

namespace API.Helper.DTOS;


public class RegisterDto
{
    [Required]
    public string FirstName { get; set; }

    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,20}$", ErrorMessage = "Password must be complex")]
    public string Password { get; set; }

    public string PhoneNumber { get; set; }
}