using BugFixer.Domain.Entities.SiteSetting;

namespace BugFixer.Application.Interfaces
{
    public interface ISiteSettingRepository
    {
        Task<EmailSetting> GetDefaultEmail();
    }
}
