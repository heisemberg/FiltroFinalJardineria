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
    public class DetallePedidoController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DetallePedidoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DetallePedidoDto>>> Get()
        {
            var detallepedidos = await _unitOfWork.DetallePedidos.GetAllAsync();
            return _mapper.Map<List<DetallePedidoDto>>(detallepedidos);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DetallePedidoDto>> Get(int id)
        {
            var detallepedido = await _unitOfWork.DetallePedidos.GetByIdAsync(id);
            if (detallepedido == null)
            {
                return NotFound();
            }
            return _mapper.Map<DetallePedidoDto>(detallepedido);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DetallePedidoDto>> Post(DetallePedidoDto detallePedidoDto)
        {
            var detallepedidos = _mapper.Map<DetallePedido>(detallePedidoDto);
            _unitOfWork.DetallePedidos.Add(detallepedidos);
            await _unitOfWork.SaveAsync();
            if (detallepedidos == null)
            {
                return BadRequest();
            }
            detallePedidoDto.CodigoPedido = detallepedidos.CodigoPedido;
            return CreatedAtAction(nameof(Post), new { id = detallePedidoDto.CodigoPedido }, detallePedidoDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DetallePedidoDto>> Put(int id, [FromBody] DetallePedidoDto detallePedidoDto)
        {
            if (detallePedidoDto.CodigoPedido == 0)
            {
                detallePedidoDto.CodigoPedido = id;
            }
            if (detallePedidoDto.CodigoPedido != id)
            {
                return NotFound();
            }
            var detallepedido = _mapper.Map<DetallePedido>(detallePedidoDto);
            detallePedidoDto.CodigoPedido = detallepedido.CodigoPedido;
            _unitOfWork.DetallePedidos.Update(detallepedido);
            await _unitOfWork.SaveAsync();
            return detallePedidoDto;
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var detallepedidos = await _unitOfWork.DetallePedidos.GetByIdAsync(id);
            if (detallepedidos == null)
            {
                return NotFound();
            }
            _unitOfWork.DetallePedidos.Remove(detallepedidos);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}