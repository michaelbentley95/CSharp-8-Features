using Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesktopApp.Services
{
    public class PlansService : IPlansService
    {
        public Task<IEnumerable<Plan>> All()
        {
            throw new NotImplementedException();
        }

        public Task<Plan> Create(Plan plan)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOne(int id)
        {
            throw new NotImplementedException();
        }
    }
}
