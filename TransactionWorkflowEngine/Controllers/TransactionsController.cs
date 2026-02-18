using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionWorkflowEngine.Dtos;
using TransactionWorkflowEngine.Handlers.TransactionsHandler;

namespace TransactionWorkflowEngine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsHandler _transactionsHandler;

        public TransactionsController(ITransactionsHandler transactionsHandler)
        {
            _transactionsHandler = transactionsHandler;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var result = await _transactionsHandler.GetTransactionByIdAsync(id, ct);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var result = await _transactionsHandler.CreateTransactionAsync(ct);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id:guid}/available-transitions")]
        [ProducesResponseType(typeof(AvailableTransitionsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAvailableTransitions(Guid id, CancellationToken ct)
        {
            var result = await _transactionsHandler.GetAvailableTransitionsAsync(id, ct);
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("{id:guid}/transition")]
        public async Task<IActionResult> Transition(Guid id, TransitionRequestDto request, CancellationToken ct)
        {
            try
            {
                var result = await _transactionsHandler.TransitionTransactionAsync(id, request.ToStatusId, request.Reason, ct);
                if (result is null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
