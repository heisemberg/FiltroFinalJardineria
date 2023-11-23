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
    public class EmpleadoController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmpleadoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<EmpleadoDto>>> Get()
        {
            var empleados = await _unitOfWork.Empleados.GetAllAsync();
            return _mapper.Map<List<EmpleadoDto>>(empleados);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmpleadoDto>> Get(int id)
        {
            var empleado = await _unitOfWork.Empleados.GetByIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return _mapper.Map<EmpleadoDto>(empleado);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmpleadoDto>> Post(EmpleadoDto empleadoDto)
        {
            var empleados = _mapper.Map<Empleado>(empleadoDto);
            _unitOfWork.Empleados.Add(empleados);
            await _unitOfWork.SaveAsync();
            if (empleados == null)
            {
                return BadRequest();
            }
            empleadoDto.CodigoEmpleado = empleados.CodigoEmpleado;
            return CreatedAtAction(nameof(Post), new { id = empleadoDto.CodigoEmpleado }, empleadoDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmpleadoDto>> Put(int id, [FromBody] EmpleadoDto empleadoDto)
        {
            if (empleadoDto.CodigoEmpleado == 0)
            {
                empleadoDto.CodigoEmpleado = id;
            }
            if (empleadoDto.CodigoEmpleado != id)
            {
                return NotFound();
            }
            var empleado = _mapper.Map<Empleado>(empleadoDto);
            empleadoDto.CodigoEmpleado = empleado.CodigoEmpleado;
            _unitOfWork.Empleados.Update(empleado);
            await _unitOfWork.SaveAsync();
            return empleadoDto;
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var empleados = await _unitOfWork.Empleados.GetByIdAsync(id);
            if (empleados == null)
            {
                return NotFound();
            }
            _unitOfWork.Empleados.Remove(empleados);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}