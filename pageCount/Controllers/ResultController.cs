using Microsoft.AspNetCore.Mvc;
using pageCount.Domains.Models;

namespace pageCount.Controllers;

[ApiController]
[Route("[controller]")]
public class ResultController : ControllerBase
{

    private readonly IResultService _resultService;

    public ResultController(IResultService resultService)
    {
        _resultService = resultService;
    }


    [HttpGet]
    [Route("[controller]/by/identifier")]
    public async Task<IActionResult> GetResultByIdentifier([FromQuery] string primaryIdentifier,[FromQuery] string secondaryIdentifier = "")
    {
        try
        {
            IEnumerable<Count> result = await _resultService.GetCountsByIdentifier(primaryIdentifier, secondaryIdentifier);
            return Ok(result);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    
    
}