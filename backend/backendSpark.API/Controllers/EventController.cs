using backendSpark.Model.Entities;
using backendSpark.Model.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Spark.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    protected EventRepository Repository { get; }

    public EventController(EventRepository repository)
    {
        Repository = repository;
    }

    [HttpGet("{id}")]
    public ActionResult<Event> GetEvent([FromRoute] int id)
    {
        Event @event = Repository.GetEventById(id);
        if (@event == null)
        {
            return NotFound();
        }
        return Ok(@event);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Event>> GetEvents()
    {
        return Ok(Repository.GetEvents());
    }

    [HttpPost]
    public ActionResult Post([FromBody] Event @event)
    {
        if (@event == null)
        {
            return BadRequest("Event info not correct");
        }
        bool status = Repository.InsertEvent(@event);
        if (status)
        {
            return Ok();
        }
        return BadRequest();
    }

    [HttpPut]
    public ActionResult UpdateEvent([FromBody] Event @event)
    {
        if (@event == null)
        {
            return BadRequest("Event info not correct");
        }
        Event existingEvent = Repository.GetEventById(@event.Id);
        if (existingEvent == null)
        {
            return NotFound($"Event with id {@event.Id} not found");
        }
        bool status = Repository.UpdateEvent(@event);
        if (status)
        {
            return Ok();
        }
        return BadRequest("Something went wrong");
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteEvent([FromRoute] int id)
    {
        Event existingEvent = Repository.GetEventById(id);
        if (existingEvent == null)
        {
            return NotFound($"Event with id {id} not found");
        }
        bool status = Repository.DeleteEvent(id);
        if (status)
        {
            return NoContent();
        }
        return BadRequest($"Unable to delete event with id {id}");
    }
}
