using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class OficinaController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OficinaController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OficinaDto>>> Get()
        {
            var oficinas = await _unitOfWork.Oficinas.GetAllAsync();
            return _mapper.Map<List<OficinaDto>>(oficinas);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OficinaDto>> Get(string id)
        {
            var oficina = await _unitOfWork.Oficinas.GetByIdAsync(id);
            if (oficina == null)
            {
                return NotFound();
            }
            return _mapper.Map<OficinaDto>(oficina);
        }
        

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OficinaDto>> Post(OficinaDto oficinaDto)
        {
            var oficinas = _mapper.Map<Oficina>(oficinaDto);
            _unitOfWork.Oficinas.Add(oficinas);
            await _unitOfWork.SaveAsync();
            if (oficinas == null)
            {
                return BadRequest();
            }
            oficinaDto.CodigoOficina = oficinas.CodigoOficina.ToString();
            return CreatedAtAction(nameof(Post), new { id = oficinaDto.CodigoOficina }, oficinaDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OficinaDto>> Put(string id, [FromBody] OficinaDto oficinaDto)
        {
            if (oficinaDto.CodigoOficina == "0")
            {
                oficinaDto.CodigoOficina = id;
            }
            if (oficinaDto.CodigoOficina != id)
            {
                return NotFound();
            }
            var oficina = _mapper.Map<Oficina>(oficinaDto);
            oficinaDto.CodigoOficina = oficina.CodigoOficina.ToString();
            _unitOfWork.Oficinas.Update(oficina);
            await _unitOfWork.SaveAsync();
            return oficinaDto;
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var oficinas = await _unitOfWork.Oficinas.GetByIdAsync(id);
            if (oficinas == null)
            {
                return NotFound();
            }
            _unitOfWork.Oficinas.Remove(oficinas);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}