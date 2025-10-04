<template>
    <div>
        <div class="flex align-items-center justify-content-between mb-3">
            <h2 class="m-0">{{ isEdit ? 'Editar Fabricante' : 'Cadastro de Fabricante' }}</h2>
        </div>
        <FabricanteForm :id="id" />
    </div>
</template>

<script>
import FabricanteForm from '@/components/fabricante/FabricanteForm.vue';

export default {
    components: {
        FabricanteForm
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
