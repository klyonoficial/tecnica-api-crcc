<template>
    <div class="p-fluid">
        <h4>Dados de Cobrança</h4>
        
        <div class="field grid">
            <div class="col-12 md:col-4">
                <label for="diaCobranca" class="font-bold">Dia de Cobrança*</label>
                <Dropdown id="diaCobranca" v-model="form.diaCobranca" 
                    :options="diasSemana" optionLabel="label" optionValue="value" 
                    placeholder="Selecione o dia da semana" class="w-full" />
                <small v-if="v$.diaCobranca.$invalid && v$.diaCobranca.$dirty" class="p-error">
                    {{ v$.diaCobranca.$errors[0].$message }}
                </small>
            </div>
            
            <div class="col-12 md:col-8">
                <label for="dadosSerasa" class="font-bold">Dados Serasa</label>
                <InputText id="dadosSerasa" v-model="form.dadosSerasa" placeholder="Informações do Serasa (opcional)" />
            </div>
        </div>
        
        <div class="field">
            <label for="observacoes" class="font-bold">Observações de Cobrança</label>
            <Textarea id="observacoes" v-model="form.observacoes" rows="5" placeholder="Informações adicionais para cobrança..." />
        </div>
        
        <Divider />
        
        <div class="field">
            <h5>Dados Adicionais</h5>
            <p class="text-color-secondary">
                Estas informações são preenchidas automaticamente pelo sistema.
            </p>
            
            <div class="grid">
                <div class="col-12 md:col-4">
                    <label class="font-bold">Data de Cadastro</label>
                    <div class="p-inputtext p-component p-disabled">
                        {{ formatDate(form.dataCadastro) }}
                    </div>
                </div>
                
                <div class="col-12 md:col-4">
                    <label class="font-bold">Data de Aprovação</label>
                    <div class="p-inputtext p-component p-disabled">
                        {{ form.dataAprovou ? formatDate(form.dataAprovou) : 'Não aprovado' }}
                    </div>
                </div>
                
                <div class="col-12 md:col-4">
                    <label class="font-bold">ID do Vendedor</label>
                    <div class="p-inputtext p-component p-disabled">
                        {{ form.vendedorId || 'Não atribuído' }}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { ref, watch } from 'vue';
import { useVuelidate } from '@vuelidate/core';
import { required, helpers, between } from '@vuelidate/validators';
import Dropdown from 'primevue/dropdown';
import Textarea from 'primevue/textarea';
import Divider from 'primevue/divider';

export default {
    components: {
        Dropdown,
        Textarea,
        Divider
    },
    props: {
        cliente: {
            type: Object,
            required: true
        }
    },
    emits: ['update:valid'],
    setup(props, { emit }) {
        const form = ref({
            diaCobranca: props.cliente.diaCobranca || 1,
            dadosSerasa: props.cliente.dadosSerasa || '',
            dataCadastro: props.cliente.dataCadastro || new Date(),
            dataAprovou: props.cliente.dataAprovou || null,
            vendedorId: props.cliente.vendedorId || 0,
            observacoes: ''
        });

        // Dias da semana (1 = Domingo, 7 = Sábado)
        const diasSemana = [
            { label: 'Domingo', value: 1 },
            { label: 'Segunda-feira', value: 2 },
            { label: 'Terça-feira', value: 3 },
            { label: 'Quarta-feira', value: 4 },
            { label: 'Quinta-feira', value: 5 },
            { label: 'Sexta-feira', value: 6 },
            { label: 'Sábado', value: 7 }
        ];

        // Validações
        const rules = {
            diaCobranca: { 
                required: helpers.withMessage('Dia de cobrança é obrigatório', required),
                between: helpers.withMessage('Dia de cobrança deve estar entre 1 e 7', between(1, 7))
            }
        };

        const v$ = useVuelidate(rules, form);

        // Formata a data para exibição
        const formatDate = (date) => {
            if (!date) return '';
            
            if (typeof date === 'string') {
                date = new Date(date);
            }
            
            return new Intl.DateTimeFormat('pt-BR', {
                day: '2-digit',
                month: '2-digit',
                year: 'numeric'
            }).format(date);
        };

        // Atualiza os dados do cliente quando o formulário muda
        watch(form, () => {
            props.cliente.diaCobranca = form.value.diaCobranca;
            props.cliente.dadosSerasa = form.value.dadosSerasa;
            
            // Valida o formulário e emite o evento
            v$.value.$validate();
            emit('update:valid', !v$.value.$invalid);
        }, { deep: true });

        // Valida o formulário inicialmente
        v$.value.$validate();
        emit('update:valid', !v$.value.$invalid);

        return {
            form,
            v$,
            formatDate,
            diasSemana
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