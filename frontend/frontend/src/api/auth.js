import api from './index'

export const login = async (data) => {
  const response = await api.post('/auth/login', data)
  return response.data
}

export const register = async (data) => {
  const response = await api.post('/auth/register', data)
  return response.data
}

export const changePassword = async (data) => {
  const response = await api.post('/auth/change-password', data)
  return response.data
}