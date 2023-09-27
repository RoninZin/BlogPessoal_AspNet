using blogpessoal.Model;
using blogpessoal.Service;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace blogpessoal.Controllers
{
    [Route("~/temas")]
    [ApiController]
    public class TemaController : ControllerBase
    {

        private readonly ITemaService _TemaService;
        private readonly IValidator<Tema> _TemaValidator;

        public TemaController(
            ITemaService TemaService,
            IValidator<Tema> TemaValidator
            )
        {
            _TemaService = TemaService;
            _TemaValidator = TemaValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _TemaService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var Resposta = await _TemaService.GetById(id);

            if (Resposta == null)
                return NotFound();

            return Ok(Resposta);
        }

        [HttpGet("descricao/{descricao}")]
        public async Task<ActionResult> GetByDescricao(string descricao)
        {
            return Ok(await _TemaService.GetByDescricao(descricao));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Tema Tema)
        {
            var validarTema = await _TemaValidator.ValidateAsync(Tema);

            if (!validarTema.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validarTema);
            }

            await _TemaService.Create(Tema);

            return CreatedAtAction(nameof(GetById), new { id = Tema.Id }, Tema);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Tema Tema)
        {
            if (Tema.Id == 0)
                return BadRequest("Id da Tema é inválido!");

            var validarTema = await _TemaValidator.ValidateAsync(Tema);

            if (!validarTema.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validarTema);
            }

            var Resposta = await _TemaService.Update(Tema);

            if (Resposta is null)
                return NotFound("Tema não Encontrada!");

            return Ok(Resposta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var BuscaTema = await _TemaService.GetById(id);

            if (BuscaTema is null)
                return NotFound("Tema não foi Encontrada!");

            await _TemaService.Delete(BuscaTema);

            return NoContent();
        }
    }
}
