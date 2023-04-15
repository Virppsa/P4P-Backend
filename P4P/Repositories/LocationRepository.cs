using P4P.Data;
using P4P.Models;
using P4P.Repositories.Interfaces;

namespace P4P.Repositories;

public class LocationRepository : GenericRepository<Location>, ILocationRepository
{
    public LocationRepository(IP4PContext context) : base(context)
    {
    }
}
