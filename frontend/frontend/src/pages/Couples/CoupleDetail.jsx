import { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom'
import { getCouple } from '../../api/couples'
import { Box, Heading, Text, Button, Spinner, Badge } from '@chakra-ui/react'
import { Link as RouterLink } from 'react-router-dom'

const CoupleDetail = () => {
  const { id } = useParams()
  const [couple, setCouple] = useState(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchCouple = async () => {
      try {
        const data = await getCouple(id)
        setCouple(data)
        setLoading(false)
      } catch (error) {
        console.error('Error fetching couple:', error)
        setLoading(false)
      }
    }

    fetchCouple()
  }, [id])

  if (loading) {
    return <Spinner size="xl" />
  }

  if (!couple) {
    return <Text>Пара не найдена</Text>
  }

  return (
    <Box p={4}>
      <Heading mb={6}>Пара #{couple.coupleId}</Heading>
      
      <Box bg="white" p={4} borderRadius="md" boxShadow="md" mb={4}>
        <Text><strong>User 1 ID:</strong> {couple.user1Id}</Text>
        <Text><strong>User 2 ID:</strong> {couple.user2Id}</Text>
        <Text>
          <strong>Дата создания:</strong> {new Date(couple.createdAt).toLocaleString()}
        </Text>
      </Box>

      <Button as={RouterLink} to="/couples" colorScheme="blue">
        Назад к списку
      </Button>
    </Box>
  )
}

export default CoupleDetail