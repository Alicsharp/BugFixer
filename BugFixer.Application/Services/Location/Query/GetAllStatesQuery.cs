using BugFixer.Application.Contract.DTOS.Location;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Services.Result;
using BugFixer.Domain.Entities.Location;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Location.Query
{
    public record GetAllStatesQuery(long? stateId = null) : IRequest< List<SelectListDto>>;
    public class GetAllStatesQueryHandler : IRequestHandler<GetAllStatesQuery,List<SelectListDto>>
    {
        private readonly IStateRepository _locationRepository;

        public GetAllStatesQueryHandler(IStateRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<List<SelectListDto>> Handle(GetAllStatesQuery request, CancellationToken cancellationToken)
        {
            var result = await _locationRepository.GetAllState(request.stateId);
            if (result == null) return new List<SelectListDto>();

            return    result.Select(s => new SelectListDto
            {
                Id = s.Id,
                Title = s.Title,
            }).ToList();
        }

        
    }



}


