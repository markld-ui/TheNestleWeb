import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../../hooks/useAuth'
import LoginForm from '../../components/Auth/LoginForm'
import { Box, Container, Heading, VStack } from '@chakra-ui/react'

const Login = () => {
  const { login } = useAuth()
  const navigate = useNavigate()
  const [error, setError] = useState('')

  const handleSubmit = async (email, password) => {
    try {
      await login(email, password)
      navigate('/dashboard')
    } catch (err) {
      setError('Неверный email или пароль')
    }
  }

  return (
    <Container maxW="md" py={10}>
      <VStack spacing={8}>
        <Heading as="h1" size="xl" textAlign="center">
          Вход
        </Heading>
        <Box w="full">
          <LoginForm onSubmit={handleSubmit} error={error} />
        </Box>
      </VStack>
    </Container>
  )
}

export default Login