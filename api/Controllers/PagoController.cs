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
    public class PagoController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PagoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PagoDto>>> Get()
        {
            var pagos = await _unitOfWork.Pagos.GetAllAsync();
            return _mapper.Map<List<PagoDto>>(pagos);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagoDto>> Get(string id)
        {
            var pago = await _unitOfWork.Pagos.GetByIdAsync(int.Parse(id));
            if (pago == null)
            {
                return NotFound();
            }
            return _mapper.Map<PagoDto>(pago);
        }
        

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagoDto>> Post(PagoDto pagoDto)
        {
            var pagos = _mapper.Map<Pago>(pagoDto);
            if (pagoDto.FechaPago == DateOnly.MinValue)
            {
                pagoDto.FechaPago = DateOnly.FromDateTime(DateTime.Now);
            }
            _unitOfWork.Pagos.Add(pagos);
            await _unitOfWork.SaveAsync();
            if (pagos == null)
            {
                return BadRequest();
            }
            pagoDto.IdTransaccion = pagos.IdTransaccion.ToString();
            return CreatedAtAction(nameof(Post), new { id = pagoDto.IdTransaccion }, pagoDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagoDto>> Put(string id, [FromBody] PagoDto pagoDto)
        {
            if (pagoDto.IdTransaccion == "")
            {
                pagoDto.IdTransaccion = id;
            }
            if (pagoDto.IdTransaccion != id)
            {
                return NotFound();
            }
            var pago = _mapper.Map<Pago>(pagoDto);
            pagoDto.IdTransaccion = pago.IdTransaccion.ToString();
            _unitOfWork.Pagos.Update(pago);
            await _unitOfWork.SaveAsync();
            return pagoDto;
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var pagos = await _unitOfWork.Pagos.GetByIdAsync(int.Parse(id));
            if (pagos == null)
            {
                return NotFound();
            }
            _unitOfWork.Pagos.Remove(pagos);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}