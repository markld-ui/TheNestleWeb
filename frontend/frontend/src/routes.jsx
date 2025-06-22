import { lazy, Suspense } from 'react'
import { Routes, Route } from 'react-router-dom'
import PrivateRoute from './components/Layout/PrivateRoute'
import { Spinner } from '@chakra-ui/react'

// Базовые страницы
const Home = lazy(() => import('./pages/Home'))
const Login = lazy(() => import('./pages/Auth/Login'))
const Register = lazy(() => import('./pages/Auth/Register'))
const Dashboard = lazy(() => import('./pages/Dashboard'))

// Пользователи
const Users = lazy(() => import('./pages/Users/Users'))
const UserDetail = lazy(() => import('./pages/Users/UserDetail'))

// Пары
const Couples = lazy(() => import('./pages/Couples/Couples'))
const CoupleDetail = lazy(() => import('./pages/Couples/CoupleDetail'))
const CreateCouple = lazy(() => import('./pages/Couples/CreateCouple'))

// Приглашения
const Invites = lazy(() => import('./pages/Invites/Invites'))
const InviteDetail = lazy(() => import('./pages/Invites/InviteDetail'))
const CreateInvite = lazy(() => import('./pages/Invites/CreateInvite'))
const AcceptInvite = lazy(() => import('./pages/Invites/AcceptInvite'))

const AppRoutes = () => {
  return (
    <Suspense fallback={<Spinner size="xl" thickness="4px" speed="0.65s" />}>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        
        <Route path="/dashboard" element={<PrivateRoute />}>
          <Route index element={<Dashboard />} />
        </Route>
        
        <Route path="/users" element={<PrivateRoute />}>
          <Route index element={<Users />} />
          <Route path=":id" element={<UserDetail />} />
        </Route>
        
        <Route path="/couples" element={<PrivateRoute />}>
          <Route index element={<Couples />} />
          <Route path=":id" element={<CoupleDetail />} />
          <Route path="create" element={<CreateCouple />} />
        </Route>

        <Route path="/invites" element={<PrivateRoute />}>
          <Route index element={<Invites />} />
          <Route path=":id" element={<InviteDetail />} />
          <Route path="create" element={<CreateInvite />} />
        </Route>

        <Route path="/invites" element={<PrivateRoute />}>
          <Route index element={<Invites />} />
          <Route path=":id" element={<InviteDetail />} />
          <Route path="create" element={<CreateInvite />} />
          <Route path="accept" element={<AcceptInvite />} />
        </Route>
      </Routes>
    </Suspense>
  )
}

export default AppRoutes