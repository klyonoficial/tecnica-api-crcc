# Sistema de Tratamento de Erros

Este sistema foi criado para tratar automaticamente os erros retornados pelo backend, suportando dois formatos principais:

## Formato 1: Erro customizado do sistema
```json
{
    "mensagem": "O CEP deve ser informado."
}
```

## Formato 2: Erro de validação do ASP.NET Core
```json
{
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "errors": {
        "dto": [
            "The dto field is required."
        ],
        "$.dataCadastro": [
            "The JSON value could not be converted to System.DateTime. Path: $.dataCadastro | LineNumber: 0 | BytePositionInLine: 217."
        ]
    },
    "traceId": "00-e16514caa199e56e01bf62a25aa60c26-220b647d2b50c77a-00"
}
```

## Como usar

### 1. Em componentes Vue

```javascript
import { useErrorHandler } from '@/utils/errorHandler';

export default {
    setup() {
        const { handleError } = useErrorHandler();
        
        const saveData = async () => {
            try {
                await apiService.save(data);
                // sucesso
            } catch (error) {
                handleError(error); // Trata automaticamente o erro
            }
        };
        
        return { saveData };
    }
}
```

### 2. Tipos de erro tratados

- **Formato customizado** (`{"mensagem":"..."}): Exibe a mensagem diretamente
- **400 (Validação ASP.NET)**: Exibe erros de validação traduzidos
- **401 (Não autorizado)**: Mensagem de permissão
- **403 (Acesso negado)**: Mensagem de acesso
- **404 (Não encontrado)**: Recurso não encontrado
- **500 (Erro interno)**: Erro do servidor
- **Erro de rede**: Problemas de conexão

### 3. Traduções automáticas

O sistema traduz automaticamente:

- Campos: `dataCadastro` → `data de cadastro`
- Mensagens: `is required` → `é obrigatório`
- Erros de conversão: `The JSON value could not be converted to System.DateTime` → `formato de data inválido. Use o formato DD/MM/AAAA.`

### 4. Configuração do Axios

Todos os serviços agora usam a instância configurada do axios (`apiClient`) que:

- Define automaticamente a URL base
- Configura timeout de 10 segundos
- Adiciona headers padrão
- Permite interceptors para autenticação futura

## Exemplo de uso completo

```javascript
// Em um componente
import { useErrorHandler } from '@/utils/errorHandler';
import { useToast } from 'primevue/usetoast';
import ClienteService from '@/service/ClienteService';

export default {
    setup() {
        const toast = useToast();
        const { handleError } = useErrorHandler();
        const clienteService = new ClienteService();
        
        const saveCliente = async (cliente) => {
            try {
                await clienteService.saveCliente(cliente);
                toast.add({ 
                    severity: 'success', 
                    summary: 'Sucesso', 
                    detail: 'Cliente salvo com sucesso!' 
                });
            } catch (error) {
                // Automaticamente exibe erros de validação traduzidos
                handleError(error);
            }
        };
        
        return { saveCliente };
    }
}
```

## Benefícios

1. **Consistência**: Todos os erros são tratados da mesma forma
2. **Suporte múltiplos formatos**: Trata tanto erros customizados quanto do ASP.NET Core
3. **Tradução**: Mensagens em português para melhor UX (quando necessário)
4. **Automático**: Não precisa tratar cada tipo de erro manualmente
5. **Flexível**: Permite customização quando necessário
6. **Centralizado**: Fácil manutenção e atualização

## Exemplos de tratamento

### Erro customizado
```json
// Backend retorna:
{"mensagem": "O CEP deve ser informado."}

// Frontend exibe:
Toast com: "Erro de Validação: O CEP deve ser informado."
```

### Erro ASP.NET Core
```json
// Backend retorna:
{"errors": {"$.dataCadastro": ["The JSON value could not be converted to System.DateTime..."]}}

// Frontend exibe:
Toast com: "Erro de Validação: data de cadastro: formato de data inválido. Use o formato DD/MM/AAAA."
```
