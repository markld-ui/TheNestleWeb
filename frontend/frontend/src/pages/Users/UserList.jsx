import { Link as RouterLink } from 'react-router-dom'
import {
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
  Box,
  Link,
  Badge,
} from '@chakra-ui/react'

const UserList = ({ users }) => {
  return (
    <Box overflowX="auto">
      <Table variant="simple">
        <Thead>
          <Tr>
            <Th>ID</Th>
            <Th>Имя</Th>
            <Th>Email</Th>
            <Th>Пол</Th>
            <Th>Дата регистрации</Th>
            <Th>Баланс</Th>
            <Th>Действия</Th>
          </Tr>
        </Thead>
        <Tbody>
          {users.map((user) => (
            <Tr key={user.userId}>
              <Td>{user.userId}</Td>
              <Td>
                <Link as={RouterLink} to={`/users/${user.userId}`}>
                  {user.firstName} {user.lastName}
                </Link>
              </Td>
              <Td>{user.email}</Td>
              <Td>
                <Badge colorScheme={user.gender === 'Male' ? 'blue' : 'pink'}>
                  {user.gender}
                </Badge>
              </Td>
              <Td>{new Date(user.createdAt).toLocaleDateString()}</Td>
              <Td>${user.currencyBalance}</Td>
              <Td>
                <Link
                  as={RouterLink}
                  to={`/users/${user.userId}`}
                  color="brand.500"
                >
                  Просмотр
                </Link>
              </Td>
            </Tr>
          ))}
        </Tbody>
      </Table>
    </Box>
  )
}

export default UserList