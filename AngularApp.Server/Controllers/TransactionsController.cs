using AngularApp.Server.Models;
using Microsoft.AspNetCore.Mvc;
 

namespace AngularApp.Server.Controllers;

// Web API Controller
[ApiController]
[Route("api/[controller]")]
 
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _service;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(ITransactionService service, ILogger<TransactionsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> PostTransaction([FromBody] Transaction transaction)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.AddOrUpdateTransactionAsync(transaction);
        return Ok(new { message = "Transaction processed successfully." });
    }

    [HttpGet("positions")]
    public async Task<IActionResult> GetPositions()
    {
        var result = await _service.GetPositionsAsync();
        return Ok(result);
    }
}

