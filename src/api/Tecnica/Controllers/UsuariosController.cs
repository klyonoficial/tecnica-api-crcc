using Microsoft.AspNetCore.Mvc;

namespace Tecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUsuarios()
        {
            return Ok("Lista de usuários");
        }

        [HttpGet("{id}")]
        public IActionResult GetUsuario(int id)
        {
            return Ok($"Usuário com ID: {id}");
        }

        [HttpPost]
        public IActionResult CreateUsuario([FromBody] object usuario)
        {
            return CreatedAtAction(nameof(GetUsuario), new { id = 1 }, usuario);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUsuario(int id, [FromBody] object usuario)
        {
            return Ok($"Usuário {id} atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUsuario(int id)
        {
            return Ok($"Usuário {id} removido");
        }
    }
}