namespace Application.Interfaces;

public interface IUser
{
    public int Id { get; init; }
    public string Name { get; }
    public string AvatarUrl{ get; init; }

}