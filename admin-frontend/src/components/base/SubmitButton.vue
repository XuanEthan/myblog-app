<script setup>
import { defineProps, defineEmits, reactive } from 'vue';
const props = defineProps({
    name: {
        type: String,
        default: 'Submit'
    },
    position: {
        type: String,
        default: 'absolute'
    }
})

</script>
<template>
    <button type="submit" :style="{ position: position }">{{ name }}</button>
</template>
<style scoped>
button[type='submit'] {
    display: block;
    top: 12px;
    right: 0px;
    background-color: #3858e9;
    color: white;
    padding: 8px;
    border: none;
    cursor: pointer;
    width: auto;
    font-size: 13px;
}
</style>