using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Collections.Generic;
using System.Security.Permissions;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,IMapper mapper, ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles ="Reader, Writer")]
        public async Task<IActionResult> GetAll()
        {
            //Geet data from Database - domain model
            var regionsDomain = await regionRepository.GetAllAsync();
            //map domain model to dto
            //var regionsDto = new List<RegionDto>();
            //foreach(var regionDomain in regionsDomain)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = regionDomain.Id,
            //        Name = regionDomain.Name,
            //        Code = regionDomain.Code,
            //        RegionImageUrl = regionDomain.RegionImageUrl
            //    });

            //}

            //Map domain model to dto
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

            //Return dto
            return Ok(regionsDto);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Reader, Writer")]
        public async Task<IActionResult> Get(Guid id)
        {
           // var regions=dbContext.Regions.Find(id);

            var regionsDomain = await regionRepository.GetByIdAsync(id);

            if (regionsDomain == null)
            {
                return NotFound();
            }
            //map domain to dto

            //var regionsDto = new RegionDto
            //{
            //    Id = regionsDomain.Id,
            //    Name = regionsDomain.Name,
            //    Code = regionsDomain.Code,
            //    RegionImageUrl = regionsDomain.RegionImageUrl
            //};

            var regionsDto = mapper.Map<RegionDto>(regionsDomain);

            return Ok(regionsDto);

        }
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
           
                //MAp Dto to domain model
                //var regionDomainModel = new Region
                //{
                //    Code = addRegionRequestDto.Code,
                //    Name = addRegionRequestDto.Name,
                //    RegionImageUrl = addRegionRequestDto.RegionImageUrl
                //};

                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                //Use domain model to create region
                await regionRepository.CreateAsync(regionDomainModel);
                await dbContext.SaveChangesAsync();

                //var regionsDto = new RegionDto
                //{
                //    Id = regionDomainModel.Id,
                //    Name = regionDomainModel.Name,
                //    Code = regionDomainModel.Code,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl
                //};

                var regionsDto = mapper.Map<RegionDto>(regionDomainModel);
                return Ok(regionsDto);

           
        }

        [HttpPut("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer, Reader")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            

                //map dto to domain model
                //var regionDomainModel = new Region
                //{
                //    Code = updateRegionRequestDto.Code,
                //    Name = updateRegionRequestDto.Name,
                //    RegionImageUrl = updateRegionRequestDto.RegionImageUrl
                //};
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);


                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                ////map dto to domain model
                //regionDomainModel.Code = updateRegionRequestDto.Code;
                //regionDomainModel.Name = updateRegionRequestDto.Name;
                //regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

                await dbContext.SaveChangesAsync();

                //convert domain model to dto
                //var regionDto = new RegionDto
                //{
                //    Id = regionDomainModel.Id,
                //    Name = regionDomainModel.Name,
                //    Code = regionDomainModel.Code,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl
                //};
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                return Ok(regionDto);
            
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Writer, reader")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //return deleted region back
            //map domain model to dto
            //var regionDto = new Region
            //{
            //    Id = regionDomainModel.Id,
            //    Name = regionDomainModel.Name,
            //    Code = regionDomainModel.Code,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            var regionDto = mapper.Map<RegionDto> (regionDomainModel);
            return Ok(regionDto);
        }
    }
}
