using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tecnica.Domain.Daos;
using Tecnica.Domain.Dtos;
using Tecnica.Domain.Entities;

namespace Tecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoDao _ProdutoDao;
        private readonly ApplicationDbContext _context;

        public ProdutosController(ProdutoDao ProdutoDao, ApplicationDbContext context)
        {
            _ProdutoDao = ProdutoDao;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProdutos()
        {
            var Produtos = await _ProdutoDao.ListarTodosAsync();
            return Ok(Produtos.Select(ToDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduto(int id)
        {
            var Produto = await _ProdutoDao.ObterPorIdAsync(id);
            if (Produto == null)
                return NotFound();
            return Ok(ToDto(Produto));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduto([FromBody] ProdutoDto ProdutoDto)
        {
            var Produto = ToEntity(ProdutoDto);
            await _ProdutoDao.AdicionarAsync(Produto);
            await _context.SaveChangesAsync();

            return Ok(ToDto(Produto));
            //TODO: REVISAR - System.InvalidOperationException: No route matches the supplied values.
            //return CreatedAtAction(nameof(GetProduto), new { id = Produto.Id });
        }

        private ProdutoDto ToDto(Produto Produto)
        {
            return new ProdutoDto
            {
                Id = Produto.Id,
                Nome = Produto.Nome,
                FabricanteId = Produto.FabricanteId,
                FabricanteNome = Produto.Fabricante?.Nome ?? string.Empty
            };
        }

        private Produto ToEntity(ProdutoDto ProdutoDto)
        {
            return new Produto(ProdutoDto.Nome, ProdutoDto.FabricanteId);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateProduto([FromBody] ProdutoDto ProdutoDto)
        {
            var Produto = await _ProdutoDao.ObterPorIdAsync(ProdutoDto.Id);
            if (Produto == null)
                return NotFound();

            Produto.Atualizar(ProdutoDto.Nome, ProdutoDto.FabricanteId);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}