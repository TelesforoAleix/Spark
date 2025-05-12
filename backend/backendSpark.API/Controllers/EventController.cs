using backendSpark.Model.Entities;
using backendSpark.Model.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Spark.API.Controllers;

[ApiController]
[Route("api/[controller]")]

// This controller handles all operations related to events, including CRUD operations.
// It uses the EventRepository to interact with the data layer.
// The controller is responsible for handling HTTP requests and returning appropriate responses.
public class EventController : ControllerBase
{
    protected EventRepository Repository { get; }

    public EventController(EventRepository repository)
    {
        Repository = repository;
    }


    // GET api/event
    // This method retrieves the only event available (hard-coded eventId(1)).
    [HttpGet()]
    public ActionResult<Event> GetEvent()
    {
        try
        {
            Event @event = Repository.GetEventById(1);
            if (@event == null)
            {
                Console.WriteLine($"Event not found");
                return NotFound();
            }
            return Ok(@event);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetEvent: {ex.Message}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    
    // PUT api/event
    // This method updates an existing event. Can be used to update any event, for scalability.
    [HttpPut]
    public ActionResult UpdateEvent([FromBody] Event @event)
    {
        try
        {
            if (@event == null)
            {
                return BadRequest("Event info not provided");
            }

            // Validate event data
            if (string.IsNullOrEmpty(@event.Name) || @event.EventId <= 0)
            {
                return BadRequest("Invalid event data");
            }

            Event existingEvent = Repository.GetEventById(@event.EventId);
            if (existingEvent == null)
            {
                return NotFound($"Event with id {@event.EventId} not found");
            }

            bool status = Repository.UpdateEvent(@event);
            if (status)
            {
                return Ok(@event); // Return the updated event
            }
            return BadRequest("Failed to update event");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateEvent: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}
