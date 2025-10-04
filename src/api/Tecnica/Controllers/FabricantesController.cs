using Microsoft.AspNetCore.Mvc;
using Tecnica.Domain.Daos;
using Tecnica.Domain.Dtos;
using Tecnica.Domain.Entities;

namespace Tecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FabricantesController : ControllerBase
    {
        private readonly FabricanteDao _fabricanteDao;
        private readonly ApplicationDbContext _context;

        public FabricantesController(FabricanteDao fabricanteDao, ApplicationDbContext context)
        {
            _fabricanteDao = fabricanteDao;
            _context = context;
        }

        // GET /fabricante  - retorna lista ordenada por nome
        [HttpGet]
        public async Task<IActionResult> GetFabricantes()
        {
            var lista = await _fabricanteDao.ListarTodosAsync();
            var ordenada = lista.OrderBy(f => f.Nome).Select(ToDto);
            return Ok(ordenada);
        }

        // GET /fabricante/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFabricante(long id)
        {
            var fabricante = await _fabricanteDao.ObterPorIdAsync(id);
            if (fabricante == null)
                return NotFound();
            return Ok(ToDto(fabricante));
        }

        // POST /fabricante
        [HttpPost]
        public async Task<IActionResult> CreateFabricante([FromBody] FabricanteDto fabricanteDto)
        {
            var fabricante = ToEntity(fabricanteDto);
            await _fabricanteDao.AdicionarAsync(fabricante);
            await _context.SaveChangesAsync();

            return Ok(ToDto(fabricante));
            //TODO: REVISAR - System.InvalidOperationException: No route matches the supplied values.
            //return CreatedAtAction(nameof(GetFabricante), new { id = fabricante.Id });
        }

        // PUT /fabricante
        [HttpPut]
        public async Task<IActionResult> UpdateFabricante([FromBody] FabricanteDto fabricanteDto)
        {
            var fabricante = await _fabricanteDao.ObterPorIdAsync(fabricanteDto.Id);
            if (fabricante == null)
                return NotFound();

            fabricante.Atualizar(fabricanteDto.Nome);

            await _context.SaveChangesAsync();

            return Ok();
        }

        private FabricanteDto ToDto(Fabricante f)
        {
            return new FabricanteDto
            {
                Id = f.Id,
                Nome = f.Nome
            };
        }

        private Fabricante ToEntity(FabricanteDto dto)
        {
            return new Fabricante(dto.Nome);
        }
    }
}
