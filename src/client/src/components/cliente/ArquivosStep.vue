<template>
    <div class="p-fluid">
        <div class="flex justify-content-between align-items-center mb-3">
            <h4>Documentos</h4>
            <Button type="button" icon="pi pi-upload" label="Adicionar Arquivo" @click="$refs.fileUpload.choose()" />
        </div>

        <FileUpload ref="fileUpload" mode="basic" :multiple="true" :auto="true" 
            :maxFileSize="10000000" accept="image/*,application/pdf" 
            :customUpload="true" @uploader="uploadFiles" class="hidden" />

        <div v-if="arquivos.length === 0" class="empty-message p-4 text-center">
            <i class="pi pi-file text-5xl mb-3 text-color-secondary"></i>
            <p>Nenhum documento adicionado. Clique em "Adicionar Arquivo" para enviar documentos.</p>
        </div>

        <div v-else class="grid">
            <div v-for="(arquivo, index) in arquivos" :key="index" class="col-12 md:col-6 lg:col-4">
                <div class="arquivo-card p-3 border-round surface-card mb-3">
                    <div class="flex align-items-center mb-3">
                        <i :class="getIconClass(arquivo.tipo)" class="text-2xl mr-2"></i>
                        <div class="flex-1">
                            <div class="font-bold text-overflow-ellipsis" :title="arquivo.nome">
                                {{ arquivo.nome }}
                            </div>
                            <div class="text-sm text-color-secondary">
                                {{ formatFileSize(arquivo.conteudo.length) }}
                            </div>
                        </div>
                        <Button type="button" icon="pi pi-trash" class="p-button-rounded p-button-text p-button-danger" 
                            @click="removerArquivo(index)" />
                    </div>
                    
                    <div class="border-1 border-dashed surface-border p-2 border-round">
                        <InputText v-model="arquivo.descricao" placeholder="Descrição do documento" class="w-full mb-2 p-inputtext-sm" />
                        <Dropdown v-model="arquivo.tipo" :options="tiposDocumento" optionLabel="label" optionValue="value" 
                            placeholder="Tipo do documento" class="w-full p-inputtext-sm" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { ref, watch } from 'vue';
import FileUpload from 'primevue/fileupload';
import Dropdown from 'primevue/dropdown';

export default {
    components: {
        FileUpload,
        Dropdown
    },
    props: {
        arquivos: {
            type: Array,
            required: true
        }
    },
    emits: ['update:arquivos', 'update:valid'],
    setup(props, { emit }) {
        const tiposDocumento = [
            { label: 'RG', value: 'RG' },
            { label: 'CPF', value: 'CPF' },
            { label: 'CNPJ', value: 'CNPJ' },
            { label: 'Comprovante de Residência', value: 'COMP_RESIDENCIA' },
            { label: 'Contrato', value: 'CONTRATO' },
            { label: 'Outros', value: 'OUTROS' }
        ];

        // Método para carregar arquivos
        const uploadFiles = async (event) => {
            const files = event.files;
            
            for (let file of files) {
                try {
                    const base64 = await readFileAsBase64(file);
                    
                    props.arquivos.push({
                        id: 0,
                        nome: file.name,
                        tipo: 'OUTROS', // tipo padrão
                        conteudo: base64,
                        descricao: ''
                    });
                } catch (error) {
                    console.error('Erro ao processar arquivo:', error);
                }
            }
            
            validateFiles();
        };

        // Converte arquivo para Base64
        const readFileAsBase64 = (file) => {
            return new Promise((resolve, reject) => {
                const reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = () => resolve(reader.result.split(',')[1]); // Remove o prefixo data:image/...
                reader.onerror = error => reject(error);
            });
        };

        // Remove um arquivo da lista
        const removerArquivo = (index) => {
            props.arquivos.splice(index, 1);
            validateFiles();
        };

        // Valida os arquivos (sempre retorna true, pois os arquivos são opcionais)
        const validateFiles = () => {
            emit('update:valid', true);
            return true;
        };

        // Formata o tamanho do arquivo
        const formatFileSize = (bytes) => {
            if (!bytes) return '0 B';
            
            const k = 1024;
            const sizes = ['B', 'KB', 'MB', 'GB'];
            const i = Math.floor(Math.log(bytes) / Math.log(k));
            
            return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
        };

        // Retorna o ícone baseado no tipo de arquivo
        const getIconClass = (tipo) => {
            const iconMap = {
                'RG': 'pi pi-id-card',
                'CPF': 'pi pi-id-card',
                'CNPJ': 'pi pi-building',
                'COMP_RESIDENCIA': 'pi pi-home',
                'CONTRATO': 'pi pi-file-pdf',
                'OUTROS': 'pi pi-file'
            };
            
            return iconMap[tipo] || 'pi pi-file';
        };

        // Observa mudanças nos arquivos
        watch(() => [...props.arquivos], () => {
            validateFiles();
        }, { deep: true });

        // Validação inicial
        validateFiles();

        return {
            tiposDocumento,
            uploadFiles,
            removerArquivo,
            formatFileSize,
            getIconClass
        };
    }
}
</script>

<style scoped>
.empty-message {
    border: 1px dashed var(--surface-border);
    border-radius: 8px;
    color: var(--text-color-secondary);
}

.arquivo-card {
    box-shadow: 0 2px 1px -1px rgba(0, 0, 0, 0.2), 0 1px 1px 0 rgba(0, 0, 0, 0.14), 0 1px 3px 0 rgba(0, 0, 0, 0.12);
    transition: box-shadow 0.3s, transform 0.3s;
}

.arquivo-card:hover {
    transform: translateY(-2px);
    box-shadow: 0 4px 5px -2px rgba(0, 0, 0, 0.2), 0 7px 10px 1px rgba(0, 0, 0, 0.14), 0 2px 16px 1px rgba(0, 0, 0, 0.12);
}

.text-overflow-ellipsis {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    max-width: 200px;
}

.hidden {
    display: none;
}
</style>