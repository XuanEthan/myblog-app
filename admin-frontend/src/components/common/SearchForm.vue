<script setup>
import { defineProps, defineEmits } from 'vue'

const props = defineProps({
    placeholder: {
        type: String,
        default: 'Type to search...'
    },
    searchKey: {
        type: String,
        default: ''
    }
})
const emits = defineEmits(['update:searchKey', 'submit'])

function handleInput(event) {
    emits('update:searchKey', event.target.value)
}

function handleSubmit() {
    emits('submit')
}
</script>

<template>
    <form @submit.prevent="handleSubmit" class="search-form">
        <input type="text" :placeholder="placeholder" @input="handleInput" />
        <input type="submit" value="Search" />
    </form>
</template>

<style scoped>
form.search-form {
    display: flex;
    column-gap: 6px;
    justify-content: end;
}

form.search-form input {
    padding: 6px;
    font-size: 13px;
}

form.search-form input[type='submit'] {
    border: 1px solid #3858e9;
    color: #3858e9;
}
</style>
