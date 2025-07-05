import api from './index'

export const getUsers = async (page = 1, pageSize = 10) => {
  try {
    const response = await fetch(`http://localhost:5205/api/v1/users?pageNumber=${page}&pageSize=${pageSize}`, {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });
    return await response.json();
  } catch (error) {
    console.error('Error fetching users:', error);
    throw error;
  }
};

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