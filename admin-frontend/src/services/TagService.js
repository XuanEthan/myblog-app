import http from '../http'

export async function getById(id) {
  return await http.get(`/tags/${id}`)
}

export async function getAll(searchKey) {
  return await http.get(`/tags?searchKey=${searchKey}`)
}
export async function addTag(tag) {
  return await http.post('/tags', tag)
}
export async function updateTag(tag) {
  return await http.put(`/tags/${tag.id}`, tag)
}
export async function deleteTag(id) {
  return await http.delete(`/tags/${id}`)
}
