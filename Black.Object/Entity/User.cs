using Black.Domain.Base;
using Black.Domain.Enumeration;
using Black.Domain.ValueObjects;

namespace Black.Domain.Entity;

public class User : EntityBase
{
    public User()
    {

    }

    public User
    (
        Name name,
        Email email,
        Auth auth
    )
    {
        Name = name;
        Email = email;
        Auth = auth;
        Activate();
    }

    public User(long id) => Id = id;

    public Guid GuidId { get; set; } 

    public Name Name { get; set; }

    public Email Email { get; set; }

    public Status Status { get; set; }

    public Auth Auth { get; set; }

    public void Activate()
    {
        Status = Status.Active;
    }

    public void Passive()
    {
        Status = Status.Passive;
    }

    public void Update(string firstName, string lastName, string email)
    {
        Name = new Name(firstName, lastName);
        Email = new Email(email);
    }
}