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

				return {
					id: '123',
					email: 'paveludin4@gmail.com',
					username: 'loch',
					avatar: 'avatar_url',
					jwt: 'jwt_token',
				} as User
			},
		}),
	],
	pages: {
		error: '/error',
	},
	callbacks: {
		jwt({ token, user, account }) {
			return { ...token, ...user }
		},
		session({ session, token, user }) {
			session.user = token as unknown as User
			return session
		},
	},
}
