namespace ProductModels.Dtos;

public class UserForLoginDto
{

    public string Email { get; set; }
    public string Password { get; set; }

    public UserForLoginDto()
    {
        if (Email == null)
        {
            Email = "";
        }
        
        if (Password == null)
        {
            Password = "";
        }
    }
}