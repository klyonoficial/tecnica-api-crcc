<template>
    <div>
        <div class="flex align-items-center justify-content-between mb-3">
            <h2 class="m-0">{{ isEdit ? 'Editar Produto' : 'Cadastro de Produto' }}</h2>            
        </div>
        <ProdutoForm :id="id" />
    </div>
</template>

<script>
import ProdutoForm from '@/components/produto/ProdutoForm.vue';

export default {
    components: {
        ProdutoForm
    },
    props: {
        id: {
            type: Number,
            default: null
        }
    },
    computed: {
        isEdit() {
            return this.id !== null;
        }
    }
}
</script>

<style scoped>
</style>
