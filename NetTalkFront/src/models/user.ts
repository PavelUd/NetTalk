export interface IUser {
	id: number
	username: string
	email: string
	avatar: string
}

export type UserJwt = {
	user: IUser
	jwt: string
}
