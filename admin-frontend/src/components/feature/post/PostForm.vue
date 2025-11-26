<script setup>
import { defineProps, defineEmits, reactive, watch, ref, onMounted, onBeforeMount } from 'vue'
import SubmitButton from '../../base/SubmitButton.vue'
import Editor from '../post/Editor.vue'
import { PostResponseDto } from '@/models/Post'
const IMAGE_SRC_BASE_URL = import.meta.env.VITE_IMAGE_SRC_BASE_URL;

const props = defineProps({
    post: {
        type: Object,
        required: false
    },
    inputErrorMessages: {
        type: Object,
        required: true
    },
    categoryStore: {
        type: Object,
        required: true
    },
    tagStore: {
        type: Object,
        required: true
    }, aboutTo: {
        type: String,
        required: false,
        default: 'Publish'
    }
})

const emits = defineEmits(['update:post', 'submit'])

// Deep-clone để tránh share reference ngầm giữa props và local copy.
// JSON clone đủ cho DTO đơn giản (không có Date/Function).
const postLocal = reactive(props.post
    ? structuredClone(JSON.parse(JSON.stringify(props.post)))
    : { id: 0, imageFile: '', title: '', content: '', categoryIds: [], tagIds: [] }
)

// Nếu props.post thay đổi từ bên ngoài → đồng bộ lại (deep clone)
watch(
    () => props.post,
    (newPost) => {
        Object.assign(postLocal, structuredClone(JSON.parse(JSON.stringify(newPost))))
        if (newPost?.imagePath)
            imagePreview.value = `${IMAGE_SRC_BASE_URL + newPost.imagePath}`
    },
    { deep: true }
)

const imagePreview = ref()

function onFileChange(event) {
    const file = event.target.files[0];
    postLocal.imageFile = file;
    imagePreview.value = URL.createObjectURL(file)
}

function onFormSubmit() { // will emits with formData instead.
    const formData = new FormData()
    formData.append('id', postLocal.id)
    formData.append('title', postLocal.title)
    formData.append('content', postLocal.content)

    postLocal.categoryIds.forEach(id => formData.append('categoryIds', id))
    postLocal.tagIds.forEach(id => formData.append('tagIds', id))

    if (postLocal.imageFile) {
        formData.append('imageFile', postLocal.imageFile)
    }
    emits('submit', formData)
}

function onRemoveChosenImage() {
    postLocal.imageFile = null
    imagePreview.value = null
    const fileInput = document.getElementById('imageFile')
    if (fileInput) {
        fileInput.value = null
    }
}

</script>

<template>
    <form @submit.prevent="onFormSubmit">
        <div class="form-left">
            <!-- Thêm name="title" để CSS match selector -->
            <input type="text" name="title" placeholder="Title" v-model="postLocal.title" />
            <i v-if="inputErrorMessages.Title?.length" class="error-msg">
                {{ inputErrorMessages.Title[0] }}
            </i>
            <label for="imageFile">Chọn ảnh cho bài viết : <input id="imageFile" type="file" name="imageFile"
                    accept="image/*" v-on:change="onFileChange"></label>
            <div v-if="imagePreview && imagePreview.length" class="image-preview">
                <i class="fa-solid fa-x fa-lg" style="color: #ff0000;" v-on:click="onRemoveChosenImage"></i>
                <img :src="imagePreview" alt="Preview" />
            </div>
            <i v-if="inputErrorMessages.ImageFile?.length" class="error-msg">
                {{ inputErrorMessages.ImageFile[0] }}
            </i>
            <Editor v-model="postLocal.content" />
            <i v-if="inputErrorMessages.Content?.length" class="error-msg">
                {{ inputErrorMessages.Content[0] }}
            </i>
        </div>

        <div class="form-right">
            <div class="card">
                <label>Categories</label>
                <div class="checkbox-group categories">
                    <label v-for="category in categoryStore.categories" :key="category.id">
                        <input type="checkbox" v-model="postLocal.categoryIds" :value="category.id" />
                        {{ category.name }}
                    </label>
                </div>
            </div>

            <div class="card">
                <label>Tags</label>
                <div class="checkbox-group tags">
                    <label v-for="tag in tagStore.tags" :key="tag.id">
                        <input type="checkbox" v-model="postLocal.tagIds" :value="tag.id" />
                        {{ tag.name }}
                    </label>
                </div>
            </div>
        </div>

        <SubmitButton :name="aboutTo" />
    </form>
</template>

<style scoped>
form {
    display: grid;
    width: 100%;
    height: 100%;
    grid-template-columns: auto 20%;
    gap: 12px;
}

/* --- FORM LEFT --- */
.form-left {
    display: flex;
    flex-direction: column;
    gap: 10px;
}

form input,
form textarea {
    border: none;
}

form input[type='text'],
form textarea {
    display: block;
    width: 100%;
    padding: 6px;
    border: 1px solid #ddd;
}

form textarea {
    height: 400px;
    resize: none;
}

/* CSS trước đó target theo input[name='title'] */
form label {
    font-size: 0.85rem;
}

form input[name='title'] {
    font-size: 30px;
    font-weight: 600;
}

.error-msg {
    color: red;
    font-size: 13px;
}

/* --- FORM RIGHT --- */
.form-right {
    width: 100%;
    display: flex;
    flex-direction: column;
    gap: 16px;
}

.card {
    background-color: #fff;
    border: 1px solid #ddd;
    padding: 12px;
}

.card>label {
    display: block;
    font-weight: 600;
    margin-bottom: 6px;
    border-bottom: 1px solid black;
}

.checkbox-group {
    display: flex;
    gap: 4px;
}

.checkbox-group.categories {
    flex-direction: column;
}

.checkbox-group.tags {
    flex-wrap: wrap;
    width: 100%;
    height: auto;
}

.checkbox-group label {
    display: flex;
    align-items: center;
    font-size: 14px;
}

/* --- TAG INPUT --- */
.card input[type='text'] {
    width: 100%;
    padding: 6px;
    border: 1px solid #ccc;
    border-radius: 4px;
}

.image-preview {
    position: relative;
    width: 200px;
    display: flex;
    justify-content: flex-start;
}

.image-preview img {
    width: 200px;
    max-width: 200px;
    max-height: 200px;
    object-fit: cover;
    border: 1px solid #ccc;
}

.image-preview .fa-x {
    position: absolute;
    top: 14px;
    right: -1px;
    cursor: pointer;
    z-index: 10;
}
</style>