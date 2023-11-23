using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repositories
{
    public class OficinaRepository : GenericRepository<Oficina>, IOficina
    {
        private readonly GardenContext _context;
        public OficinaRepository(GardenContext context) : base(context)
        {
            _context = context;
        }

    }
}