using Microsoft.AspNetCore.Mvc;
using Tecnica.Domain.Daos;
using Tecnica.Domain.Dtos;
using Tecnica.Domain.Entities;

namespace Tecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FornecedorController : ControllerBase
    {
        private readonly FabricanteDao _fornecedorDao;
        private readonly ApplicationDbContext _context;

        public FornecedorController(FabricanteDao fornecedorDao, ApplicationDbContext context)
        {
            _fornecedorDao = fornecedorDao;
            _context = context;
        }

        // GET /fabricante  - retorna lista ordenada por nome
        [HttpGet]
        public async Task<IActionResult> GetFabricantes()
        {
            var lista = await _fornecedorDao.ListarTodosAsync();
            var ordenada = lista.OrderBy(f => f.Nome).Select(ToDto);
            return Ok(ordenada);
        }

        // GET /fabricante/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFabricante(long id)
        {
            var fornecedor = await _fornecedorDao.ObterPorIdAsync(id);
            if (fornecedor == null)
                return NotFound();
            return Ok(ToDto(fornecedor));
        }

        // POST /fabricante
        [HttpPost]
        public async Task<IActionResult> CreateFabricante([FromBody] FornecedorDto fornecedorDto)
        {
            var fornecedor = ToEntity(fornecedorDto);
            await _fornecedorDao.AdicionarAsync(fornecedor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFabricante), new { id = fornecedor.Id });
        }

        // PUT /fabricante
        [HttpPut]
        public async Task<IActionResult> UpdateFabricante([FromBody] FornecedorDto fornecedorDto)
        {
            var fornecedor = await _fornecedorDao.ObterPorIdAsync(fornecedorDto.Id);
            if (fornecedor == null)
                return NotFound();

            fornecedor.Atualizar(fornecedorDto.Nome);

            await _context.SaveChangesAsync();

            return Ok();
        }

        private FornecedorDto ToDto(Fabricante f)
        {
            return new FornecedorDto
            {
                Id = f.Id,
                Nome = f.Nome
            };
        }

        private Fabricante ToEntity(FornecedorDto dto)
        {
            return new Fabricante(dto.Nome);
        }
    }
}
