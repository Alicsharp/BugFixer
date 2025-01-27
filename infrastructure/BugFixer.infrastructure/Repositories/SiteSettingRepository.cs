using BugFixer.Application.Interfaces;
using BugFixer.Domain.Entities.SiteSetting;
using Microsoft.EntityFrameworkCore;

namespace BugFixer.infrastructure.Repositories
{
    public class SiteSettingRepository : ISiteSettingRepository
    {
        private BugFixerDbContext _context;

        public SiteSettingRepository(BugFixerDbContext context)
        {
            _context = context;
        }

        public async Task<EmailSetting> GetDefaultEmail()
        {
            return await _context.EmailSettings.FirstOrDefaultAsync(s => !s.IsDeleted && s.IsDefault);
        }
    }
}
