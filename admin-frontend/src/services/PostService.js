import http from '../http'

export async function getAll(searchKey) {
  return await http.get(`/posts?searchKey=${searchKey}`)
}
export async function getById(id) {
  return await http.get(`/posts/${id}`)
}
export async function addPost(post) {
  return await http.post('/posts', post)
}
export async function updatePost(post) {
  return await http.put(`/posts/${post.id}`, post)
}
export async function deletePost(id) {
  return await http.delete(`/posts/${id}`)
}
