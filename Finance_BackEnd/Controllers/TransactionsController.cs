using Finance_BackEnd.Models.DTOs;
using Finance_BackEnd.Models.Responses;
using Finance_BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Finance_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Obtém todas as transações.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TransactionResponse>>> Get()
        {
            var transactions = await _transactionService.GetAllAsync();
            return Ok(transactions);
        }

        /// <summary>
        /// Obtém uma transação por ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransactionResponse>> GetById(int id)
        {
            var transaction = await _transactionService.GetByIdAsync(id);
            return transaction != null ? Ok(transaction) : NotFound();
        }

        /// <summary>
        /// Cria uma nova transação.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TransactionResponse>> Post([FromBody] TransactionDto request)
        {
            var newTransaction = await _transactionService.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = newTransaction.Id }, newTransaction);
        }

        /// <summary>
        /// Atualiza uma transação existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] TransactionDto request)
        {
            var updated = await _transactionService.UpdateAsync(id, request);
            if (updated == null) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Exclui uma transação.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _transactionService.DeleteAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Obtém o relatório financeiro consolidado por pessoa.
        /// </summary>
        [HttpGet("report")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReportPerPersonResponse>>> GetReport()
        {
            var report = await _transactionService.GetReportPerPersonAsync();
            return Ok(report);
        }
    }
}