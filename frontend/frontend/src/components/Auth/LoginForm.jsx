import { useState } from 'react'
import {
  Button,
  FormControl,
  FormLabel,
  Input,
  InputGroup,
  InputRightElement,
  Stack,
  Text,
  Link,
} from '@chakra-ui/react'
import { ViewIcon, ViewOffIcon } from '@chakra-ui/icons'
import { Link as RouterLink } from 'react-router-dom'

const LoginForm = ({ onSubmit, error }) => {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [showPassword, setShowPassword] = useState(false)

  const handleSubmit = (e) => {
    e.preventDefault()
    onSubmit(email, password)
  }

  return (
    <form onSubmit={handleSubmit}>
      <Stack spacing={4}>
        {error && (
          <Text color="red.500" textAlign="center">
            {error}
          </Text>
        )}
        <FormControl id="email" isRequired>
          <FormLabel>Email</FormLabel>
          <Input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </FormControl>
        <FormControl id="password" isRequired>
          <FormLabel>Пароль</FormLabel>
          <InputGroup>
            <Input
              type={showPassword ? 'text' : 'password'}
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
            <InputRightElement h={'full'}>
              <Button
                variant={'ghost'}
                onClick={() => setShowPassword(!showPassword)}
              >
                {showPassword ? <ViewOffIcon /> : <ViewIcon />}
              </Button>
            </InputRightElement>
          </InputGroup>
        </FormControl>
        <Stack spacing={10}>
          <Button
            type="submit"
            bg={'brand.500'}
            color={'white'}
            _hover={{
              bg: 'brand.600',
            }}
          >
            Войти
          </Button>
        </Stack>
        <Text align={'center'}>
          Нет аккаунта?{' '}
          <Link as={RouterLink} to="/register" color={'brand.500'}>
            Зарегистрироваться
          </Link>
        </Text>
      </Stack>
    </form>
  )
}

export default LoginForm