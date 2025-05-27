using Microsoft.EntityFrameworkCore;
using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;

namespace GloboTicket.TicketManagement.Persistence.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(GloboTicketDbContext dbContext) : base(dbContext) { }

    public async Task<List<Category>> GetCategoriesWithEvents(bool includePassedEvents)
    {
        var allCategories = await _dbContext.Categories
            .Include(c => c.Events)
            .ToListAsync();

        if (!includePassedEvents)
        {
            allCategories.ForEach(p => p.Events.ToList().RemoveAll(c => c.Date < DateTime.Today));
        }
        
        return allCategories;
    }
}