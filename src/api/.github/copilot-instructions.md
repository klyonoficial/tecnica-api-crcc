# Visão Geral

Este projeto é desenvolvido em C# utilizando o Entity Framework para acesso a dados e o ASP.NET Core como framework principal para construção de APIs. O objetivo é garantir uma arquitetura limpa, segura e de fácil manutenção, seguindo boas práticas de encapsulamento, injeção de dependências e separação de responsabilidades.

# Regras de Codigo
- As propriedades dos objetos devem ser alteradas apenas através de métodos públicos específicos (não use setters). Não modifique propriedades diretamente fora da classe.

Exemplo (correto):
class ContaBancaria {
    private saldo: number;

    constructor(saldoInicial: number) {
        this.saldo = saldoInicial;
    }

    public depositar(valor: number): void {
        this.saldo += valor;
    }

    public sacar(valor: number): void {
        if (valor <= this.saldo) {
            this.saldo -= valor;
        }
    }

    public obterSaldo(): number {
        return this.saldo;
    }
}

# Regras de Uso do `this`
- Evite o uso do `this` em métodos e funções. Sempre que possível, prefira passar dependências explicitamente como parâmetros ou utilizar funções estáticas/puras. Isso facilita a testabilidade e manutenção do código.

Exemplo (correto):
public static class CalculadoraSaldo
{
    public static decimal CalcularSaldo(decimal saldo, decimal valor)
    {
        return saldo + valor;
    }
}

Exemplo (incorreto):
public class Conta
{
    private decimal saldo = 0;

    public void Adicionar(decimal valor)
    {
        this.saldo += valor; // Evite usar `this`
    }
}

# Regras de Acesso a Dados
- Todas as queries ao banco de dados devem ser realizadas exclusivamente através de DAOs (Data Access Objects). Nunca utilize o DbContext diretamente em controllers ou serviços de aplicação.

Exemplo (correto):
public class UsuarioDao
{
    private readonly DbContext _dbContext;

    public UsuarioDao(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Usuario?> BuscarPorIdAsync(int id)
    {
        return await _dbContext.Set<Usuario>().FindAsync(id);
    }
}

// Controller utilizando o DAO
public class UsuarioController : ControllerBase
{
    private readonly UsuarioDao _usuarioDao;

    public UsuarioController(UsuarioDao usuarioDao)
    {
        _usuarioDao = usuarioDao;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterUsuario(int id)
    {
        var usuario = await _usuarioDao.BuscarPorIdAsync(id);
        if (usuario == null)
            return NotFound();
        return Ok(usuario);
    }
}