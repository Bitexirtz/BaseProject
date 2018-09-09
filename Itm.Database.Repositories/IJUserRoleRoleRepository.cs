using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Itm.Database.Entities;

namespace Itm.Database.Repositories
{
    public interface IJUserRoleRoleRepository
    {
        IQueryable<JUserRole> GetAll(int pageSize = 10, int pageNumber = 1);

        Task<int> AddAsync(JUserRole entity);

        Task<int> DeleteAsync(JUserRole entity);
    }
}
