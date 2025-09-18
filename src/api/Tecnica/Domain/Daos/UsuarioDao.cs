using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;
using Tecnica.Dtos;

namespace Tecnica.Domain.Daos
{
    public class UsuarioDao
    {
        private readonly ApplicationDbContext _context;

        public UsuarioDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> ObterPorIdAsync(long id)
        {
            return await _context.Set<Usuario>().FindAsync(id);
        }

        public async Task<List<Usuario>> ListarTodosAsync()
        {
            return await _context.Set<Usuario>().ToListAsync();
        }

        public async Task AdicionarAsync(Usuario usuario)
        {
            await _context.Set<Usuario>().AddAsync(usuario);
        }

        public async Task RemoverAsync(Usuario usuario)
        {
            _context.Set<Usuario>().Remove(usuario);
        }

        public async Task<List<PermissaoDto>> ObterPermissoesAsync(long usuarioId)
        {
            var permissoesUsuario = await _context.Set<Permissao>()
                .Where(p => p.UsuarioId == usuarioId)
                .Select(p => p.ComponenteId)
                .ToListAsync();

            var componentes = await _context.Set<Componente>()
                .Where(c => c.Ativo)
                .ToListAsync();

            var permissoesDto = componentes.Select(c => new PermissaoDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Ordem = c.Ordem,
                Descricao = c.Descricao,
                Tipo = c.Tipo,
                Codigo = c.Codigo,
                PossuiPermissao = permissoesUsuario.Contains(c.Id)
            }).ToList();

            return permissoesDto;
        }
    }
}
