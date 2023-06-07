using Flights.Dal.Exceptions;
using Flights.Dal.Repos.Base;
using Flights.Models.Entities.Base;
using Microsoft.AspNetCore.Mvc;

namespace Flights.Api.Controllers.Base;

[ApiController]
public abstract class BaseCrudController<T, TController>: ControllerBase 
    where T : BaseEntity, new()
    where TController: BaseCrudController<T, TController>
{
    protected readonly IRepo<T> _repo;

    protected BaseCrudController(IRepo<T> repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public ActionResult<IEnumerable<T>> GetAll()
    {
        return Ok(_repo.GetAllIgnoreQueryFilters());
    }

    [HttpGet("id")]
    public ActionResult<T> GetOne(string id)
    {
        var entity = _repo.Find(id);
        if (entity is null) return NotFound();
        return Ok(entity);
    }

    [HttpPost]
    public IActionResult AddOne(T entity)
    {
        try
        {
            _repo.Add(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }

        return CreatedAtAction(nameof(GetOne), new { id = entity.Id }, entity);
    }

    [HttpPut("id")]
    public IActionResult UpdateOne(T entity)
    {
        try
        {
            _repo.Update(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }

        return Ok(entity);
    }

    [HttpDelete]
    public IActionResult DeleteOne(T entity)
    {
        try
        {
            _repo.Delete(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }

        return Ok();
    }
}