import { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom'
import { getInvite } from '../../api/invites'
import { Box, Heading, Text, Button, Spinner, Badge } from '@chakra-ui/react'
import { Link as RouterLink } from 'react-router-dom'

const InviteDetail = () => {
  const { id } = useParams()
  const [invite, setInvite] = useState(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchInvite = async () => {
      try {
        const data = await getInvite(id)
        setInvite(data)
        setLoading(false)
      } catch (error) {
        console.error('Error fetching invite:', error)
        setLoading(false)
      }
    }

    fetchInvite()
  }, [id])

  if (loading) {
    return <Spinner size="xl" />
  }

  if (!invite) {
    return <Text>Приглашение не найдено</Text>
  }

  return (
    <Box p={4}>
      <Heading mb={6}>Приглашение #{invite.inviteId}</Heading>
      
      <Box bg="white" p={4} borderRadius="md" boxShadow="md" mb={4}>
        <Text><strong>Код:</strong> {invite.code}</Text>
        <Text><strong>Статус:</strong> 
          <Badge ml={2} colorScheme={invite.status === 'accepted' ? 'green' : 'orange'}>
            {invite.status}
          </Badge>
        </Text>
        <Text><strong>Использовано:</strong> {invite.isUsed ? 'Да' : 'Нет'}</Text>
        <Text><strong>Дата создания:</strong> {new Date(invite.createdAt).toLocaleString()}</Text>
        <Text><strong>Действительно до:</strong> {new Date(invite.expiresAt).toLocaleString()}</Text>
      </Box>

      <Button as={RouterLink} to="/invites" colorScheme="blue">
        Назад к списку
      </Button>
    </Box>
  )
}

export default InviteDetail