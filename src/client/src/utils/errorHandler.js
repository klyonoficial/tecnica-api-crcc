import { useToast } from 'primevue/usetoast';

/**
 * Utilitário para tratamento de erros de API
 */
export class ErrorHandler {
    constructor(toast) {
        this.toast = toast;
    }

    /**
     * Trata erros de validação do backend
     * @param {Object} error - Objeto de erro do axios
     */
    handleValidationError(error) {
        if (!error.response || !error.response.data) {
            this.showGenericError();
            return;
        }

        const errorData = error.response.data;
        
        // Verifica se é um erro no formato customizado {"mensagem":"..."}
        if (errorData.mensagem) {
            this.toast.add({
                severity: 'error',
                summary: 'Erro de Validação',
                detail: errorData.mensagem,
                life: 4000
            });
            return;
        }
        
        // Verifica se é um erro de validação do ASP.NET Core (status 400 com errors)
        if (error.response.status === 400 && errorData.errors) {
            this.showValidationErrors(errorData.errors);
        } else {
            this.showGenericError(errorData.title || errorData.detail);
        }
    }

    /**
     * Exibe erros de validação específicos
     * @param {Object} errors - Objeto com os erros de validação
     */
    showValidationErrors(errors) {
        const errorMessages = [];

        // Processa cada campo com erro
        Object.keys(errors).forEach(field => {
            const fieldErrors = errors[field];
            
            if (Array.isArray(fieldErrors)) {
                fieldErrors.forEach(errorMessage => {
                    // Traduz mensagens comuns de erro
                    const translatedMessage = this.translateErrorMessage(field, errorMessage);
                    errorMessages.push(translatedMessage);
                });
            }
        });

        // Exibe todos os erros em um toast
        if (errorMessages.length > 0) {
            const detail = errorMessages.length === 1 
                ? errorMessages[0]
                : `Foram encontrados os seguintes erros:\n• ${errorMessages.join('\n• ')}`;

            this.toast.add({
                severity: 'error',
                summary: 'Erro de Validação',
                detail: detail,
                life: 5000
            });
        }
    }

    /**
     * Traduz mensagens de erro para português
     * @param {string} field - Nome do campo
     * @param {string} message - Mensagem original do erro
     * @returns {string} - Mensagem traduzida
     */
    translateErrorMessage(field, message) {
        // Tradução de campos comuns
        const fieldTranslations = {
            'dto': 'dados',
            'dataCadastro': 'data de cadastro',
            'nome': 'nome',
            'email': 'e-mail',
            'telefone': 'telefone',
            'cpf': 'CPF',
            'cnpj': 'CNPJ',
            'endereco': 'endereço',
            'cep': 'CEP'
        };

        // Tradução de mensagens comuns
        const messageTranslations = {
            'is required': 'é obrigatório',
            'field is required': 'é obrigatório',
            'The JSON value could not be converted': 'formato inválido',
            'Invalid format': 'formato inválido',
            'Invalid email': 'e-mail inválido',
            'Invalid phone': 'telefone inválido',
            'Invalid CPF': 'CPF inválido',
            'Invalid CNPJ': 'CNPJ inválido'
        };

        // Traduz o nome do campo
        const translatedField = fieldTranslations[field] || field;
        
        // Traduz a mensagem
        let translatedMessage = message;
        Object.keys(messageTranslations).forEach(key => {
            if (message.includes(key)) {
                translatedMessage = message.replace(key, messageTranslations[key]);
            }
        });

        // Casos especiais para conversão de dados
        if (message.includes('The JSON value could not be converted to System.DateTime')) {
            return `${translatedField}: formato de data inválido. Use o formato DD/MM/AAAA.`;
        }

        if (message.includes('The JSON value could not be converted')) {
            return `${translatedField}: formato de dados inválido.`;
        }

        // Se o campo não está na mensagem, adiciona
        if (!translatedMessage.toLowerCase().includes(translatedField.toLowerCase())) {
            return `${translatedField}: ${translatedMessage}`;
        }

        return translatedMessage;
    }

    /**
     * Exibe erro genérico
     * @param {string} customMessage - Mensagem personalizada (opcional)
     */
    showGenericError(customMessage = null) {
        this.toast.add({
            severity: 'error',
            summary: 'Erro',
            detail: customMessage || 'Ocorreu um erro inesperado. Tente novamente.',
            life: 3000
        });
    }

    /**
     * Trata diferentes tipos de erro HTTP
     * @param {Object} error - Objeto de erro do axios
     */
    handleError(error) {
        if (!error.response) {
            // Erro de rede
            this.toast.add({
                severity: 'error',
                summary: 'Erro de Conexão',
                detail: 'Não foi possível conectar ao servidor. Verifique sua conexão.',
                life: 3000
            });
            return;
        }

        const status = error.response.status;
        
        // Primeiro verifica se é um erro no formato customizado {"mensagem":"..."}
        const errorData = error.response.data;
        if (errorData && errorData.mensagem) {
            this.toast.add({
                severity: 'error',
                summary: 'Erro',
                detail: errorData.mensagem,
                life: 4000
            });
            return;
        }

        switch (status) {
            case 400:
                this.handleValidationError(error);
                break;
            case 401:
                this.toast.add({
                    severity: 'error',
                    summary: 'Não Autorizado',
                    detail: 'Você não tem permissão para realizar esta ação.',
                    life: 3000
                });
                break;
            case 403:
                this.toast.add({
                    severity: 'error',
                    summary: 'Acesso Negado',
                    detail: 'Você não tem permissão para acessar este recurso.',
                    life: 3000
                });
                break;
            case 404:
                this.toast.add({
                    severity: 'error',
                    summary: 'Não Encontrado',
                    detail: 'O recurso solicitado não foi encontrado.',
                    life: 3000
                });
                break;
            case 500:
                this.toast.add({
                    severity: 'error',
                    summary: 'Erro Interno',
                    detail: 'Ocorreu um erro interno no servidor. Tente novamente mais tarde.',
                    life: 3000
                });
                break;
            default:
                this.showGenericError();
        }
    }
}

/**
 * Composable para usar o ErrorHandler
 * @returns {Object} - Objeto com métodos para tratamento de erro
 */
export function useErrorHandler() {
    const toast = useToast();
    const errorHandler = new ErrorHandler(toast);

    return {
        handleError: (error) => errorHandler.handleError(error),
        handleValidationError: (error) => errorHandler.handleValidationError(error),
        showGenericError: (message) => errorHandler.showGenericError(message)
    };
}

export default ErrorHandler;
