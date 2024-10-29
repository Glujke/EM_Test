using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM_TestRepository.Entity
{
    public class Request
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public DateTime RequestTime { get; set; }
        public string Answer { get; set; }
    }
}
