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
    public class ProductoController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> Get()
        {
            var productos = await _unitOfWork.Productos.GetAllAsync();
            return _mapper.Map<List<ProductoDto>>(productos);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoDto>> Get(string id)
        {
            var producto = await _unitOfWork.Productos.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return _mapper.Map<ProductoDto>(producto);
        }

        [HttpGet("consulta4")]
        public async Task<IEnumerable<Producto>> GetProductosMasVendidos()
        {
            var productos = await _unitOfWork.Productos.GetAllAsync();
            var detallePedidos = await _unitOfWork.DetallePedidos.GetAllAsync();
            var pedidos = await _unitOfWork.Pedidos.GetAllAsync();
          


            var consultas = from producto in productos
                            join detallePedido in detallePedidos
                            on producto.CodigoProducto equals detallePedido.CodigoProducto
                            join pedido in pedidos
                            on detallePedido.CodigoPedido equals pedido.CodigoPedido
                            group producto by producto.CodigoProducto into g
                            orderby g.Count() descending
                            select g.Key;

            var productosMasVendidos = new List<Producto>();
            foreach (var item in consultas)
            {
                var producto = await _unitOfWork.Productos.GetByIdAsync(item);
                productosMasVendidos.Add(producto);
            }
            return productosMasVendidos;
        }

        [HttpGet("consulta9")]
        public async Task<IEnumerable<Producto>> GetProductosSinPedidos()
        {
            var productos = await _unitOfWork.Productos.GetAllAsync();
            var detallePedidos = await _unitOfWork.DetallePedidos.GetAllAsync();
            var pedidos = await _unitOfWork.Pedidos.GetAllAsync();

            var consultas = from producto in productos
                            join detallePedido in detallePedidos
                            on producto.CodigoProducto equals detallePedido.CodigoProducto into detallePedidosProductos
                            from detallePedido in detallePedidosProductos.DefaultIfEmpty()
                            join pedido in pedidos
                            on detallePedido.CodigoPedido equals pedido.CodigoPedido into pedidosDetallePedidos
                            from pedido in pedidosDetallePedidos.DefaultIfEmpty()
                            where pedido.CodigoPedido == null
                            select producto;

            return consultas;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoDto>> Post(ProductoDto productoDto)
        {
            var productos = _mapper.Map<Producto>(productoDto);
            _unitOfWork.Productos.Add(productos);
            await _unitOfWork.SaveAsync();
            if (productos == null)
            {
                return BadRequest();
            }
            productoDto.CodigoProducto = productos.CodigoProducto.ToString();
            return CreatedAtAction(nameof(Post), new { id = productoDto.CodigoProducto }, productoDto);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoDto>> Put(string id, [FromBody] ProductoDto productoDto)
        {
            if (productoDto.CodigoProducto == "0")
            {
                productoDto.CodigoProducto = id;
            }
            if (productoDto.CodigoProducto != id)
            {
                return NotFound();
            }
            var producto = _mapper.Map<Producto>(productoDto);
            productoDto.CodigoProducto = producto.CodigoProducto.ToString();
            _unitOfWork.Productos.Update(producto);
            await _unitOfWork.SaveAsync();
            return productoDto;
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var productos = await _unitOfWork.Productos.GetByIdAsync(id);
            if (productos == null)
            {
                return NotFound();
            }
            _unitOfWork.Productos.Remove(productos);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}