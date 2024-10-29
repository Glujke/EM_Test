using EM_TestRepository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM_TestRepository.Repository
{
    public interface ISortable<TEntity>
    {
        Task<IEnumerable<TEntity>> Sort(int idLocation, DateTime date);
    }
}
