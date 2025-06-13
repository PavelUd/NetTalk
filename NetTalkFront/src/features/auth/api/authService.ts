import { $fetch } from '@/api/api.fetch'
import { useAuthStore, useRegistrationStore } from '../model/authStore'

type AuthResponse = {
	data: string
}

export const login = async (email: string, password: string) => {
	const data = await $fetch.post<AuthResponse>(
		'auth/login',
		{
			Login: email,
			Password: password,
		},
		false,
		undefined,
		true
	)
	const accessToken = data.data
	console.log('[login] setting token:', data)

	useAuthStore.getState().setAccessToken(accessToken)
}

export const sendRegisterRequest = async (
	email: string,
	login: string,
	password: string
) => {
	const data = await $fetch.post<AuthResponse>(
		'auth/register',
		{
			Email: email,
			Login: login,
			Password: password,
		},
		false,
		undefined,
		true
	)
	const result = data.data
	console.log(result)
	useRegistrationStore.getState().setId(result.id)
	useRegistrationStore.getState().setEmail(result.email)
}

export const confirmRegister = async (id: string, code: number) => {
	console.log(id, code)
	const data = await $fetch.post<AuthResponse>(
		'auth/verify',
		{
			IdRegistration: id,
			Code: code,
		},
		false,
		undefined,
		true
	)
	const accessToken = data.data
	console.log('[login] setting token:', data)
	useAuthStore.getState().setAccessToken(accessToken)
}
