export class Cliente {
    constructor() {
        this.id = 0;
        this.vendedorId = 0;
        this.cpfCnpj = '';
        this.razaoSocial = '';
        this.nomeFantasia = '';
        this.contato = '';
        this.telefoneDdd = '';
        this.telefoneNumero = '';
        this.diaCobranca = 1;
        this.status = 'Ativo';
        this.dataAprovou = null;
        this.dataCadastro = null;
        this.dadosSerasa = '';
        this.enderecos = [];
        this.arquivos = [];
    }
}

export class Endereco {
    constructor() {
        this.id = 0;
        this.cep = '';
        this.logradouro = '';
        this.numero = '';
        this.complemento = '';
        this.bairro = '';
        this.cidade = '';
        this.uf = '';
        this.principal = false;
    }
}

export class Arquivo {
    constructor() {
        this.id = 0;
        this.nome = '';
        this.tipo = '';
        this.conteudo = '';
    }
}