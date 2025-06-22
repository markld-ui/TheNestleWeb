import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { createInvite } from '../../api/invites'
import { Box, Heading, Text, Button, useToast, Input, FormControl, FormLabel, Code, Flex } from '@chakra-ui/react'
import { Link as RouterLink } from 'react-router-dom';

const CreateInvite = () => {
  const [coupleId, setCoupleId] = useState('')
  const [inviteCode, setInviteCode] = useState('')
  const [loading, setLoading] = useState(false)
  const navigate = useNavigate()
  const toast = useToast()

  const handleSubmit = async (e) => {
      e.preventDefault();
      setLoading(true);
      
      try {
        const response = await api.post('/invites'); // Пустой body
        setInviteCode(response.data.code);
        toast({
          title: 'Приглашение создано',
          description: `Код: ${response.data.code}`,
          status: 'success',
          duration: null, // Не исчезает автоматически
          isClosable: true
        });
      } catch (error) {
        toast({
          title: 'Ошибка',
          description: error.response?.data?.message || 'Не удалось создать приглашение',
          status: 'error',
          duration: 5000,
        });
      } finally {
        setLoading(false);
      }
    };

  return (
    <Box p={4}>
      <Heading mb={6}>Создать приглашение</Heading>
      
      {inviteCode ? (
        <Box>
          <Text mb={4}>Приглашение успешно создано! Отправьте этот код другому пользователю:</Text>
          <Code p={2} fontSize="xl" mb={4}>{inviteCode}</Code>
          <Button onClick={() => navigate('/invites')} colorScheme="blue">
            Вернуться к списку
          </Button>
        </Box>
      ) : (
        <form onSubmit={handleSubmit}>
          <FormControl mb={4}>
            <FormLabel>ID пары (вашего текущего соединения)</FormLabel>
            <Input
              type="number"
              value={coupleId}
              onChange={(e) => setCoupleId(e.target.value)}
              required
            />
          </FormControl>
          
          <Flex gap={4}>
            <Button type="submit" colorScheme="blue" isLoading={loading}>
              Создать приглашение
            </Button>
            <Button onClick={() => navigate('/invites')} variant="outline">
              Отмена
            </Button>
          </Flex>
        </form>
      )}
    </Box>
  )
}

export default CreateInvite