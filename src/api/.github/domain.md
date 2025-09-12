- O formato das classe de dominio é o seguinte abaixo.
- Utiliza Entity Framework Core como Orm.
- A classe Assegure e Validation devem ser usado para validacoes.
- As classe domain ficam na pasta  src\api\Tecnica\Domain\Entities .

'''
using Tecnica.Infra;

package Tecnica.Domain;

[Table("pessoa")]
public class Pessoa 
{
    [Key]
    public long Id {get; set}
    
    [Column("nome")]
    public string Nome {get; set}

    public Pessoa(sting theNome) 
    {
        Assegure.NaoNulo(theNome, "O nome deve ser informado.");

        Nome = theNome;
    }
}

'''