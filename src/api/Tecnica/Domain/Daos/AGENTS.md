- O formato de Classes DAO é o seguinte abaixo
tendo como exemplo a classe de Dominio Usuario.
- A classe DaoRegister deve receber uma chamada:
    services.AddScoped<UsuarioDao>();

- A classe ApplicationDBContext deve receber uma chamada:
    public DbSet<Usuario> Usuarios { get; set; }

- Daos ficam na pasta api/Tecnica/Domain/Daos

```
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

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

        public async Task AtualizarAsync(Usuario usuario)
        {
            _context.Set<Usuario>().Update(usuario);
        }

        public async Task RemoverAsync(Usuario usuario)
        {
            _context.Set<Usuario>().Remove(usuario);
        }
    }
}

```