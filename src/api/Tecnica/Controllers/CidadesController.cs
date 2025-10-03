using Microsoft.AspNetCore.Mvc;
using Tecnica.Domain.Daos;

namespace Tecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CidadesController : ControllerBase
    {
        private readonly CidadeDao _cidadeDao;

        public CidadesController(CidadeDao cidadeDao)
        {
            _cidadeDao = cidadeDao;
        }

        // GET /api/cidades/by-uf?uf=SP
        [HttpGet("by-uf")]
        public async Task<IActionResult> GetCidadesByUF([FromQuery] string uf)
        {
            if (string.IsNullOrWhiteSpace(uf))
            {
                return BadRequest("UF é obrigatória");
            }

            var cidades = await _cidadeDao.BuscarPorUfAsync(uf);

            return Ok(cidades);
        }
    }
}
