import { $fetch } from '@/api/api.fetch'
import { JwtPayload } from '@/models/jwtPayload'
import { jwtDecode } from 'jwt-decode'
import { User } from 'next-auth'
import Credentials from 'next-auth/providers/credentials'
export const nextAuthOptions = {
	debug: !!process.env.AUTH_DEBUG,
	providers: [
		Credentials({
			credentials: {
				username: {
					type: 'text',
				},
				email: {
					type: 'text',
				},
				password: { type: 'password' },
			},
			async authorize(credentials) {
				console.log('Received credentials:', credentials)
				const response = await $fetch.post('api/auth/login', credentials)
				const user = jwtDecode<JwtPayload>(JSON.stringify(response))
				return {
					id: user.Id,
					email: user.unique_name,
					username: user.FullName,
					avatar: user.PhotoUrl,
					jwt: response,
				} as User
			},
		}),
	],
	pages: {
		error: '/error',
	},
	callbacks: {
		jwt({ token, user, account }) {
			console.log(token, user, account)
			return { ...token, ...user }
		},
		session({ session, token, user }) {
			session.user = token as unknown as User
			return session
		},
	},
}
