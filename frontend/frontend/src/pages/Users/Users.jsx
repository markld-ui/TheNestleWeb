import { useState, useEffect } from 'react'
import { getUsers } from '../../../api/users'
import UserList from '../../../components/Users/UserList'
import { Box, Heading, Button, Flex, Spinner } from '@chakra-ui/react'
import { Link as RouterLink } from 'react-router-dom'

const Users = () => {
  const [users, setUsers] = useState([])
  const [loading, setLoading] = useState(true)
  const [page, setPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        setLoading(true)
        const response = await getUsers(page)
        setUsers(response.data)
        setTotalPages(Math.ceil(response.totalCount / response.pageSize))
        setLoading(false)
      } catch (error) {
        console.error('Ошибка загрузки пользователей:', error)
        setLoading(false)
      }
    }

    fetchUsers()
  }, [page])

  return (
    <Box p={4}>
      <Flex justifyContent="space-between" alignItems="center" mb={6}>
        <Heading as="h1" size="xl">
          Пользователи
        </Heading>
        <Button as={RouterLink} to="/users/create" colorScheme="brand">
          Создать пользователя
        </Button>
      </Flex>

      {loading ? (
        <Flex justify="center">
          <Spinner size="xl" />
        </Flex>
      ) : (
        <UserList users={users} />
      )}

      <Flex justifyContent="center" mt={4}>
        <Button
          onClick={() => setPage((p) => Math.max(p - 1, 1))}
          disabled={page === 1}
          mr={2}
        >
          Назад
        </Button>
        <Button
          onClick={() => setPage((p) => p + 1)}
          disabled={page === totalPages}
        >
          Вперед
        </Button>
      </Flex>
    </Box>
  )
}

export default Users