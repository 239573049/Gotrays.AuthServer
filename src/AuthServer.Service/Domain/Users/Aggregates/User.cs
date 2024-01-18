namespace AuthServer.Service.Domain.Users.Aggregates;

public class User : FullAggregateRoot<Guid, Guid>, IMultiTenant
{
    protected User()
    {
    }

    public string UserName { get; private set; }
    public string Email { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public string PasswordHash { get; private set; }
    public string SecurityStamp { get; private set; }
    public string ConcurrencyStamp { get; private set; }
    public string PhoneNumber { get; private set; }
    public bool PhoneNumberConfirmed { get; private set; }
    public bool TwoFactorEnabled { get; private set; }
    public DateTimeOffset? LockoutEnd { get; private set; }
    public bool LockoutEnabled { get; private set; }
    public int AccessFailedCount { get; private set; }
    public Guid TenantId { get; set; }

    public User(string userName, string email,
        bool emailConfirmed, string passwordHash, string phoneNumber)
    {
        UserName = userName;
        Email = email;
        EmailConfirmed = emailConfirmed;
        PasswordHash = passwordHash;
        PhoneNumber = phoneNumber;
    }

    public void SetEmailConfirmed(bool confirmed)
    {
        EmailConfirmed = confirmed;
    }

    public void SetPhoneNumberConfirmed(bool confirmed)
    {
        PhoneNumberConfirmed = confirmed;
    }

    public void SetTwoFactorEnabled(bool enabled)
    {
        TwoFactorEnabled = enabled;
    }

    public void SetLockoutEnd(DateTimeOffset? lockoutEnd)
    {
        LockoutEnd = lockoutEnd;
    }

    public void SetLockoutEnabled(bool enabled)
    {
        LockoutEnabled = enabled;
    }

    public void IncrementAccessFailedCount()
    {
        AccessFailedCount++;
    }
}