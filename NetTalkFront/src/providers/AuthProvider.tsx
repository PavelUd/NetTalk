'use client'

import { useAuth } from '@/hooks/useAuth'
import { usePathname, useRouter } from 'next/navigation'
import { useEffect, useState, type PropsWithChildren } from 'react'

export default function AuthProvider({ children }: PropsWithChildren<unknown>) {
	const { user, isLoggedIn } = useAuth()
	const pathname = usePathname()
	const router = useRouter()
	const [loading, setLoading] = useState(true)

	useEffect(() => {
		// Сохраняем JWT токен в localStorage при изменении user
		if (user) {
			window.localStorage.setItem('token', user.jwt || '')
		}
	}, [user, isLoggedIn])

	useEffect(() => {
		// Проверка токена и редирект, если нет токена
		const token = window.localStorage.getItem('token')
		if (!token && pathname !== '/login' && pathname !== '/register') {
			router.replace('/login')
		} else {
			setLoading(false)
		}
	}, [pathname, isLoggedIn, router])

	if (loading) {
		return <div>Loading...</div> // Или спиннер, чтобы пользователь не видел пустую страницу
	}

	return <>{children}</>
}
