# Visão Geral

Este projeto é desenvolvido em C# utilizando o Entity Framework para acesso a dados e o ASP.NET Core como framework principal para construção de APIs. O objetivo é garantir uma arquitetura limpa, segura e de fácil manutenção, seguindo boas práticas de encapsulamento, injeção de dependências e separação de responsabilidades.

# Regras de Codigo
- As propriedades dos objetos devem ser alteradas apenas através de métodos públicos específicos (não use setters). Não modifique propriedades diretamente fora da classe.

Exemplo (correto):
class ContaBancaria {
    private saldo: number;

    constructor(saldoInicial: number) {
        saldo = saldoInicial;
    }

    public depositar(valor: number): void {
        saldo += valor;
    }

    public sacar(valor: number): void {
        if (valor <= saldo) {
            saldo -= valor;
        }
    }

    public obterSaldo(): number {
        return saldo;
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

# Regras de Uso de Datas

- Sempre utilize `DateTime.Now` para obter a data e hora atual do sistema. Não utilize outras formas como `DateTime.UtcNow`, `DateTime.Today` ou instâncias manuais de `DateTime` para representar a data e hora corrente, exceto em casos específicos de necessidade de fuso horário ou testes.

Exemplo (correto):
var dataAtual = DateTime.Now;

Exemplo (incorreto):
var dataAtual = DateTime.UtcNow;
var dataAtual = DateTime.Today;
var dataAtual = new DateTime(2024, 6, 1);


# Regras de Dependência entre Camadas

- Classes do domínio (pasta `Domain`) **não podem** referenciar ou depender de classes de DTO (pasta `Dto`). O domínio deve ser independente de detalhes de transporte ou apresentação, garantindo isolamento e reutilização das regras de negócio.

Exemplo (correto):
// Em Domain
public class Pedido
{
    // propriedades e métodos de negócio
}

// Em Dto
public class PedidoDto
{
    // propriedades para transporte de dados
}

Exemplo (incorreto):
// Em Domain
public class Pedido
{
    public PedidoDto ToDto() { ... } // Não permitido!
}

# Regras de Uso de Fully Qualified Names (FQN)

- Não utilize nomes totalmente qualificados (FQN) ao referenciar classes dentro do código. Sempre prefira adicionar a diretiva `using` no início do arquivo para importar o namespace necessário e utilize apenas o nome da classe no restante do código.

Exemplo (correto):
using MeuProjeto.Domain;

public class Exemplo
{
    public void Metodo()
    {
        var pedido = new Pedido();
    }
}

Exemplo (incorreto):
public class Exemplo
{
    public void Metodo()
    {
        var pedido = new MeuProjeto.Domain.Pedido(); // Não utilize FQN
    }
}