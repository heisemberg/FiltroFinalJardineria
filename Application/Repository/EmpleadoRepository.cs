using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Persistence.Data;

namespace Application.Repositories
{
    public class EmpleadoRepository : GenericRepository<Empleado>, IEmpleado
    {
        private readonly GardenContext _context;
        public EmpleadoRepository(GardenContext context) : base(context)
        {
            _context = context;
        }

     
    }
}