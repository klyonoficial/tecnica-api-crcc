using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Daos;
using Tecnica.Domain.Dtos;
using Tecnica.Domain.Entities;

namespace Tecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteDao _clienteDao;
        private readonly ObservacaoDao _observacaoDao;
        private readonly ApplicationDbContext _context;

        public ClientesController(ClienteDao clienteDao, ObservacaoDao observacaoDao, ApplicationDbContext context)
        {
            _clienteDao = clienteDao;
            _observacaoDao = observacaoDao;
            _context = context;
        }

        // GET /cliente?status=&filtro=
        [HttpGet]
        public async Task<IActionResult> GetClientes([FromQuery] string? status, [FromQuery] string? filtro)
        {
            var clientes = await _clienteDao.ListarPorFiltroAsync(status, filtro);

            var result = clientes.Select(c => new ClienteResultDto
            {
                Id = c.Id,
                CpfCnpj = c.CpfCnpj,
                RazaoSocial = c.RazaoSocial,
                NomeFantasia = c.NomeFantasia,
                Contato = c.Contato,
                Telefone = string.IsNullOrWhiteSpace(c.TelefoneNumero) ? null : $"({c.TelefoneDdd}) {c.TelefoneNumero}",
                Status = c.Status
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCliente(long id)
        {
            var cliente = await _clienteDao.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound();

            return Ok(ToDto(cliente));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCliente([FromBody] ClienteDto dto)
        {
            // Map Enderecos DTOs
            List<Endereco>? enderecos = null;
            if (dto.Enderecos != null && dto.Enderecos.Any())
            {
                enderecos = dto.Enderecos.Select(e => e.ToEntity()).ToList();
            }

            // Map Arquivos DTOs (if ArquivoDto exists with needed props)
            List<Arquivo>? arquivos = null;
            if (dto.Arquivos != null && dto.Arquivos.Any())
            {
                arquivos = dto.Arquivos.Select(a => a.ToEntity()).ToList();
            }

            var cliente = new Cliente(dto.VendedorId!.Value, dto.CpfCnpj, dto.RazaoSocial, dto.NomeFantasia, dto.Contato, dto.TelefoneDdd, dto.TelefoneNumero, dto.DiaCobranca!.Value, dto.DadosSerasa, enderecos, arquivos);
            await _clienteDao.AdicionarAsync(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, ToDto(cliente));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCliente([FromBody] ClienteDto dto)
        {
            var cliente = await _clienteDao.ObterPorIdAsync(dto.Id!.Value);
            if (cliente == null)
                return NotFound();

            // Use domain entity method to perform update (includes status normalization)
            // Map EnderecoDto list to Endereco entities
            List<Endereco>? enderecos = null;
            if (dto.Enderecos != null && dto.Enderecos.Any())
            {
                enderecos = dto.Enderecos.Select(e => new Endereco(
                    e.Cep,
                    e.Logradouro,
                    e.CodigoIbge,
                    e.Bairro,
                    e.UF,
                    e.Numero,
                    e.Complemento,
                    e.Tipo
                )).ToList();
            }

            cliente.Atualizar(
                dto.VendedorId!.Value,
                dto.CpfCnpj,
                dto.RazaoSocial,
                dto.NomeFantasia,
                dto.Contato,
                dto.TelefoneDdd,
                dto.TelefoneNumero,
                dto.DiaCobranca!.Value,
                enderecos,
                dto.DadosSerasa);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}/aprovar")]
        public async Task<IActionResult> Aprovar(long id)
        {
            var cliente = await _clienteDao.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound();

            cliente.Aprovar();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}/reprovar")]
        public async Task<IActionResult> Reprovar(long id)
        {
            var cliente = await _clienteDao.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound();

            cliente.Reprovar();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}/inativar")]
        public async Task<IActionResult> Inativar(long id)
        {
            var cliente = await _clienteDao.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound();

            cliente.Inativar();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}/resumo-compras")]
        public async Task<IActionResult> ResumoCompras(long id)
        {
            // Minimal implementation: aggregate vendas if table exists
            var vendas = await _context.Set<Venda>().Where(v => v.PessoaId == id).ToListAsync();
            var total = vendas.Sum(v => v.ValorTotal);
            var count = vendas.Count;
            var last = vendas.OrderByDescending(v => v.DataCadastro).FirstOrDefault()?.DataCadastro;

            var resumo = new ClienteResumoComprasDto
            {
                ClienteId = id,
                TotalComprado = total,
                QuantidadeVendas = count,
                DataUltimaCompra = last
            };

            return Ok(resumo);
        }

        private ClienteDto ToDto(Cliente c)
        {
            return new ClienteDto
            {
                Id = c.Id,
                VendedorId = c.VendedorId,
                CpfCnpj = c.CpfCnpj,
                RazaoSocial = c.RazaoSocial,
                NomeFantasia = c.NomeFantasia,
                Contato = c.Contato,
                TelefoneDdd = c.TelefoneDdd,
                TelefoneNumero = c.TelefoneNumero,
                DiaCobranca = c.DiaCobranca,
                Status = c.Status,
                DataAprovou = c.DataAprovou,
                DataCadastro = c.DataCadastro,
                DadosSerasa = c.DadosSerasa,
                Enderecos = c.Enderecos?.Select(e => ToDto(e)).Where(e => e != null).Cast<EnderecoDto>().ToList() ?? new List<EnderecoDto>(),
                Arquivos = c.Arquivos?.Select(a => ToDto(a)).Where(a => a != null).Cast<ArquivoDto>().ToList() ?? new List<ArquivoDto>()
            };
        }

        private EnderecoDto? ToDto(Endereco endereco)
        {
            if (endereco == null) return null;

            return new EnderecoDto
            {
                Id = endereco.Id,
                PessoaId = endereco.PessoaId,
                Cep = endereco.Cep,
                Logradouro = endereco.Logradouro ?? string.Empty,
                CodigoIbge = endereco.CodigoIbge,
                Bairro = endereco.Bairro,
                UF = endereco.UF,
                Numero = endereco.Numero,
                Complemento = endereco.Complemento,
                Tipo = endereco.Tipo
            };
        }

        private ArquivoDto? ToDto(Arquivo arquivo)
        {
            if (arquivo == null) return null;

            return new ArquivoDto
            {
                Id = arquivo.Id,
                PessoaId = arquivo.PessoaId,
                Tipo = arquivo.Tipo,
                Nome = arquivo.Nome
            };
        }
    }
}
