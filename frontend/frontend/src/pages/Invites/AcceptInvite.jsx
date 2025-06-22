import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useParams } from 'react-router-dom'
import { acceptInvite } from '../../api/invites'
import { Box, Heading, Text, Button, useToast, Input, FormControl, FormLabel } from '@chakra-ui/react'

const AcceptInvite = () => {
  const [code, setCode] = useState('')
  const [loading, setLoading] = useState(false)
  const navigate = useNavigate()
  const toast = useToast()

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)
    
    try {
      await acceptInvite(code)
      toast({
        title: 'Приглашение принято',
        description: 'Пара успешно создана!',
        status: 'success',
        duration: 3000,
      })
      navigate('/couples')
    } catch (error) {
      toast({
        title: 'Ошибка',
        description: error.response?.data?.message || 'Не удалось принять приглашение',
        status: 'error',
        duration: 5000,
      })
    } finally {
      setLoading(false)
    }
  }

  return (
    <Box p={4}>
      <Heading mb={6}>Принять приглашение</Heading>
      
      <form onSubmit={handleSubmit}>
        <FormControl mb={4}>
          <FormLabel>Введите код приглашения</FormLabel>
          <Input
            value={code}
            onChange={(e) => setCode(e.target.value)}
            placeholder="Код из 6 символов"
            required
          />
        </FormControl>
        
        <Button type="submit" colorScheme="blue" isLoading={loading} mr={4}>
          Принять приглашение
        </Button>
        <Button onClick={() => navigate('/invites')} variant="outline">
          Отмена
        </Button>
      </form>
    </Box>
  )
}

export default AcceptInvite