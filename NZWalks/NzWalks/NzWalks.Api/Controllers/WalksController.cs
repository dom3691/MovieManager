﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NzWalks.Api.CustomActionFilters;
using NzWalks.Api.Models.Domain;
using NzWalks.Api.Models.DTO;
using NzWalks.Api.Repositories;

namespace NzWalks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        //CREATE WALK 
        //POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {

            //Map Dto to Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateAsync(walkDomainModel);

            //Map Dommain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        //GET ALL WALKS
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walkDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true
                , pageNumber, pageSize);

            //map Domain Model to Dto 
            return Ok(mapper.Map<List<WalkDto>>(walkDomainModel));
        }


        //Get Walk by Id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }
            //Map Domain Model to Dto
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        //Update Walk by Id
        [HttpPut]
        [ValidateModel]
        [Route("{id:Guid}")]

        public async Task<ActionResult> UpdateWalkAsync([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {

            //Map Dto to Domain Model 
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            await walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            //Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);
            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }
            //Map Domain model to dto
            return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
        }

    }
}
