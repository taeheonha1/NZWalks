using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;
using System.Collections.Generic;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //[1] Fetch data from database - Domain Walks
            var walksDomain = await walkRepository.GetAllAsync();
            //[2] Convert Domain Walks to DTO Walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            //[3] Return Response
            return Ok(walksDTO);
            //return wait walkRepository.GetAllAsync();
            //Error: It is expecting an ActionResult. But it just returning a list.
            //In the previous control, we do mapping from the domain model to the DTO
        }

        //public async Task<Walk> GetSync(Guid id)
        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //[1] Fetch data from database - Domain Walks
            var walkDomain = await walkRepository.GetAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }
            //Convert Domain Walk to DTO Walk
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            //Return Response
            return Ok(walkDTO);
        }



        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //[1] Convert DTO to Domain object 
            var walkDomain = new Models.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            // [2] Pass Domain obect to Repository
            walkDomain = await walkRepository.AddAsync(walkDomain);

            // [3] Convert the Domain Object back to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            // [4] Send DTO response back Client with route values
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }
        //     public async Task<Walk> UpdateSync(Guid id, Walk walk)
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequestDTO)
        {
            // [1]Convert DTO to Domain object
            // 1.1 Create a new Region Domain model
            var walkDomain = new Models.Domain.Walk
            {
                Name = updateWalkRequestDTO.Name,
                Length = updateWalkRequestDTO.Length,
                RegionId = updateWalkRequestDTO.RegionId,
                WalkDifficultyId = updateWalkRequestDTO.WalkDifficultyId
            };

            // [2] Pass details to Repository - Get Domain Object in response (or null)
            // update Walk using repository
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            // [3] If Null, then NotFound
            if (walkDomain == null)
            {
                return NotFound();
            }

            // Create a new response!
            // [4]Convert Domain back to DTO 
            // Convert backk to DTO
            var newlyGenerated_WalkDTO = new Models.DTO.Walk
            {
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            // [5]Return Ok response
            return Ok(newlyGenerated_WalkDTO);

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> RemoveWalkAsync(Guid id)
        {
            //[1] Call Repository to delete Walk from DB
            //[1.2] Return 
            var deletedWalkDomain = await walkRepository.DeleteAsync(id);
            //[2] If null, NotFound
            if (deletedWalkDomain == null)
            {
                return NotFound();
            }
            //[3] If a region found, Convert Response back to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(deletedWalkDomain);
            //[4] return Ok response
            return Ok(walkDTO);
        }
    }
}
