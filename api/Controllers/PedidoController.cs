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
    public class PedidoController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PedidoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PedidoDto>>> Get()
        {
            var pedidos = await _unitOfWork.Pedidos.GetAllAsync();
            return _mapper.Map<List<PedidoDto>>(pedidos);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PedidoDto>> Get(int id)
        {
            var pedido = await _unitOfWork.Pedidos.GetByIdAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return _mapper.Map<PedidoDto>(pedido);
        }
        


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PedidoDto>> Post([FromBody]PedidoDto pedidoDto)
        {
            var pedidos = _mapper.Map<Pedido>(pedidoDto);
            if (pedidoDto.FechaPedido == DateOnly.MinValue)
            {
                pedidoDto.FechaPedido = DateOnly.FromDateTime(DateTime.Now);
                pedidos.FechaPedido = DateOnly.FromDateTime(DateTime.Now);
            }
            if (pedidoDto.FechaEsperada == DateOnly.MinValue)
            {
                pedidoDto.FechaEsperada = DateOnly.FromDateTime(DateTime.Now);
                pedidos.FechaEsperada = DateOnly.FromDateTime(DateTime.Now);
            }
            if (pedidoDto.FechaEntrega == DateOnly.MinValue)
            {
                pedidoDto.FechaEntrega = DateOnly.FromDateTime(DateTime.Now);
                pedidos.FechaEntrega = DateOnly.FromDateTime(DateTime.Now);
            }
            _unitOfWork.Pedidos.Add(pedidos);
            await _unitOfWork.SaveAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            pedidoDto.CodigoPedido = pedidos.CodigoPedido;
            return CreatedAtAction(nameof(Post), new { id = pedidoDto.CodigoPedido }, pedidoDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PedidoDto>> Put(int id, [FromBody] PedidoDto pedidoDto)
        {
            if (pedidoDto.CodigoPedido == 0)
            {
                pedidoDto.CodigoPedido = id;
            }
            if (pedidoDto.CodigoPedido != id)
            {
                return NotFound();
            }
            var pedido = _mapper.Map<Pedido>(pedidoDto);
            if (pedidoDto.FechaPedido == DateOnly.MinValue)
            {
                pedidoDto.FechaPedido = DateOnly.FromDateTime(DateTime.Now);
                pedido.FechaPedido = DateOnly.FromDateTime(DateTime.Now);
            }
            if (pedidoDto.FechaEsperada == DateOnly.MinValue)
            {
                pedidoDto.FechaEsperada = DateOnly.FromDateTime(DateTime.Now);
                pedido.FechaEsperada = DateOnly.FromDateTime(DateTime.Now);
            }
            if (pedidoDto.FechaEntrega == DateOnly.MinValue)
            {
                pedidoDto.FechaEntrega = DateOnly.FromDateTime(DateTime.Now);
                pedido.FechaEntrega = DateOnly.FromDateTime(DateTime.Now);
            }
            pedidoDto.CodigoPedido = pedido.CodigoPedido;
            _unitOfWork.Pedidos.Update(pedido);
            await _unitOfWork.SaveAsync();
            return pedidoDto;
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var pedidos = await _unitOfWork.Pedidos.GetByIdAsync(id);
            if (pedidos == null)
            {
                return NotFound();
            }
            _unitOfWork.Pedidos.Remove(pedidos);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}