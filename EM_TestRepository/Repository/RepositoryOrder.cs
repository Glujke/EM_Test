using EM_TestRepository.Context;
using EM_TestRepository.Entity;
using Microsoft.EntityFrameworkCore;

namespace EM_TestRepository.Repository
{
    public class RepositoryOrder : RepositoryBase<Order>, ISortable<Order>
    {
        public RepositoryOrder(EM_TestContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<Order>> GetAllAsync() =>
               await Task.FromResult<IEnumerable<Order>>(_context.Orders.Include(o => o.Location));

        public override async Task<Order> GetByIdAsync(int id) =>
               await _context.Orders.Include(o => o.Location).FirstOrDefaultAsync();

        public async Task<IEnumerable<Order>> Sort(int idLocation, DateTime date)
        {
            var dateLast = date.AddMinutes(30);
            var location = await _context.Locations.SingleOrDefaultAsync(l => l.Id == idLocation);
            if (location == null) return null;
            var result = await _context.Orders.Where(o => o.Date >= date)
                                         .Where(o => o.Date <= dateLast)
                                         .Where(o => o.LocationId == location.Id)
                                         .Include(o => o.Location).ToListAsync();
            return result;
        }
    }
}
