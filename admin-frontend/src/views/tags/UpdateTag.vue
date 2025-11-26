<script setup>
import { onMounted, reactive } from 'vue'
import { useTagStore } from '@/stores/tagStore'
import { useToast } from 'vue-toastification'
import router from '@/router'
import { useRoute } from 'vue-router'

const toast = useToast()
const tagStore = useTagStore()
const route = useRoute()

const tag = reactive({
    id: '',
    name: '',
    description: ''
})

const ErrorInputMessages = reactive({
    Name: [],
    Description: []
})

onMounted(async () => {
    const id = route.params.id
    try {
        const resp = await tagStore.getById(id)
        Object.assign(tag, resp)
    } catch (e) {
        router.push("/NotFound")
    }
})
async function handleUpdate() {
    try {
        await tagStore.updateTag(tag)
        toast.success("Updated tag successfully")
        router.push("/Tags")
    } catch (e) {
        if (e.response?.data?.errors)
            Object.assign(ErrorInputMessages, e.response.data.errors)
    }
}
</script>
<template>
    <div class="header">
        <h1>Edit Tag</h1>
    </div>
    <form action="" class="update-tag-form" @submit.prevent="handleUpdate">
        <div class="group-control">
            <label for="">Name</label>
            <input type="text" v-model="tag.name" name="" id=""><br>
            <i v-if="ErrorInputMessages.Name.length > 0" class="error-msg">{{ ErrorInputMessages.Name[0] }}</i>
        </div>
        <div class="group-control">
            <label for="">Description</label>
            <textarea v-model="tag.description" name="" id=""></textarea><br>
            <i v-if="ErrorInputMessages.Description.length > 0" class="error-msg">{{ ErrorInputMessages.Description[0]
                }}</i>
        </div>
        <input type="submit" value="Update">
    </form>
</template>
<style>
form {
    margin-top: 12px;
}

div.group-control {
    display: grid;
    grid-template-columns: 100px auto;
    margin-bottom: 40px;
    font-size: 14px;
}

div.group-control input,
div.group-control textarea {
    padding: 3px 8px;
    width: 300px;
}

div.group-control label {
    font-weight: bold;
}

div.group-control textarea {
    resize: vertical;
}

.update-tag-form input[type="submit"] {
    padding: 8px;
    background-color: var(--button-add-save-bg-color);
    color: white;
    border: none;
}
</style>