import { useEffect, useState } from 'react'
import { getUsers } from '../api/users'
import { getCouples } from '../api/couples'
import { getInvites } from '../api/invites'
import {
  Box,
  SimpleGrid,
  Stat,
  StatLabel,
  StatNumber,
  Heading,
  Text,
} from '@chakra-ui/react'

const Dashboard = () => {
  const [users, setUsers] = useState([])
  const [couples, setCouples] = useState([])
  const [invites, setInvites] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchData = async () => {
        try {
          setLoading(true);
          const [usersData, couplesData, invitesData] = await Promise.all([
            getUsers(),
            getCouples().catch(() => ({ data: [], totalCount: 0 })), // Обработка ошибок
            getInvites().catch(() => []), // Обработка ошибок
          ]);
          
          setUsers(usersData.data || []);
          setCouples(couplesData.data || []);
          setInvites(invitesData || []);
          setLoading(false);
        } catch (error) {
          console.error('Ошибка загрузки данных:', error);
          setLoading(false);
          // Инициализируем состояния пустыми массивами при ошибке
          setUsers([]);
          setCouples([]);
          setInvites([]);
        }
      };

    fetchData()
  }, [])

  if (loading) {
    return <div>Загрузка...</div>
  }

  return (
    <Box p={4}>
      <Heading as="h1" size="xl" mb={6}>
        Панель управления
      </Heading>
      <SimpleGrid columns={{ base: 1, md: 3 }} spacing={4} mb={6}>
        <Stat p={4} bg="white" borderRadius="md" boxShadow="sm">
          <StatLabel>Всего пользователей</StatLabel>
          <StatNumber>{users.length}</StatNumber>
        </Stat>
        <Stat p={4} bg="white" borderRadius="md" boxShadow="sm">
          <StatLabel>Всего пар</StatLabel>
          <StatNumber>{couples.length}</StatNumber>
        </Stat>
        <Stat p={4} bg="white" borderRadius="md" boxShadow="sm">
          <StatLabel>Активные приглашения</StatLabel>
          <StatNumber>
            {invites.filter((invite) => !invite.isUsed).length}
          </StatNumber>
        </Stat>
      </SimpleGrid>

      <Box mb={6}>
        <Heading as="h2" size="md" mb={4}>
          Последние пользователи
        </Heading>
        {users.slice(0, 5).map((user) => (
          <Box key={user.userId} p={3} mb={2} bg="white" borderRadius="md" boxShadow="sm">
            <Text fontWeight="bold">
              {user.firstName} {user.lastName}
            </Text>
            <Text fontSize="sm" color="gray.600">
              {user.email}
            </Text>
          </Box>
        ))}
      </Box>

      <Box>
        <Heading as="h2" size="md" mb={4}>
          Последние пары
        </Heading>
        {couples.slice(0, 5).map((couple) => (
          <Box key={couple.coupleId} p={3} mb={2} bg="white" borderRadius="md" boxShadow="sm">
            <Text fontWeight="bold">Пара #{couple.coupleId}</Text>
            <Text fontSize="sm" color="gray.600">
              Создана: {new Date(couple.createdAt).toLocaleDateString()}
            </Text>
          </Box>
        ))}
      </Box>
    </Box>
  )
}

export default Dashboard