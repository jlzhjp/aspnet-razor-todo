using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AspNetRazorTodo.WebApp.Identity;

public class SimpleTodoUserStore(
    IMongoDatabase database,
    IdentityErrorDescriber errorDescriber) : IUserPasswordStore<SimpleTodoUser>, IUserEmailStore<SimpleTodoUser>, IUserPhoneNumberStore<SimpleTodoUser>
{
    private readonly IMongoCollection<SimpleTodoUser> _users = database.GetCollection<SimpleTodoUser>("users");

    public void Dispose() => GC.SuppressFinalize(this);

    public Task<string> GetUserIdAsync(SimpleTodoUser user, CancellationToken cancellationToken) => Task.FromResult(user.Id.ToString());

    public Task<string?> GetUserNameAsync(SimpleTodoUser user, CancellationToken cancellationToken) => Task.FromResult(user.UserName);

    public Task SetUserNameAsync(SimpleTodoUser user, string? userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedUserNameAsync(SimpleTodoUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task SetNormalizedUserNameAsync(SimpleTodoUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> CreateAsync(SimpleTodoUser user, CancellationToken cancellationToken)
    {
        await _users.InsertOneAsync(
            document: user,
            cancellationToken: cancellationToken);
        
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(SimpleTodoUser user, CancellationToken cancellationToken)
    {
        var result = await _users.ReplaceOneAsync(
            filter: x => x.Id == user.Id,
            replacement: user,
            cancellationToken: cancellationToken);

        return result switch
        {
            { IsAcknowledged: false } => IdentityResult.Failed(errorDescriber.DefaultError()),
            { ModifiedCount: < 1 } => IdentityResult.Failed(errorDescriber.DefaultError()),
            _ => IdentityResult.Success
        };
    }

    public async Task<IdentityResult> DeleteAsync(SimpleTodoUser user, CancellationToken cancellationToken)
    {
        var result = await _users.DeleteOneAsync(
            x => x.Id == user.Id,
            cancellationToken: cancellationToken);
        
        return result switch
        {
            { IsAcknowledged: false } => IdentityResult.Failed(errorDescriber.DefaultError()),
            { DeletedCount: < 1 } => IdentityResult.Failed(errorDescriber.DefaultError()),
            _ => IdentityResult.Success
        };
    }

    public async Task<SimpleTodoUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(userId, out var id)) return null;
        var cursor = await _users.FindAsync(
            filter: user => user.Id == id,
            cancellationToken: cancellationToken);
        return await cursor.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<SimpleTodoUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        var cursor = await _users.FindAsync(
            filter: Builders<SimpleTodoUser>.Filter.Eq(user => user.NormalizedUserName, normalizedUserName),
            cancellationToken: cancellationToken);
        return await cursor.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public Task SetPasswordHashAsync(SimpleTodoUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string?> GetPasswordHashAsync(SimpleTodoUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(SimpleTodoUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
    }

    public Task SetEmailAsync(SimpleTodoUser user, string? email, CancellationToken cancellationToken)
    {
        user.Email = email;
        return Task.CompletedTask;
    }

    public Task<string?> GetEmailAsync(SimpleTodoUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(SimpleTodoUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task SetEmailConfirmedAsync(SimpleTodoUser user, bool confirmed, CancellationToken cancellationToken)
    {
        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    public async Task<SimpleTodoUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        var cursor = await _users.FindAsync(
            filter: user => user.NormalizedEmail == normalizedEmail,
            cancellationToken: cancellationToken);
        return await cursor.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public Task<string?> GetNormalizedEmailAsync(SimpleTodoUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task SetNormalizedEmailAsync(SimpleTodoUser user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }

    public Task SetPhoneNumberAsync(SimpleTodoUser user, string? phoneNumber, CancellationToken cancellationToken)
    {
        user.PhoneNumber = phoneNumber;
        return Task.CompletedTask;
    }

    public Task<string?> GetPhoneNumberAsync(SimpleTodoUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PhoneNumber);
    }

    public Task<bool> GetPhoneNumberConfirmedAsync(SimpleTodoUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PhoneNumberConfirmed);
    }

    public Task SetPhoneNumberConfirmedAsync(SimpleTodoUser user, bool confirmed, CancellationToken cancellationToken)
    {
        user.PhoneNumberConfirmed = confirmed;
        return Task.CompletedTask;
    }
}