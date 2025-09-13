A seguir temos um exemplo de Controller para a classe de dominio Usuario.

### Pontos
- Exemplo de actions que podem existir:
        - Lista todas as entidades
        [HttpGet]
        public async Task<IActionResult> GetEntidadess()

        - Retorna a entidade por id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntidade(int id)

        - Cria uma entidade de Dominio atraves de seu Dto que tambem deve ser criado
        [HttpPost]
        public async Task<IActionResult> CreateEntidade([FromBody] EntidadeDto entidadeDto) 

        - Cria um metodo update para atualizar a entidade
        [HttpPut()]
        public async Task<IActionResult> UpdateEntidade([FromBody] EntidadeDto entidadeDto)

- O controller sempre usa o DAO do objeto de dominio para acessar os dados. Ex: Usuario possui UsuarioDao

- Os Dtos sao convertidos para Domain para serem gravados e atualizados 
e ficam na pasta api/Tecnica/Dtos

- Os objetos de Dominio sao convertidos em Dtos para serem retornados para o endpoint

- Os controllers tem nome no plural: Classe de Dominio Usario vira UsuariosController

- Controllers ficam na pasta api/Tecnica/Controllers

### Codigo de Exemplo

````
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tecnica.Domain.Daos;
using Tecnica.Domain.Dtos;
using Tecnica.Domain.Entities;

namespace Tecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioDao _usuarioDao;
        private readonly ApplicationDbContext _context;

        public UsuariosController(UsuarioDao usuarioDao, ApplicationDbContext context)
        {
            _usuarioDao = usuarioDao;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _usuarioDao.ListarTodosAsync();
            return Ok(usuarios.Select(ToDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _usuarioDao.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound();
            return Ok(ToDto(usuario));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuario([FromBody] UsuarioDto usuarioDto)
        {
            var usuario = ToEntity(usuarioDto);
            await _usuarioDao.AdicionarAsync(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id });
        }

        private UsuarioDto ToDto(Usuario usuario)
        {
            return new UsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Ativo = usuario.Ativo,
                DataCriacao = usuario.DataCriacao
            };
        }

        private Usuario ToEntity(UsuarioDto usuarioDto)
        {
            return new Usuario(usuarioDto.Nome, usuarioDto.Email, "senhaFixa123"); // Senha fixa para exemplo
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateUsuario([FromBody] UsuarioDto usuarioDto)
        {
            var usuario = await _usuarioDao.ObterPorIdAsync(usuarioDto.Id);
            if (usuario == null)
                return NotFound();

            usuario.Atualizar(usuarioDto.Nome, usuarioDto.Email, usuarioDto.Ativo);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
```