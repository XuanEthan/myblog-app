<script setup>
import { onMounted, reactive } from 'vue'
import { useTagStore } from '@/stores/tagStore'
import { useToast } from 'vue-toastification'
import SearchForm from '@/components/common/SearchForm.vue'
import SubmitButton from '@/components/base/SubmitButton.vue'

const toast = useToast()
const tagStore = useTagStore()

const query = reactive({
    searchKey: ""
})

const tagForm = reactive({
    name: '',
    description: ''
})

const ErrorInputMessages = reactive({
    Name: [],
    Description: []
})

async function handleAdd() {
    try {
        resetErrorInputMessages()
        await tagStore.addTag(tagForm)
        await tagStore.getAll(query.searchKey)
        tagForm.name = ''
        tagForm.description = ''
        toast.success('Tag added successfully!')
    } catch (e) {
        if (e.response?.data?.errors)
            Object.assign(ErrorInputMessages, e.response.data.errors)
    }
}

function resetErrorInputMessages() {
    ErrorInputMessages.Name = ''
    ErrorInputMessages.Description = ''
}

async function handleSearch() {
    await tagStore.getAll(query.searchKey)
}

async function handleDelete(id) {
    if (confirm('Delete this tag?') === false) return
    await tagStore.deleteTag(id)
    toast.success('Tag deleted successfully!')
}

onMounted(() => {
    tagStore.getAll(query.searchKey)
})
</script>

<template>
    <div class="header">
        <h1>Tags</h1>
    </div>

    <SearchForm :placeholder="'Search by Name'" @submit="handleSearch" v-model:searchKey="query.searchKey" />

    <div class="Tags-container">
        <!-- Form Add -->
        <div class="add-tag-form">
            <h2>Add Tag</h2>
            <form @submit.prevent="handleAdd">
                <label for="name">Name</label>
                <input type="text" id="name" v-model="tagForm.name">
                <i v-if="ErrorInputMessages.Name.length > 0" class="error-msg">{{ ErrorInputMessages.Name[0]
                    }}</i>
                <label for="description">Description</label>
                <textarea id="description" v-model="tagForm.description"></textarea>
                <i v-if="ErrorInputMessages.Description.length > 0" class="error-msg">{{
                    ErrorInputMessages.Description[0] }}</i>
                <div>
                    <SubmitButton :name="'Add Tag'" :position="'relative'" />
                </div>
            </form>
        </div>

        <!-- Table -->
        <div class="tag-list">
            <p class="item-count">{{ tagStore.tags.length }} items</p>
            <table>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="tag in tagStore.tags" v-if="tagStore.tags.length > 0" :key="tag.id">
                        <td>
                            <RouterLink :to="`/UpdateTag/${tag.id}`" class="tag-name">{{ tag.name }}</RouterLink>
                        </td>
                        <td class="tag-description">{{ tag.description || '_' }}</td>
                        <td>
                            <RouterLink :to="`/UpdateTag/${tag.id}`">Edit</RouterLink> | <a
                                @click="handleDelete(tag.id)">Delete</a>
                        </td>
                    </tr>
                    <tr v-if="tagStore.tags.length === 0">
                        <td colspan="3" style="text-align: center; padding: 20px 0; font-size: 14px;"><i>Not found any
                                tags.</i></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<style scoped>
/* tag container layout */
.Tags-container {
    display: grid;
    grid-template-columns: 35% 1fr;
    gap: 20px;
    margin-top: 16px;
}


.add-tag-form form {
    display: flex;
    flex-direction: column;
    margin-top: 12px;
}

.add-tag-form label {
    font-size: 13px;
    margin-bottom: 4px;
}

.add-tag-form input[type="text"],
.add-tag-form textarea {
    padding: 8px;
    margin-bottom: 12px;
    border: 1px solid #ccc;
    border-radius: 4px;
}

.add-tag-form textarea {
    min-height: 80px;
    resize: vertical;
}

/* 
.add-tag-form input[type="submit"] {
    width: 70px;
    padding: 8px;
    border: none;
    background-color: var(--button-add-save-bg-color);
    color: white;
    cursor: pointer;
} */

.add-tag-form input[type="submit"]:hover {
    background-color: #0056b3;
}



/* Table styles */
.tag-list table {
    width: 100%;
    max-width: 100%;
    border-collapse: collapse;
    border: 1px solid var(--table-border-bg-color);
    font-size: 14px;
    margin-top: 12px;
}

.tag-list th,
.tag-list td {
    padding: 12px;
    text-align: left;
    border-bottom: 1px solid #ddd;
    font-weight: normal;
}

.tag-list thead {
    background-color: white;
    font-weight: 600;
}

.tag-list tbody tr:nth-child(even) {
    background-color: white;
}

.tag-list th:first-child,
.tag-list td:first-child {
    width: 40%;
}

.tag-list th:nth-child(2),
.tag-list td:nth-child(2) {
    width: 40%;
}

.tag-list th:last-child,
.tag-list td:last-child {
    width: 20%;
}

.tag-list a {
    color: var(--a-color-normal);
    text-decoration: none;
    cursor: pointer;
}

.tag-list .tag-name,
.tag-list .tag-description {
    word-break: break-word;
}

.tag-list a:hover {
    text-decoration: underline;
}
</style>
