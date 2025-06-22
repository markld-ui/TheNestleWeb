import api from './index'

export const getInvites = async (pageNumber = 1, pageSize = 10) => {
  const response = await api.get('/invites', {
    params: { pageNumber, pageSize },
  })
  return response.data
}

export const getInvite = async (id) => {
  const response = await api.get(`/invites/${id}`)
  return response.data
}

export const createInvite = async (data) => {
  const response = await api.post('/invites', data); // Отправляем data как объект
  return response.data;
};

export const updateInvite = async (id, data) => {
  const response = await api.put(`/invites/${id}`, data)
  return response.data
}

export const deleteInvite = async (id) => {
  const response = await api.delete(`/invites/${id}`)
  return response.data
}

export const getMyInvites = async () => {
  const response = await api.get('/invites/my');
  return response.data;
};

export const acceptInvite = async (code) => {
  const response = await api.post('/invites/accept', { code });
  return response.data;
};