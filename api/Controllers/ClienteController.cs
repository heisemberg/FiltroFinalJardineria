using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using api.Dtos;

namespace Api.Controllers
{
    public class ClienteController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClienteController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> Get()
        {
            var clientes = await _unitOfWork.Clientes.GetAllAsync();
            return _mapper.Map<List<ClienteDto>>(clientes);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClienteDto>> Get(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return _mapper.Map<ClienteDto>(cliente);
        }
 
        
        [HttpGet("consulta2")]
        public async Task<IEnumerable<ClienteNoPagos>> GetClienteNoPagos()
        {
            var clientes = await _unitOfWork.Clientes.GetAllAsync();
            var pagos = await _unitOfWork.Pagos.GetAllAsync();
            var empleados = await _unitOfWork.Empleados.GetAllAsync();
            var oficinas = await _unitOfWork.Oficinas.GetAllAsync();

            var consultas = from cliente in clientes
                            join pago in pagos
                            on cliente.CodigoCliente equals pago.CodigoCliente into pagosClientes
                            from pago in pagosClientes.DefaultIfEmpty()
                            join empleado in empleados
                            on cliente.CodigoEmpleadoRepVentas equals empleado.CodigoEmpleado
                            join oficina in oficinas
                            on empleado.CodigoOficina equals oficina.CodigoOficina
                            select new ClienteNoPagos
                            {
                                NombreCliente = cliente.NombreCliente,
                                NombreRepVentas = empleado.Nombre,
                                CiudadOficina = oficina.Ciudad
                            };
            return consultas;
        }

        [HttpGet("consulta7")]
        public async Task<IEnumerable<ClientePedidos>> GetClientePedidos()
        {
            var clientes = await _unitOfWork.Clientes.GetAllAsync();
            var pedidos = await _unitOfWork.Pedidos.GetAllAsync();

            var consultas = from cliente in clientes
                            join pedido in pedidos
                            on cliente.CodigoCliente equals pedido.CodigoCliente into pedidosClientes
                            from pedido in pedidosClientes.DefaultIfEmpty()
                            select new ClientePedidos
                            {
                                NombreCliente = cliente.NombreCliente,
                                CantidadPedidos = pedido.CodigoPedido
                            };
            return consultas;
        }

        [HttpGet("consulta8")]
        public async Task<IEnumerable<ClienteOficina>> GetClienteOficina()
        {
            var clientes = await _unitOfWork.Clientes.GetAllAsync();
            var empleados = await _unitOfWork.Empleados.GetAllAsync();
            var oficinas = await _unitOfWork.Oficinas.GetAllAsync();

            var consultas = from cliente in clientes
                            join empleado in empleados
                            on cliente.CodigoEmpleadoRepVentas equals empleado.CodigoEmpleado
                            join oficina in oficinas
                            on empleado.CodigoOficina equals oficina.CodigoOficina
                            select new ClienteOficina
                            {
                                NombreCliente = cliente.NombreCliente,
                                NombreRepVentas = empleado.Nombre,
                                CiudadOficina = oficina.Ciudad
                            };
            return consultas;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClienteDto>> Post(ClienteDto clienteDto)
        {
            var clientes = _mapper.Map<Cliente>(clienteDto);
            _unitOfWork.Clientes.Add(clientes);
            await _unitOfWork.SaveAsync();
            if (clientes == null)
            {
                return BadRequest();
            }
            clienteDto.CodigoCliente = clientes.CodigoCliente;
            return CreatedAtAction(nameof(Post), new { id = clienteDto.CodigoCliente }, clienteDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClienteDto>> Put(int id, [FromBody] ClienteDto clienteDto)
        {
            if (clienteDto.CodigoCliente == 0)
            {
                clienteDto.CodigoCliente = id;
            }
            if (clienteDto.CodigoCliente != id)
            {
                return NotFound();
            }
            var cliente = _mapper.Map<Cliente>(clienteDto);
            clienteDto.CodigoCliente = cliente.CodigoCliente;
            _unitOfWork.Clientes.Update(cliente);
            await _unitOfWork.SaveAsync();
            return clienteDto;
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var clientes = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (clientes == null)
            {
                return NotFound();
            }
            _unitOfWork.Clientes.Remove(clientes);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}