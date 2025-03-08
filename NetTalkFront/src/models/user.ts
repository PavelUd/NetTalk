export interface IUser {
	id: string
	username: string
	email: string
	avatar: string
}

export type UserJwt = {
	user: IUser
	jwt: string
}
