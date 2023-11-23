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
    public class GamaProductoController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GamaProductoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<GamaProductoDto>>> Get()
        {
            var gamaproductos = await _unitOfWork.GamaProductos.GetAllAsync();
            return _mapper.Map<List<GamaProductoDto>>(gamaproductos);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GamaProductoDto>> Get(string id)
        {
            var gamaproducto = await _unitOfWork.GamaProductos.GetByIdAsync(int.Parse(id));
            if (gamaproducto == null)
            {
                return NotFound();
            }
            return _mapper.Map<GamaProductoDto>(gamaproducto);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GamaProductoDto>> Post(GamaProductoDto gamaProductoDto)
        {
            var gamaproductos = _mapper.Map<GamaProducto>(gamaProductoDto);
            _unitOfWork.GamaProductos.Add(gamaproductos);
            await _unitOfWork.SaveAsync();
            if (gamaproductos == null)
            {
                return BadRequest();
            }
            gamaProductoDto.Gama = gamaproductos.Gama.ToString();
            return CreatedAtAction(nameof(Post), new { id = gamaProductoDto.Gama }, gamaProductoDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GamaProductoDto>> Put(string id, [FromBody] GamaProductoDto gamaProductoDto)
        {
            if (gamaProductoDto.Gama == "0")
            {
                gamaProductoDto.Gama = id;
            }
            if (gamaProductoDto.Gama != id)
            {
                return NotFound();
            }
            var gamaproducto = _mapper.Map<GamaProducto>(gamaProductoDto);
            gamaProductoDto.Gama = gamaproducto.Gama.ToString();
            _unitOfWork.GamaProductos.Update(gamaproducto);
            await _unitOfWork.SaveAsync();
            return gamaProductoDto;
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var gamaproductos = await _unitOfWork.GamaProductos.GetByIdAsync(int.Parse(id));
            if (gamaproductos == null)
            {
                return NotFound();
            }
            _unitOfWork.GamaProductos.Remove(gamaproductos);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}