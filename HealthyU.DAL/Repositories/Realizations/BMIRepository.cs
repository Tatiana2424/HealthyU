using HealthyU.DAL.Entities;
using HealthyU.DAL.Repositories.Interfaces;
using HealthyU.DAL.Repositories.Realizations.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyU.DAL.Repositories.Realizations;

public class BMIRepository : RepositoryBase<BMIData>, IBMIRepository
{
    public BMIRepository(HealthyUDbContext dbContext)
        : base(dbContext)
    {
    }
}
