

namespace Solution.Models;

public class User
{
    public int Id  { get; set; }
    
    public int RoleId  { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }

    public string Password { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string PhoneNumber { get; set; } = string.Empty;
    
    public DateTime DateCreation { get; set; }
}