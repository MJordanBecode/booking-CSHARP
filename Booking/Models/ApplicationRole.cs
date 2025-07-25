using Microsoft.AspNetCore.Identity;

public class ApplicationRole : IdentityRole
{
    public int Level { get; set; }
}