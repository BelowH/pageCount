using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using pageCount.Domains;

namespace pageCount.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{

    private readonly IDatabaseRepository _databaseRepository;

    public HealthController(IDatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }

    [HttpGet]
    public IActionResult GetHealth()
    {
        
        try
        {
            bool dbConnection = _databaseRepository.TestConnection();
            HealthResponse healthResponse = new HealthResponse(dbConnection);
            return Ok(healthResponse);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}