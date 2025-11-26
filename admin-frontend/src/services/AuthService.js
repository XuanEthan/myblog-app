import http from '../http'

/**
 * @returns {{ok : boolean, result : Object}}
 */
export async function login(loginForm) {
  try {
    var response = await http.post('/auth/login', loginForm)
    return {
      ok: true,
      result: response,
    }
  } catch (error) {
    return { ok: false, result: error }
  }
}
