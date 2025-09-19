using NvsBank.Domain.Entities;
using NvsBank.Domain.Interfaces;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context)
    {
    }
}