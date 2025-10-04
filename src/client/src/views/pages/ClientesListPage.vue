<template>
    <div>
        <ClienteList />
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