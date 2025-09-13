using System.Linq;
using System.Threading.Tasks;
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
            var clientes = await _clienteDao.ListarTodosAsync();

            var query = clientes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(c => c.Status == status);

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.ToLower();
                query = query.Where(c => (c.RazaoSocial ?? "").ToLower().Contains(f)
                    || (c.NomeFantasia ?? "").ToLower().Contains(f)
                    || (c.TelefoneNumero ?? "").ToLower().Contains(f)
                    || (c.Contato ?? "").ToLower().Contains(f)
                    || (c.CpfCnpj ?? "").ToLower().Contains(f));
            }

            var result = query.OrderBy(c => c.RazaoSocial).Select(c => new ClienteResultDto
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
        public async Task<IActionResult> CreateCliente([FromBody] NovoClienteDto dto)
        {
            var cliente = new Cliente(dto.VendedorId, dto.CpfCnpj, dto.RazaoSocial, dto.NomeFantasia, dto.Contato, dto.TelefoneDdd, dto.TelefoneNumero, dto.DiaCobranca, dto.DadosSerasa);
            await _clienteDao.AdicionarAsync(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, ToDto(cliente));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCliente([FromBody] ClienteDto dto)
        {
            var cliente = await _clienteDao.ObterPorIdAsync(dto.Id);
            if (cliente == null)
                return NotFound();

            // Atualiza manualmente as propriedades necessárias
            cliente.VendedorId = dto.VendedorId;
            cliente.CpfCnpj = dto.CpfCnpj;
            cliente.RazaoSocial = dto.RazaoSocial;
            cliente.NomeFantasia = dto.NomeFantasia;
            cliente.Contato = dto.Contato;
            cliente.TelefoneDdd = dto.TelefoneDdd;
            cliente.TelefoneNumero = dto.TelefoneNumero;
            cliente.DiaCobranca = dto.DiaCobranca;
            cliente.Status = dto.Status ?? cliente.Status;
            cliente.DadosSerasa = dto.DadosSerasa;

            await _context.SaveChangesAsync();

            return Ok();
        }

        // Endereços
        [HttpPost("{id}/endereco")]
        public async Task<IActionResult> AddEndereco(long id, [FromBody] EnderecoDto dto)
        {
            var cliente = await _clienteDao.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound();

            var endereco = new Endereco(id, dto.Cep, dto.CodigoIbge, dto.Bairro, dto.UF, dto.Numero, dto.Complemento, dto.Tipo);
            await _context.Set<Endereco>().AddAsync(endereco);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEnderecos), new { id }, dto);
        }

        [HttpPut("{id}/endereco")]
        public async Task<IActionResult> UpdateEndereco(long id, [FromBody] EnderecoDto dto)
        {
            var endereco = await _context.Set<Endereco>().FindAsync(dto.Id);
            if (endereco == null || endereco.ClienteId != id)
                return NotFound();

            endereco.Cep = dto.Cep;
            endereco.CodigoIbge = dto.CodigoIbge;
            endereco.Bairro = dto.Bairro;
            endereco.UF = dto.UF;
            endereco.Numero = dto.Numero;
            endereco.Complemento = dto.Complemento;
            endereco.Tipo = dto.Tipo;

            _context.Set<Endereco>().Update(endereco);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}/endereco")]
        public async Task<IActionResult> DeleteEndereco(long id, [FromQuery] long enderecoId)
        {
            var endereco = await _context.Set<Endereco>().FindAsync(enderecoId);
            if (endereco == null || endereco.ClienteId != id)
                return NotFound();

            _context.Set<Endereco>().Remove(endereco);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/endereco")]
        public async Task<IActionResult> GetEnderecos(long id)
        {
            var list = await _context.Set<Endereco>().Where(e => e.ClienteId == id).ToListAsync();
            var dtos = list.Select(e => new EnderecoDto
            {
                Id = e.Id,
                ClienteId = e.ClienteId,
                Cep = e.Cep,
                CodigoIbge = e.CodigoIbge,
                Bairro = e.Bairro,
                UF = e.UF,
                Numero = e.Numero,
                Complemento = e.Complemento,
                Tipo = e.Tipo
            }).ToList();

            return Ok(dtos);
        }

        // Observações
        [HttpPost("{id}/observacao")]
        public async Task<IActionResult> AddObservacao(long id, [FromBody] ObservacaoDto dto)
        {
            var obs = new Observacao("cliente", id, dto.Texto);
            await _observacaoDao.AdicionarAsync(obs);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetObservacoes), new { id }, dto);
        }

        [HttpPut("{id}/observacao")]
        public async Task<IActionResult> UpdateObservacao(long id, [FromBody] ObservacaoDto dto)
        {
            var observacao = await _observacaoDao.ObterPorIdAsync(dto.Id);
            if (observacao == null || observacao.EntidadeId != id)
                return NotFound();

            observacao.Texto = dto.Texto;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}/observacao")]
        public async Task<IActionResult> DeleteObservacao(long id, [FromQuery] long observacaoId)
        {
            var observacao = await _observacaoDao.ObterPorIdAsync(observacaoId);
            if (observacao == null || observacao.EntidadeId != id)
                return NotFound();

            await _observacaoDao.RemoverAsync(observacao);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/observacao")]
        public async Task<IActionResult> GetObservacoes(long id)
        {
            var list = await _context.Set<Observacao>().Where(o => o.Entidade == "cliente" && o.EntidadeId == id).ToListAsync();
            var dtos = list.Select(o => new ObservacaoDto
            {
                Id = o.Id,
                Entidade = o.Entidade,
                EntidadeId = o.EntidadeId,
                Texto = o.Texto
            }).ToList();

            return Ok(dtos);
        }

        // Serasa
        [HttpPut("{id}/serasa")]
        public async Task<IActionResult> UpdateSerasa(long id, [FromBody] ClienteSerasaDto dto)
        {
            var cliente = await _clienteDao.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound();

            cliente.DadosSerasa = dto.DadosSerasa;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}/aprovar")]
        public async Task<IActionResult> Aprovar(long id)
        {
            var cliente = await _clienteDao.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound();

            cliente.Status = "aprovado";
            cliente.DataAprovou = System.DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}/reprovar")]
        public async Task<IActionResult> Reprovar(long id)
        {
            var cliente = await _clienteDao.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound();

            cliente.Status = "reprovado";

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}/inativar")]
        public async Task<IActionResult> Inativar(long id)
        {
            var cliente = await _clienteDao.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound();

            cliente.Status = "inativo";

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}/resumo-compras")]
        public async Task<IActionResult> ResumoCompras(long id)
        {
            // Minimal implementation: aggregate vendas if table exists
            var vendas = await _context.Set<Venda>().Where(v => v.ClienteId == id).ToListAsync();
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
                DadosSerasa = c.DadosSerasa
            };
        }
    }
}
