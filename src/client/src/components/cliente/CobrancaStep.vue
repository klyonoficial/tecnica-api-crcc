<template>
    <div class="p-fluid">
        
        <div class="field">
            <label for="diaCobranca" class="font-bold">Dia de Cobrança*</label>
            <Dropdown id="diaCobranca" v-model="form.diaCobranca" 
                :options="diasSemana" optionLabel="label" optionValue="value" 
                placeholder="Selecione o dia da semana" class="w-full" />
            <small v-if="v$.diaCobranca.$invalid && v$.diaCobranca.$dirty" class="p-error">
                {{ v$.diaCobranca.$errors[0].$message }}
            </small>
        </div>
    </div>
</template>

<script>
import { ref, watch } from 'vue';
import { useVuelidate } from '@vuelidate/core';
import { required, helpers, between } from '@vuelidate/validators';
import Dropdown from 'primevue/dropdown';

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
        const form = ref({
            diaCobranca: props.cliente.diaCobranca || 1
        });

        // Observa mudanças no prop cliente e atualiza o form
        watch(() => props.cliente, (newCliente) => {
            form.value.diaCobranca = newCliente.diaCobranca || 1;
        }, { deep: true });

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

        // Atualiza os dados do cliente quando o formulário muda
        watch(form, () => {
            props.cliente.diaCobranca = form.value.diaCobranca;
            
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
            diasSemana
        };
    }
}
</script>

<style scoped>
/* Estilos específicos para o componente de cobrança */
</style>