<template>
    <div class="p-fluid">
        <h4>Dados Serasa</h4>
        <p class="text-color-secondary mb-4">
            Informações relacionadas ao Serasa do cliente.
        </p>
        
        <div class="field">
            <label for="dadosSerasa" class="font-bold">Dados Serasa</label>
            <Textarea 
                id="dadosSerasa" 
                v-model="form.dadosSerasa" 
                rows="8" 
                placeholder="Digite as informações do Serasa..." 
                class="w-full" />
        </div>
    </div>
</template>

<script>
import { ref, watch } from 'vue';
import Textarea from 'primevue/textarea';

export default {
    components: {
        Textarea
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
            dadosSerasa: props.cliente.dadosSerasa || ''
        });

        // Observa mudanças no prop cliente e atualiza o form
        watch(() => props.cliente, (newCliente) => {
            form.value.dadosSerasa = newCliente.dadosSerasa || '';
        }, { deep: true });

        // Atualiza os dados do cliente quando o formulário muda
        watch(form, () => {
            props.cliente.dadosSerasa = form.value.dadosSerasa;
            
            // Este passo sempre é válido
            emit('update:valid', true);
        }, { deep: true });

        // Emite válido inicialmente
        emit('update:valid', true);

        return {
            form
        };
    }
}
</script>

<style scoped>
/* Estilos específicos para o componente de dados Serasa */
</style>
