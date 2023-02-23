using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;

        public IMapper _mapper { get; }

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await _regionRepository.GetAllAsync();

            /*// return DTO regions
            var regionsDTO = new List<Models.DTO.Region>();
            regions.ToList().ForEach(region =>
            {
                var regionDTO = new Models.DTO.Region()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    Area = region.Area,
                    Latitude = region.Latitude,
                    Longitude = region.Longitude,
                    Population = region.Population
                };

                regionsDTO.Add(regionDTO);
            });*/

            var regionsDTO = _mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id) 
        {
           var region =  await _regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();
            }
           var regionDTO =  _mapper.Map<Models.DTO.Region>(region);
           return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Request (DTO) to Domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Latitude = addRegionRequest.Latitude,
                Longitude = addRegionRequest.Longitude,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population
            };

            // Pass details to Repository
            region = await _regionRepository.AddSync(region);

            // Convert backk to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Latitude = region.Latitude,
                Longitude = region.Longitude,
                Name = region.Name,
                Population = region.Population
            };

            return CreatedAtAction(nameof(GetRegionAsync), new {id=regionDTO.Id}, regionDTO);

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> RemoveRegionAsync(Guid id)
        {
            //[1] Get and Delete a region from DB
            //[1.2] Return 
            var deletedRegion = await _regionRepository.DeleteAsyn(id);
            //[2] If null, NotFound
            if (deletedRegion == null)
            {
                return NotFound();
            }
            //[3] If a region found, Convert Response back to DTO
            var responsedRegionDTO = new Models.DTO.Region
            {
                Id = deletedRegion.Id,
                Code = deletedRegion.Code,
                Area = deletedRegion.Area,
                Latitude = deletedRegion.Latitude,
                Longitude = deletedRegion.Longitude,
                Name = deletedRegion.Name,
                Population = deletedRegion.Population
            };

            //[4] return Ok response
            return Ok(responsedRegionDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            // Convert DTO to Domain model
            // 1. Create a new Region Domain model
            var region = new Models.Domain.Region
            {

                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Latitude = updateRegionRequest.Latitude,
                Longitude = updateRegionRequest.Longitude,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population
            };

            // update Region using repository
            region = await _regionRepository.UpdateAsync(id, region);

            // If Null, then NotFound
            if(region == null)
            {
                return NotFound();
            }
            // Convert Domain back to DTO
            // Convert backk to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Latitude = region.Latitude,
                Longitude = region.Longitude,
                Name = region.Name,
                Population = region.Population
            };

            // Return Ok response
            return Ok(regionDTO);

        }
    }
}
