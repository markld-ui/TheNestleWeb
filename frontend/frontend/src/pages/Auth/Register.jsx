import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../../hooks/useAuth'
import RegisterForm from '../../components/Auth/RegisterForm'
import { Box, Container, Heading, VStack } from '@chakra-ui/react'

const Register = () => {
  const { register } = useAuth()
  const navigate = useNavigate()
  const [error, setError] = useState('')

  const handleSubmit = async (firstName, lastName, email, password, gender) => {
    try {
      await register(firstName, lastName, email, password, gender)
      navigate('/dashboard')
    } catch (err) {
      setError('Ошибка регистрации. Попробуйте снова.')
    }
  }

  return (
    <Container maxW="md" py={10}>
      <VStack spacing={8}>
        <Heading as="h1" size="xl" textAlign="center">
          Регистрация
        </Heading>
        <Box w="full">
          <RegisterForm onSubmit={handleSubmit} error={error} />
        </Box>
      </VStack>
    </Container>
  )
}

export default Register