import { useAuthStore } from '@/features/auth/model/authStore'
class FetchClient {
	private API_URL = 'http://localhost:5209/api/' as string
	constructor(private defaultHeaders: Record<string, string> = {}) {}

	async get<T>(
		path: string,
		isAuth: boolean = false,
		headers?: Record<string, string>
	): Promise<T> {
		return this.fetch<T>(path, 'GET', isAuth, undefined, headers)
	}

	async post<T>(
		path: string,
		body?: Record<string, unknown>,
		isAuth: boolean = false,
		headers?: Record<string, string>,
		isCredentials: boolean = false
	): Promise<T> {
		return this.fetch<T>(path, 'POST', isAuth, body, headers, isCredentials)
	}

	async put<T>(
		path: string,
		body?: Record<string, unknown>,
		isAuth: boolean = false,
		headers?: Record<string, string>
	): Promise<T> {
		return this.fetch<T>(path, 'PUT', isAuth, body, headers)
	}

	async delete<T>(
		path: string,
		isAuth: boolean = false,

		headers?: Record<string, string>
	): Promise<T> {
		return this.fetch<T>(path, 'DELETE', isAuth, undefined, headers)
	}

	async patch<T>(
		path: string,
		body?: Record<string, unknown>,
		isAuth: boolean = false,
		headers?: Record<string, string>
	): Promise<T> {
		return this.fetch<T>(path, 'PATCH', isAuth, body, headers)
	}

	private async fetch<T>(
		path: string,
		method: string,
		isAuth: boolean,
		body?: Record<string, unknown>,
		headers?: Record<string, string>,
		isCredentials: boolean = false
	): Promise<T> {
		const url = `${this.API_URL}${path}`
		const { accessToken, setAccessToken } = useAuthStore.getState()

		try {
			const response = await fetch(url, {
				method,
				credentials: 'include',
				headers: {
					'Content-Type': 'application/json',
					...this.defaultHeaders,
					...(isAuth && accessToken
						? { Authorization: `Bearer ${accessToken}` }
						: {}),
					...headers,
				},
				body: body ? JSON.stringify(body) : null,
			})
			const data = await response.json()

			if (!response.ok) {
				throw new Error(data.error)
			}

			return data
		} catch (error) {
			throw new Error(error)
		}
	}
	private async tryRefreshToken(
		setAccessToken: (token: string | null) => void
	): Promise<string> {
		try {
			const refreshRes = await this.post<string>(
				`auth/refresh`,
				'',
				false,
				null,
				true
			)
			const { accessToken: newToken } = refreshRes.data
			setAccessToken(newToken)
			return newToken
		} catch (error) {
			throw new Error(error)
		}
	}
}

export const $fetch = new FetchClient()
