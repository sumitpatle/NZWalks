using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // https://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext,
            IRegionRepository regionRepository,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // GET All Regions
        // GET: https://localhost:1234/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //get Data from databse - domain  models
            var regionDomain = await regionRepository.GetAllAsync();
            //map domain models to DTO
            //var regiondto = new List<RegionDTO>();
            //foreach (var item in regionDomain)
            //{
            //    regiondto.Add(new RegionDTO()
            //    {
            //        Name = item.Name,
            //        Code = item.Code,
            //        RegionImageUrl = item.RegionImageUrl
            //    });
            //}

            var regiondto = mapper.Map<List<RegionDTO>>(regionDomain);
            //return DTOs
            return Ok(regiondto);
        }


        // Get region by Id
        // GET : https//localhost:1234/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //1st way to that: find method take the primary key only, not able to use Code or name.
            //var regionDomain = dbContext.Regions.Find(id);

            //2nd way to do that
            var regionDomain = await regionRepository.GetbyIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //map domain models to DTO
            //var regiondto = new RegionDTO
            //{
            //    Name = regionDomain.Name,
            //    Code = regionDomain.Code,
            //    RegionImageUrl = regionDomain.RegionImageUrl
            //};
            var regiondto = mapper.Map<RegionDTO>(regionDomain);

            return Ok(regiondto);
        }

        //POST : https:localhost:1234/api/regions/create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionrequestDto addRegionrequestDto)
        {
            //map dto to domain model 
            //var regionDomainModel = new Region
            //{
            //    Code = addRegionrequestDto.Code,
            //    Name = addRegionrequestDto.Name,
            //    RegionImageUrl = addRegionrequestDto.RegionImageUrl
            //};

            //Directly Map to domainModel from dto
            var regionDomainModel = mapper.Map<Region>(addRegionrequestDto);
            //use domain model to create region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //map domain model to dto
            //var regiondto = new RegionDTO
            //{
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl

            //};

            var regiondto = mapper.Map<RegionDTO>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regiondto);

        }

        //PUT : https://localhost:1234/api/regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RegionDTO regionDTO)
        {
            //map dto to domain model
            //var regionDomainModel = new Region
            //{
            //    Code = regionDTO.Code,
            //    Name = regionDTO.Name,
            //    RegionImageUrl = regionDTO.RegionImageUrl
            //};

            var regionDomainModel = mapper.Map<Region>(regionDTO);

            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //convert domain model to dto
            //var regiondto = new RegionDTO
            //{
            //    Name = regionDomainModel.Name,
            //    Code = regionDomainModel.Code,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            var regiondto = mapper.Map<RegionDTO>(regionDomainModel);
            return Ok(regiondto);

        }

        //DELETE : https//localhost:1234/api/region/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //return deleted region back
            //var regiondto = new RegionDTO
            //{
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            var regiondto = mapper.Map<RegionDTO>(regionDomainModel);
            return Ok(regiondto);

        }
    }
}
