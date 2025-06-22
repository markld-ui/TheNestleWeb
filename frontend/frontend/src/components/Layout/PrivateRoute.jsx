import { Navigate, Outlet } from 'react-router-dom'
import { useAuth } from '../../hooks/useAuth'
import { Box, Spinner } from '@chakra-ui/react'

const PrivateRoute = () => {
  const { isAuthenticated, loading } = useAuth()

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minH="100vh">
        <Spinner size="xl" />
      </Box>
    )
  }

  return isAuthenticated ? <Outlet /> : <Navigate to="/login" />
}

export default PrivateRoute