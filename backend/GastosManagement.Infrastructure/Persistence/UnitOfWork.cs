using System;
using System.Collections.Generic;
using System.Text;

using GastosManagement.Application.Interfaces;

namespace GastosManagement.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GastosDbContext _context;

        public UnitOfWork(GastosDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}