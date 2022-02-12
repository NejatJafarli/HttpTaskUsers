public class PostedUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public PostedUser()
    {
        Name = "";
        Surname = "";

    }
    public PostedUser(User user)
    {
        Name = user.Name;
        Surname = user.Surname;
    }

    public override string ToString()
    {
        return $"Name->{Name} --  Surname->{Surname} ";
    }
}
