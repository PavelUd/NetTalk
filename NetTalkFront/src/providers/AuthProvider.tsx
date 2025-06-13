import { $fetch } from '@/api/api.fetch'
import { useAuthStore } from '@/features/auth/model/authStore'
import { usePathname, useRouter } from 'next/navigation'
import { PropsWithChildren, useEffect, useState } from 'react'

export default function AuthProvider({ children }: PropsWithChildren<unknown>) {
	const { accessToken, setAccessToken } = useAuthStore()
	const pathname = usePathname()
	const router = useRouter()
	const [loading, setLoading] = useState(true)

	useEffect(() => {
		const checkAuth = async () => {
			try {
				if (accessToken) {
					console.log(accessToken)
					return
				}
				const result = await $fetch.post(
					'auth/refresh',
					undefined,
					false,
					undefined,
					true
				)
				console.log(result)
				setAccessToken(result.data)
			} catch (err) {
				console.log(err)
				if (pathname !== '/login' && pathname !== '/register') {
					router.replace('/login')
				}
			} finally {
				setLoading(false)
			}
		}

		checkAuth()
	}, [pathname, router, setAccessToken])

	if (loading) {
		return <div>Loading...</div> // можно заменить на спиннер
	}

	return <>{children}</>
}
