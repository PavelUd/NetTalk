import { create } from 'zustand'
type AuthState = {
	accessToken: string | null
	userId: string | null
	setAccessToken: (token: string | null) => void
}

type RegistrationState = {
	email: string
	id: string
	setEmail: (email: string) => void
	setId: (id: string) => void
}

export const useAuthStore = create<AuthState>(set => ({
	accessToken: null,
	userId: null,
	setAccessToken: (token: string | null) => {
		if (!token) {
			set({ accessToken: null, userId: null })
			return
		}

		const payload = parseJwt(token)
		set({ accessToken: token, userId: payload?.Id || null })
	},
}))

export const useRegistrationStore = create<RegistrationState>(set => ({
	email: '',
	id: '',
	setEmail: email => set(() => ({ email })),
	setId: id => set(() => ({ id })),
}))

function parseJwt(token: string): any | null {
	try {
		const base64Url = token.split('.')[1]
		const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
		const jsonPayload = decodeURIComponent(
			atob(base64)
				.split('')
				.map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
				.join('')
		)
		return JSON.parse(jsonPayload)
	} catch (e) {
		return null
	}
}
