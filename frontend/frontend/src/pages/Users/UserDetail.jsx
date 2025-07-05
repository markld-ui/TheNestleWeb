import { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import { getUser } from '../../api/users';
import {
  Box,
  Heading,
  Text,
  Stack,
  Badge,
  Button,
  useToast,
} from '@chakra-ui/react'
import { Link as RouterLink } from 'react-router-dom'

const UserDetail = () => {
  const { id } = useParams()
  const [user, setUser] = useState(null)
  const [loading, setLoading] = useState(true)
  const toast = useToast()

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const userData = await getUser(Number(id))
        setUser(userData)
        setLoading(false)
      } catch (error) {
        console.error('Ошибка загрузки пользователя:', error)
        toast({
          title: 'Ошибка',
          description: 'Не удалось загрузить пользователя',
          status: 'error',
          duration: 5000,
          isClosable: true,
        })
        setLoading(false)
      }
    }

    fetchUser()
  }, [id, toast])

  if (loading) {
    return <div>Загрузка...</div>
  }

  if (!user) {
    return <div>Пользователь не найден</div>
  }

  return (
    <Box p={4}>
      <Stack direction="row" justifyContent="space-between" alignItems="center" mb={6}>
        <Heading as="h1" size="xl">
          {user.firstName} {user.lastName}
        </Heading>
        <Button as={RouterLink} to="/users" colorScheme="brand">
          Назад к списку
        </Button>
      </Stack>

      <Box bg="white" p={6} borderRadius="md" boxShadow="sm">
        <Stack spacing={4}>
          <Box>
            <Text fontWeight="bold">ID:</Text>
            <Text>{user.userId}</Text>
          </Box>
          <Box>
            <Text fontWeight="bold">Email:</Text>
            <Text>{user.email}</Text>
          </Box>
          <Box>
            <Text fontWeight="bold">Пол:</Text>
            <Badge colorScheme={user.gender === 'Male' ? 'blue' : 'pink'}>
              {user.gender}
            </Badge>
          </Box>
          <Box>
            <Text fontWeight="bold">Дата регистрации:</Text>
            <Text>{new Date(user.createdAt).toLocaleDateString()}</Text>
          </Box>
          <Box>
            <Text fontWeight="bold">Баланс:</Text>
            <Text>${user.currencyBalance}</Text>
          </Box>
        </Stack>
      </Box>
    </Box>
  )
}

export default UserDetail