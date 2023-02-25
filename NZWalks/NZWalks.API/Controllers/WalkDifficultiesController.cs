using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WalkDifficultiesController : ControllerBase
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
            //[1] Fetch data from database - Domain WalkDifficulty
            var walkDifficultiesDomain = await walkDifficultyRepository.GetAllAsync();
            //[2] Convert Domain WalkDifficulties to DTO WalkDifficulties
            var walkdifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultiesDomain);

            //[3] Return Response
            return Ok(walkdifficultiesDTO);
        }

        //public async Task<Walk> GetSync(Guid id)
        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            //[1] Fetch data from database - Domain Walkdifficulty
            var walkDifficultyDomain = await walkDifficultyRepository.GetSync(id);
            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }
            //Convert Domain WalkDifficulty to DTO WalkDifficulty
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            //Return Response
            return Ok(walkDifficultyDTO);
        }

        // public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] Models.DTO.AddWalkDifficutyRequest addWalkDifficutyRequest)
        {
            //[1] Convert DTO to Domain object 
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficutyRequest.Code
            };

            // [2] Pass Domain obect to Repository
            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            // [3] Convert the Domain Object back to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Id = walkDifficultyDomain.Id,
                Code = walkDifficultyDomain.Code
            };

            // [4] Send DTO response back Client with route values
            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }

        //public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequestDTO)
        {
            // [1]Convert DTO to Domain object
            // 1.1 Create a new Region Domain model
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequestDTO.Code
            };

            // [2] Pass details to Repository - Get Domain Object in response (or null)
            // update Walk using repositorywalkDifficultyDomain
            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);

            // [3] If Null, then NotFound
            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }

            // Create a new response!
            // [4]Convert Domain back to DTO 
            // Convert backk to DTO
            var newlyGenerated_WalkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Code = walkDifficultyDomain.Code
            };

            // [5]Return Ok response
            return Ok(newlyGenerated_WalkDifficultyDTO);

        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> RemoveWalkDifficultyAsync(Guid id)
        {
            //[1] Call Repository to delete Walk from DB
            //[1.2] Return 
            var deletedWalkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);
            //[2] If null, NotFound
            if (deletedWalkDifficultyDomain == null)
            {
                return NotFound();
            }
            //[3] If a region found, Convert Response back to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(deletedWalkDifficultyDomain);
            //[4] return Ok response
            return Ok(walkDifficultyDTO);
        }


    }
}
