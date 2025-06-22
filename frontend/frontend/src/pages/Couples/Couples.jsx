import { useState, useEffect } from 'react';
import { getMyCouple } from '../../api/couples';
import { Box, Heading, Text, Button, Spinner } from '@chakra-ui/react';
import { Link as RouterLink } from 'react-router-dom';

const Couples = () => {
  const [couple, setCouple] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchCouple = async () => {
      try {
        const data = await getMyCouple();
        setCouple(data || null); // data будет null при 404
      } catch (error) {
        setCouple(null);
      } finally {
        setLoading(false);
      }
    };

    fetchCouple();
  }, []);

  if (loading) {
    return <Spinner size="xl" />;
  }

  if (!couple) {
    return (
      <Box p={4}>
        <Heading mb={4}>У вас пока нет пары</Heading>
        <Text mb={4}>Создайте или примите приглашение, чтобы создать пару</Text>
        <Button as={RouterLink} to="/invites" colorScheme="blue">
          Перейти к приглашениям
        </Button>
        <Button as={RouterLink} to="/couples/create" colorScheme="blue">
          Создать пару
        </Button>
      </Box>
    );
  }

  return (
    <Box p={4}>
      <Heading mb={6}>Моя пара</Heading>
      <Box bg="white" p={4} borderRadius="md" boxShadow="md">
        <Text><strong>ID пары:</strong> {couple.coupleId}</Text>
        <Text><strong>User 1 ID:</strong> {couple.user1Id}</Text>
        <Text><strong>User 2 ID:</strong> {couple.user2Id}</Text>
        <Text>
          <strong>User 1 Name:</strong>{' '}
          {couple.user1 ? `${couple.user1.firstName} ${couple.user1.lastName}` : 'Неизвестно'}
        </Text>
        <Text>
          <strong>User 2 Name:</strong>{' '}
          {couple.user2 ? `${couple.user2.firstName} ${couple.user2.lastName}` : 'Неизвестно'}
        </Text>
        <Text><strong>Дата создания:</strong> {new Date(couple.createdAt).toLocaleString()}</Text>
      </Box>
    </Box>
  );
};

export default Couples;