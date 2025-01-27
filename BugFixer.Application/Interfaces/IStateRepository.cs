using BugFixer.Domain.Entities.Location;

namespace BugFixer.Application.Interfaces
{
    public interface IStateRepository
    {
        Task<List<State>> GetAllState(long? stateId = null);
        //Task<List<State>> GetCitiesByCountryId(long countryId);
    }


}
