import { Link as RouterLink } from 'react-router-dom'
import { useAuth } from '../../hooks/useAuth'
import {
  Box,
  Flex,
  Link,
  Button,
  Menu,
  MenuButton,
  MenuList,
  MenuItem,
  useColorModeValue,
  Stack,
  useColorMode,
  IconButton,
  Heading,
} from '@chakra-ui/react'
import { MoonIcon, SunIcon, HamburgerIcon } from '@chakra-ui/icons'

const Header = () => {
  const { isAuthenticated, logout } = useAuth()
  const { colorMode, toggleColorMode } = useColorMode()

  return (
    <Box bg={useColorModeValue('gray.100', 'gray.900')} px={4}>
      <Flex h={16} alignItems={'center'} justifyContent={'space-between'}>
        <Box>
          <Heading size="md">
            <Link as={RouterLink} to="/" _hover={{ textDecoration: 'none' }}>
              Thenestle
            </Link>
          </Heading>
        </Box>

        <Flex alignItems={'center'}>
          <Stack direction={'row'} spacing={7}>
            <IconButton
              onClick={toggleColorMode}
              icon={colorMode === 'light' ? <MoonIcon /> : <SunIcon />}
              aria-label={'Toggle color mode'}
              variant="ghost"
            />

            {isAuthenticated ? (
              <Menu>
                <MenuButton
                  as={Button}
                  rounded={'full'}
                  variant={'link'}
                  cursor={'pointer'}
                  minW={0}
                >
                  <HamburgerIcon />
                </MenuButton>
                <MenuList>
                  <MenuItem as={RouterLink} to="/dashboard">
                    Dashboard
                  </MenuItem>
                  <MenuItem as={RouterLink} to="/users">
                    Users
                  </MenuItem>
                  <MenuItem as={RouterLink} to="/couples">
                    Couples
                  </MenuItem>
                  <MenuItem as={RouterLink} to="/invites">
                    Invites
                  </MenuItem>
                  <MenuItem onClick={logout}>Logout</MenuItem>
                </MenuList>
              </Menu>
            ) : (
              <Stack direction={'row'} spacing={4}>
                <Button as={RouterLink} to="/login" colorScheme="brand">
                  Login
                </Button>
                <Button
                  as={RouterLink}
                  to="/register"
                  variant="outline"
                  colorScheme="brand"
                >
                  Register
                </Button>
              </Stack>
            )}
          </Stack>
        </Flex>
      </Flex>
    </Box>
  )
}

export default Header