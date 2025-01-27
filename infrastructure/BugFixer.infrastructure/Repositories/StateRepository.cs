using BugFixer.Application.Interfaces;
using BugFixer.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;

namespace BugFixer.infrastructure.Repositories
{
    public class StateRepository : IStateRepository
    {
        private readonly BugFixerDbContext _context;

        public StateRepository(BugFixerDbContext context)
        {
            _context = context;
        }

        public async  Task<List<State>> GetAllState(long? stateId=null)
        {
            var states = _context.States.Where(s => !s.IsDeleted).AsQueryable();

            if (stateId.HasValue)
            {
                states = states.Where(s => s.ParentId.HasValue && s.ParentId.Value == stateId.Value);
            }
            else
            {
                states = states.Where(s => s.ParentId == null);
            }

            return await states.ToListAsync();
        }

        // دریافت لیست شهرها بر اساس کشور انتخاب شده
        public async Task<List<State>> GetCitiesByCountryId(long countryId)
        {
            return _context.States.Where(s => s.ParentId == countryId).ToList();
        }

    }
}
 
