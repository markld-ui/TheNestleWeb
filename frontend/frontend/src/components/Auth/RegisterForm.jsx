import { useState } from 'react'
import {
  Button,
  FormControl,
  FormLabel,
  Input,
  InputGroup,
  InputRightElement,
  Select,
  Stack,
  Text,
  Link,
} from '@chakra-ui/react'
import { ViewIcon, ViewOffIcon } from '@chakra-ui/icons'
import { Link as RouterLink } from 'react-router-dom'

const RegisterForm = ({ onSubmit, error }) => {
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [gender, setGender] = useState('')
  const [showPassword, setShowPassword] = useState(false)

  const handleSubmit = (e) => {
    e.preventDefault()
    onSubmit(firstName, lastName, email, password, gender)
  }

  return (
    <form onSubmit={handleSubmit}>
      <Stack spacing={4}>
        {error && (
          <Text color="red.500" textAlign="center">
            {error}
          </Text>
        )}
        <FormControl id="firstName" isRequired>
          <FormLabel>Имя</FormLabel>
          <Input
            type="text"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
          />
        </FormControl>
        <FormControl id="lastName" isRequired>
          <FormLabel>Фамилия</FormLabel>
          <Input
            type="text"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
          />
        </FormControl>
        <FormControl id="email" isRequired>
          <FormLabel>Email</FormLabel>
          <Input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </FormControl>
        <FormControl id="gender" isRequired>
          <FormLabel>Пол</FormLabel>
          <Select
            placeholder="Выберите пол"
            value={gender}
            onChange={(e) => setGender(e.target.value)}
          >
            <option value="Male">Мужской</option>
            <option value="Female">Женский</option>
            <option value="Other">Другой</option>
          </Select>
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
            Зарегистрироваться
          </Button>
        </Stack>
        <Text align={'center'}>
          Уже есть аккаунт?{' '}
          <Link as={RouterLink} to="/login" color={'brand.500'}>
            Войти
          </Link>
        </Text>
      </Stack>
    </form>
  )
}

export default RegisterForm