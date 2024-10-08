using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Event;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models;

namespace NXTBackend.API.Core.Services.Implementation;

/// <summary>
/// Temporary service to search for users, projects, cursi and learning goals.
/// Later on this service SHOULD be converted to use a search engine like ElasticSearch.
/// </summary>
public sealed class UserService : IUserService
{
    private readonly DatabaseContext _databaseContext;

    public UserService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<User?> FindByIdAsync(Guid id)
    {
        return await _databaseContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <inheritdoc/>
    public async Task<User?> FindByLoginAsync(string login)
    {
        return await _databaseContext.Users.FirstOrDefaultAsync(u => u.Login == login);
    }

    public async Task<User?> FindByNameAsync(string displayName)
    {
        return await _databaseContext.Users.FirstOrDefaultAsync(u => u.DisplayName == displayName);
    }

    public async Task<User> CreateAsync(User entity)
    {
        var user = await _databaseContext.Users.AddAsync(entity);
        _databaseContext.SaveChanges();
        return user.Entity;
    }

    public User Delete(User entity)
    {
        var user = _databaseContext.Users.Remove(entity);
        _databaseContext.SaveChanges();
        return user.Entity;
    }

    public User Update(User entity)
    {
        var user = _databaseContext.Users.Update(entity);
        _databaseContext.SaveChanges();
        return user.Entity;
    }

    public async Task<PaginatedList<User>> GetAllAsync(PaginationParams pagination)
    {
        var query = _databaseContext.Users.Select(User => User);
        return await PaginatedList<User>.CreateAsync(query, pagination.Page, pagination.Size);
    }

    public Task<User> SubscribeToCursus(User entity)
    {
        throw new NotImplementedException();
    }

    public Task<Review?> didRecentReviewOnUserProject(User entity, UserProject userProject)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task<User> DeleteAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task<User> SubscribeToCursus(User entity, Cursus cursus)
    {
        throw new NotImplementedException();
    }

    public Task<User> UnsubscribeFromCursus(User entity, Cursus cursus)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedList<Event>> GetEvents(User entity, PaginationParams pagination)
    {
        var dismissedEvents = _databaseContext.EventActions.Where(ae => ae.IsDismissed);
        var events = _databaseContext.Events.Where(e => !dismissedEvents.Any(ae => ae.EventId == e.Id));

        return PaginatedList<Event>.CreateAsync(events, pagination.Page, pagination.Size);
    }
}
