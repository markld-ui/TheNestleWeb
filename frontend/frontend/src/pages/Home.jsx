import { Box, Heading, Text, Button } from '@chakra-ui/react'
import { Link as RouterLink } from 'react-router-dom'

const Home = () => {
  return (
    <Box textAlign="center" py={10} px={6}>
      <Heading as="h1" size="xl" mb={4}>
        Добро пожаловать в Thenestle
      </Heading>
      <Text fontSize="lg" mb={6}>
        Платформа для управления парами и приглашениями
      </Text>
      <Button
        as={RouterLink}
        to="/login"
        colorScheme="brand"
        size="lg"
        mr={4}
      >
        Войти
      </Button>
      <Button
        as={RouterLink}
        to="/register"
        variant="outline"
        colorScheme="brand"
        size="lg"
      >
        Зарегистрироваться
      </Button>
    </Box>
  )
}

export default Home