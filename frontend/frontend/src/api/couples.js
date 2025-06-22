import api from './index';

export const getCouples = async (pageNumber = 1, pageSize = 10, sortField = "CoupleId", ascending = true) => {
  const response = await api.get("/couples", {
    params: { pageNumber, pageSize, sortField, ascending },
  });
  return response.data;
};

export const getCouple = async (id) => {
  const response = await api.get(`/couples/${id}`);
  return response.data;
};

export const createCouple = async () => {
  const response = await api.post('/couples/create');
  return response.data;
};

export const completeCouple = async (coupleId, secondUserId) => {
  const response = await api.post(`/couples/${coupleId}/complete`, { secondUserId });
  return response.data;
};

export const deleteCouple = async (id) => {
  const response = await api.delete(`/couples/${id}`);
  return response.data;
};

export const getMyCouple = async () => {
  try {
    const response = await api.get('/couples/my');
    return response.data ?? null; // Явно возвращаем null при отсутствии данных
  } catch (error) {
    console.error('Error fetching couple:', error);
    return null;
  }
};

export const generateInvite = async (coupleId) => {
  const response = await api.post(`/invites/generate/${coupleId}`);
  return response.data;
};

export const acceptInvite = async (code) => {
  const response = await api.post('/invites/accept', { code });
  return response.data;
};

export const createCoupleByAdmin = async (user1Id, user2Id) => {
  const response = await api.post('/couples/createByAdmin', { user1Id, user2Id });
  return response.data;
};