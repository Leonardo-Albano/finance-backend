using Finance_BackEnd.Models.DTOs;
using Finance_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Finance_BackEnd.Services.Interfaces;

namespace Finance_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PeopleController(IPersonService personService)
        {
            _personService = personService;
        }

        /// <summary>
        /// Obtém todas as pessoas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Person>>> Get()
        {
            var people = await _personService.GetAllAsync();
            return Ok(people);
        }

        /// <summary>
        /// Obtém uma pessoa por ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Person>> GetById(int id)
        {
            var person = await _personService.GetByIdAsync(id);
            return person != null ? Ok(person) : NotFound();
        }

        /// <summary>
        /// Cria uma nova pessoa.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Person>> Post([FromBody] PersonDTO request)
        {
            var newPerson = await _personService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = newPerson.Id }, newPerson);
        }

        /// <summary>
        /// Atualiza uma pessoa existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] PersonDTO request)
        {
            var updated = await _personService.UpdateAsync(id, request);
            if (updated == null) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Exclui uma pessoa.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _personService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}