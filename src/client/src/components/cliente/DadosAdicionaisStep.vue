<template>
    <div class="p-fluid">
        <h4>Dados Adicionais</h4>
        <p class="text-color-secondary mb-4">
            Estas informações são preenchidas automaticamente pelo sistema.
        </p>
        
        <div class="grid">
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="font-bold">Data de Cadastro</label>
                    <div class="p-inputtext p-component p-disabled">
                        {{ formatDate(form.dataCadastro) }}
                    </div>
                </div>
            </div>
            
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="font-bold">Data de Aprovação</label>
                    <div class="p-inputtext p-component p-disabled">
                        {{ form.dataAprovou ? formatDate(form.dataAprovou) : 'Não aprovado' }}
                    </div>
                </div>
            </div>
            
            <div class="col-12 md:col-4">
                <div class="field">
                    <label class="font-bold">Vendedor</label>
                    <Dropdown 
                        v-model="form.vendedorId" 
                        :options="usuarios" 
                        optionLabel="nome" 
                        optionValue="id" 
                        placeholder="Selecione um vendedor"
                        :loading="loadingUsuarios"
                        class="w-full" />
                </div>
            </div>
        </div>
        
        <div class="grid mt-4">
            <div class="col-12 md:col-6">
                <div class="field">
                    <label class="font-bold">Status</label>
                    <div class="p-inputtext p-component p-disabled">
                        {{ form.status || 'Ativo' }}
                    </div>
                </div>
            </div>
            
            <div class="col-12 md:col-6">
                <div class="field">
                    <label class="font-bold">Última Atualização</label>
                    <div class="p-inputtext p-component p-disabled">
                        {{ form.dataUltimaAtualizacao ? formatDate(form.dataUltimaAtualizacao) : 'Não atualizado' }}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { ref, onMounted, watch } from 'vue';
import Dropdown from 'primevue/dropdown';
import UsuarioService from '@/service/UsuarioService';

export default {
    components: {
        Dropdown
    },
    props: {
        cliente: {
            type: Object,
            required: true
        }
    },
    emits: ['update:valid'],
    setup(props, { emit }) {
        const usuarioService = new UsuarioService();
        const usuarios = ref([]);
        const loadingUsuarios = ref(false);
        
        const form = ref({
            dataCadastro: props.cliente.dataCadastro || new Date(),
            dataAprovou: props.cliente.dataAprovou || null,
            vendedorId: props.cliente.vendedorId || null,
            status: props.cliente.status || 'Ativo',
            dataUltimaAtualizacao: props.cliente.dataUltimaAtualizacao || null
        });

        // Observa mudanças no prop cliente e atualiza o form
        watch(() => props.cliente, (newCliente) => {
            form.value.dataCadastro = newCliente.dataCadastro || new Date();
            form.value.dataAprovou = newCliente.dataAprovou || null;
            form.value.vendedorId = newCliente.vendedorId || null;
            form.value.status = newCliente.status || 'Ativo';
            form.value.dataUltimaAtualizacao = newCliente.dataUltimaAtualizacao || null;
        }, { deep: true });

        // Formata a data para exibição
        const formatDate = (date) => {
            if (!date) return '';
            
            if (typeof date === 'string') {
                date = new Date(date);
            }
            
            return new Intl.DateTimeFormat('pt-BR', {
                day: '2-digit',
                month: '2-digit',
                year: 'numeric',
                hour: '2-digit',
                minute: '2-digit'
            }).format(date);
        };

        // Carrega a lista de usuários
        const loadUsuarios = async () => {
            try {
                loadingUsuarios.value = true;
                const data = await usuarioService.getUsuarios();
                usuarios.value = data.filter(usuario => usuario.ativo); // Apenas usuários ativos
            } catch (error) {
                console.error('Erro ao carregar usuários:', error);
            } finally {
                loadingUsuarios.value = false;
            }
        };

        // Atualiza os dados do cliente quando o vendedor muda
        watch(() => form.value.vendedorId, (newVendedorId) => {
            props.cliente.vendedorId = newVendedorId;
        });

        // Este passo sempre é válido
        onMounted(async () => {
            await loadUsuarios();
     
            emit('update:valid', true);
        });

        return {
            form,
            usuarios,
            loadingUsuarios,
            formatDate
        };
    }
}
</script>

<style scoped>
.p-disabled {
    background-color: var(--surface-200);
    color: var(--text-color-secondary);
    padding: 0.75rem;
    border-radius: 6px;
    border: 1px solid var(--surface-border);
}
</style>
