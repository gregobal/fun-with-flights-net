using Flights.Api.Controllers.Base;
using Flights.Dal.Repos.Base;
using Flights.Dal.Repos.Interfaces;
using Flights.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Flights.Api.Controllers;

[Route("api/[controller]")]
public class AircraftsController: BaseCrudController<Aircraft, AircraftsController>
{
    public AircraftsController(IAircraftRepo repo) 
        : base(repo)
    {
    }
}