<script setup>
import { onMounted, reactive } from 'vue'
import { useCategoryStore } from '@/stores/categoryStore'
import SearchForm from '@/components/common/SearchForm.vue'
import SubmitButton from '@/components/base/SubmitButton.vue'

const categoryStore = useCategoryStore()

const query = reactive({
    searchKey: ""
})

const categoryForm = reactive({
    name: '',
    description: ''
})

async function handleAdd() {
    await categoryStore.addCategory(categoryForm)
    await categoryStore.getAll(query.searchKey)
    categoryForm.name = ''
    categoryForm.description = ''
}

async function handleSearch() {
    await categoryStore.getAll(query.searchKey)
}

onMounted(() => {
    categoryStore.getAll(query.searchKey)
})

</script>

<template>
    <div class="header">
        <h1>Categories</h1>
    </div>
    <SearchForm :placeholder="'Search by Name'" @submit="handleSearch" v-model:searchKey="query.searchKey" />

    <div class="categories-container">
        <!-- Form Add -->
        <div class="add-category-form">
            <h2>Add Category</h2>
            <form @submit.prevent="handleAdd">
                <label for="name">Name</label>
                <input type="text" id="name" v-model="categoryForm.name" disabled>

                <label for="description">Description</label>
                <textarea id="description" v-model="categoryForm.description" disabled></textarea>
                <div>
                    <!-- <SubmitButton :name="'Add Category'" :position="'relative'" /> -->
                </div>
            </form>
        </div>

        <!-- Table -->
        <div class="category-list">
            <p class="item-count">{{ categoryStore.categories.length }} items</p>
            <table>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="category in categoryStore.categories" v-if="categoryStore.categories.length > 0"
                        :key="category.id">
                        <td><a href="#">{{ category.name }}</a></td>
                        <td>{{ category.description || '_' }}</td>
                        <td><a href="#">Edit</a> | <a href="#">Delete</a></td>
                    </tr>
                    <tr v-if="categoryStore.categories.length === 0">
                        <td colspan="3" style="text-align: center; padding: 20px 0; font-size: 14px;"><i>Not found any
                                categories.</i>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<style scoped>
/* Category container layout */
.categories-container {
    display: grid;
    grid-template-columns: 35% 1fr;
    gap: 20px;
    margin-top: 16px;
}

.add-category-form form {
    display: flex;
    flex-direction: column;
    margin-top: 12px;
}

.add-category-form label {
    font-size: 13px;
    margin-bottom: 4px;
}

.add-category-form input[type="text"],
.add-category-form textarea {
    padding: 8px;
    margin-bottom: 12px;
    border: 1px solid #ccc;
    border-radius: 4px;
}

.add-category-form textarea {
    min-height: 80px;
    resize: vertical;
}

.add-category-form input[type="submit"]:hover {
    background-color: #0056b3;
}


/* Table styles */
.category-list table {
    width: 100%;
    border-collapse: collapse;
    border: 1px solid #ddd;
    font-size: 14px;
    margin-top: 12px;
}

.category-list th,
.category-list td {
    padding: 12px;
    text-align: left;
    border-bottom: 1px solid #ddd;
    font-weight: normal;
}

.category-list thead {
    background-color: white;
    font-weight: 600;
}

.category-list tbody tr:nth-child(even) {
    background-color: white;
}

.category-list th:first-child,
.category-list td:first-child {
    width: 40%;
}

.category-list th:last-child,
.category-list td:last-child {
    width: 20%;
}

.category-list a {
    color: #007bff;
    text-decoration: none;
    cursor: pointer;
}

.category-list a:hover {
    text-decoration: underline;
}
</style>
