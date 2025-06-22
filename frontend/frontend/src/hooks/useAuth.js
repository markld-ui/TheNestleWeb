import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { login as apiLogin, register as apiRegister } from '../api/auth'

export const useAuth = () => {
  const [user, setUser] = useState(null)
  const [isAuthenticated, setIsAuthenticated] = useState(false)
  const [loading, setLoading] = useState(true)
  const navigate = useNavigate()

  useEffect(() => {
    const token = localStorage.getItem('token')
    if (token) {
      setIsAuthenticated(true)
    }
    setLoading(false)
  }, [])

  const login = async (email, password) => {
    try {
      const { token, refreshToken } = await apiLogin({ email, password })
      localStorage.setItem('token', token)
      localStorage.setItem('refreshToken', refreshToken)
      setIsAuthenticated(true)
      navigate('/dashboard')
    } catch (error) {
      throw error
    }
  }

  const register = async (firstName, lastName, email, password, gender) => {
    try {
      const { token, refreshToken } = await apiRegister({
        firstName,
        lastName,
        email,
        password,
        gender,
      })
      localStorage.setItem('token', token)
      localStorage.setItem('refreshToken', refreshToken)
      setIsAuthenticated(true)
      navigate('/dashboard')
    } catch (error) {
      throw error
    }
  }

  const logout = () => {
    localStorage.removeItem('token')
    localStorage.removeItem('refreshToken')
    setIsAuthenticated(false)
    navigate('/login')
  }

  return {
    user,
    isAuthenticated,
    loading,
    login,
    register,
    logout,
  }
}