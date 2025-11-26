import { defineStore } from 'pinia'
import { getAll, addTag, deleteTag, updateTag, getById } from '@/services/TagService'

export const useTagStore = defineStore('tag', {
  state: () => ({
    tags: [],
  }),
  actions: {
    async getById(id) {
      return (await getById(id)).data
    },
    async getAll(searchKey = "") {
      try {
        const resp = await getAll(searchKey)
        this.tags = resp.data
      } catch (e) {
        throw e
      }
    },
    async addTag(tag) {
      try {
        // await addTag(tag
        const resp = await addTag(tag)
      } catch (e) {
        throw e
      }
    },
    async updateTag(tag) {
      try {
        // await updateTag(tag)
        const resp = await updateTag(tag)
      } catch (e) {
        throw e
      }
    },
    async deleteTag(id) {
      try {
        // await deleteTag(id)
        const resp = await deleteTag(id)
        this.tags = this.tags.filter((t) => t.id !== id)
      } catch (e) {
        throw e
      }
    },
  },
})
