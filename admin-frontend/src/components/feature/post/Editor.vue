<script setup>
import { ref, watch } from 'vue'
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Underline from '@tiptap/extension-underline'

// ---- Props & Emits để hỗ trợ v-model ----
const props = defineProps({
    modelValue: {
        type: String,
        default: '',
    },
})
const emit = defineEmits(['update:modelValue'])

// ---- Khởi tạo Editor ----
const editor = useEditor({
    extensions: [
        StarterKit,
        Underline,
    ],
    content: props.modelValue,
    onUpdate({ editor }) {
        emit('update:modelValue', editor.getHTML()) // gửi dữ liệu ra cha
    },
})

// ---- Đồng bộ khi cha đổi content ----
watch(
    () => props.modelValue,
    (value) => {
        if (editor.value && value !== editor.value.getHTML()) {
            editor.value.commands.setContent(value, false)
        }
    }
)

// ======== Các hành động ========

// Thêm ảnh
function addImage() {
    const url = prompt('Nhập URL ảnh:')
    if (url) editor.value.chain().focus().setImage({ src: url }).run()
}

// Thêm link
function setLink() {
    const previousUrl = editor.value.getAttributes('link').href
    const url = prompt('Nhập URL:', previousUrl)
    if (url === null) return
    if (url === '') {
        editor.value.chain().focus().unsetLink().run()
        return
    }
    editor.value.chain().focus().extendMarkRange('link').setLink({ href: url }).run()
}
</script>

<template>
    <div class="editor-container">
        <!-- Toolbar -->
        <div class="toolbar" v-if="editor">
            <div>
                <button type="button" @click="editor.chain().focus().toggleBold().run()"
                    :class="{ active: editor.isActive('bold') }">
                    B
                </button>
                <button type="button" @click="editor.chain().focus().toggleItalic().run()"
                    :class="{ active: editor.isActive('italic') }">
                    I
                </button>
                <button type="button" @click="editor.chain().focus().toggleUnderline().run()"
                    :class="{ active: editor.isActive('underline') }">
                    U
                </button>
                <input type="number" @change="" value="11">
            </div>
            <div>
                <!-- text algins -->

            </div>
            <!-- <button @click="setLink()">🔗</button>
            <button @click="addImage()">🖼️</button> -->
        </div>

        <!-- Nội dung -->
        <EditorContent v-if="editor" :editor="editor" class="editor" />
    </div>
</template>

<style scoped>
.editor-container {
    border: 1px solid #ddd;
    overflow: hidden;
    background: #fff;
    margin-top: 20px;
    max-width: 100%;
}

.toolbar {
    display: flex;
    align-items: center;
    gap: 6px;
    padding: 8px;
    height: auto;
    border-bottom: 1px solid #ddd;
    background: #f7f7f7;
}

.toolbar button {
    border: none;
    cursor: pointer;
    font-weight: bold;
    transition: background-color 0.2s;
    padding: 6px 10px;
    background-color: #f7f7f7;
}

.toolbar button:hover {
    background: #eee;
}

.toolbar button.active {
    background: #dcdcdc;
}

.toolbar input {
    width: 50px;
}

.editor {
    width: 100%;
    padding: 12px;
    font-size: 16px;
    line-height: 1.6;
}

.editor img {
    max-width: 100%;
    border-radius: 8px;
    margin: 10px 0;
}
</style>
