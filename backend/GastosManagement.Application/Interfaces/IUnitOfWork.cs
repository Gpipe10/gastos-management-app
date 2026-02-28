using System;
using System.Collections.Generic;
using System.Text;

namespace GastosManagement.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}