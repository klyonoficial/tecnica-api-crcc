<template>
    <div class="layout-content">
        <div class="grid">
            <div class="col-12">
                <ClienteList />
            </div>
        </div>
    </div>
</template>

<script>
import ClienteList from '@/components/cliente/ClienteList.vue';

export default {
    name: 'ClientesListPage',
    components: {
        ClienteList
    }
};
</script>

