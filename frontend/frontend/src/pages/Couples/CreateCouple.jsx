import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { createCouple } from '../../api/couples';
import { createCoupleByAdmin } from '../../api/couples';
import { Box, Heading, FormControl, FormLabel, Input, Button, useToast } from '@chakra-ui/react'

const CreateCouple = () => {
  const [user1Id, setUser1Id] = useState('')
  const [user2Id, setUser2Id] = useState('')
  const [loading, setLoading] = useState(false)
  const navigate = useNavigate()
  const toast = useToast()

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)
    
    try {
      await createCoupleByAdmin(Number(user1Id), Number(user2Id))
      toast({
        title: 'Пара создана',
        status: 'success',
        duration: 3000,
      })
      navigate('/couples')
    } catch (error) {
      toast({
        title: 'Ошибка',
        description: error.response?.data?.message || 'Не удалось создать пару',
        status: 'error',
        duration: 5000,
      })
    } finally {
      setLoading(false)
    }
  }

  return (
    <Box p={4}>
      <Heading mb={6}>Создать пару</Heading>
      
      <form onSubmit={handleSubmit}>
        <FormControl mb={4}>
          <FormLabel>ID первого пользователя</FormLabel>
          <Input
            type="number"
            value={user1Id}
            onChange={(e) => setUser1Id(e.target.value)}
            required
          />
        </FormControl>
        
        <FormControl mb={4}>
          <FormLabel>ID второго пользователя</FormLabel>
          <Input
            type="number"
            value={user2Id}
            onChange={(e) => setUser2Id(e.target.value)}
            required
          />
        </FormControl>
        
        <Button type="submit" colorScheme="blue" isLoading={loading}>
          Создать пару
        </Button>
      </form>
    </Box>
  )
}

export default CreateCouple