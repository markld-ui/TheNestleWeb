// App.jsx
import { BrowserRouter } from 'react-router-dom'
import AppRoutes from './routes'
import Header from './components/Layout/Header'
import { Box } from '@chakra-ui/react'

function App() {
  return (
    <BrowserRouter>
      <Header />
      <Box p={4}>
        <AppRoutes />
      </Box>
    </BrowserRouter>
  )
}

export default App