import { useState, useEffect } from 'react';
import { getMyInvites } from '../../api/invites';
import { Box, Heading, Table, Thead, Tbody, Tr, Th, Td, Button, Spinner, Badge } from '@chakra-ui/react';
import { Link as RouterLink } from 'react-router-dom';

const Invites = () => {
  const [invites, setInvites] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchInvites = async () => {
      try {
        const data = await getMyInvites();
        setInvites(data);
        setLoading(false);
      } catch (error) {
        console.error('Error fetching invites:', error);
        setLoading(false);
      }
    };

    fetchInvites();
  }, []);

  if (loading) {
    return <Spinner size="xl" />;
  }

  return (
    <Box p={4}>
      <Heading mb={6}>Мои приглашения</Heading>
      <Button as={RouterLink} to="/invites/create" colorScheme="blue" mb={4}>
        Создать приглашение
      </Button>
      
      <Table variant="simple">
        <Thead>
          <Tr>
            <Th>Код</Th>
            <Th>Статус</Th>
            <Th>Действителен до</Th>
            <Th>Действия</Th>
          </Tr>
        </Thead>
        <Tbody>
          {invites.map(invite => (
            <Tr key={invite.inviteId}>
              <Td>{invite.code}</Td>
              <Td>
                <Badge colorScheme={invite.status === 'accepted' ? 'green' : 'orange'}>
                  {invite.status}
                </Badge>
              </Td>
              <Td>{new Date(invite.expiresAt).toLocaleString()}</Td>
              <Td>
                <Button as={RouterLink} to={`/invites/${invite.inviteId}`} size="sm">
                  Подробнее
                </Button>
              </Td>
            </Tr>
          ))}
        </Tbody>
      </Table>
    </Box>
  );
};

export default Invites;