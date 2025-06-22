import api from './index'

export const getUsers = async (pageNumber = 1, pageSize = 10, sortField = 'UserId', ascending = true) => {
  const response = await api.get('/users', {
    params: { pageNumber, pageSize, sortField, ascending },
  })
  return response.data
}

export const getUser = async (id) => {
  const response = await api.get(`/users/${id}`)
  return response.data
}

export const updateUser = async (id, data) => {
  const response = await api.put(`/users/${id}`, data)
  return response.data
}

export const deleteUser = async (id) => {
  const response = await api.delete(`/users/${id}`)
  return response.data
}