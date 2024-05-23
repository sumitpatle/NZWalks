using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    //api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        public readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        //Create walk
        //POST: /api/walks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTO addWalkRequestDTO)
        {
            //map DOT to domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDTO);
            await walkRepository.CreateAsyn(walkDomainModel);

            //map domain model to dto
            var walkdto = mapper.Map<WalkDTO>(walkDomainModel);
            return Ok(walkdto);
        }

        //Get Walks
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var walkDomainModel = await walkRepository.GetAsync();

            return Ok(mapper.Map<List<WalkDTO>>(walkDomainModel));
        }

        //get Walk by Id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetbyId([FromRoute] Guid id)
        {
            var walks = await walkRepository.GetByIdAsync(id);
            if (walks == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDTO>(walks));
        }

        //update walks
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdatebyId([FromRoute] Guid id, [FromBody] UpdateWalkRequestDTO updateWalkRequestdto)
        {
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestdto);
            var walks = await walkRepository.UpdatebyIdAsync(id, walkDomainModel);
            if (walks == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDTO>(walks));

        }

        //Delete Walks
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var existingWalk = await walkRepository.DeleteAsync(id);
            if(existingWalk == null)
            {
                return NotFound();
            }
            return Ok(existingWalk);
        }
    }
}
