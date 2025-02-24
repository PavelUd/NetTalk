namespace Application.Interfaces;

public interface IUser
{
    public Guid  Id { get; init; }
    public string Name { get; }
    public string AvatarUrl{ get; init; }

}