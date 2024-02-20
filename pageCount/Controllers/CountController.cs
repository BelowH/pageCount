using Microsoft.AspNetCore.Mvc;
using pageCount.Domains;

namespace pageCount.Controllers;

[ApiController]
[Route("[controller]")]
public class CountController : ControllerBase
{

    private readonly ICountService _countService;

    public CountController(ICountService countService)
    {
        _countService = countService;
    }


    [HttpPost]
    public async Task<IActionResult> SubmitCount([FromBody] CountDto countDto)
    {
        try
        {
            bool successful = await _countService.SubmitCount(countDto);
            if (successful)
            {
                return Ok();
            }
            throw new Exception("Failed to submit count.");
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    
    
    
}