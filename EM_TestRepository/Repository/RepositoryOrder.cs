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

        public async Task<IEnumerable<Order>> Sort(Location location, DateTime date)
        {
            var loc = await _context.Locations.SingleOrDefaultAsync(l => l.Name.Equals(location.Name));
            if (loc == null) throw new Exception("Location field is incorrect");
            var result = await _context.Orders.Where(o => o.Date >= date).Where(o => o.LocationId == loc.Id).Include(o => o.Location).ToListAsync();
            return result;
        }
    }
}
