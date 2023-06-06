using Flights.Dal.Repos.Base;
using Flights.Models.Entities;

namespace Flights.Dal.Repos.Interfaces;

public interface IAircraftRepo: IRepo<Aircraft, string?>
{
}