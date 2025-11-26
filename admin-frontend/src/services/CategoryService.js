import http from '../http'

export async function getAll(searchKey) {
  return await http.get(`/categories?searchKey=${searchKey}`)
}
export async function addCategory(category) {
  return await http.post('/categories', category)
}
export async function updateCategory(category) {
  return await http.put(`/categories/${category.id}`, category)
}
export async function deleteCategory(id) {
  return await http.delete(`/categories/${id}`)
}
