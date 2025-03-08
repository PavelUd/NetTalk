import { IUser } from '@/types/user.types'

export interface IMessage {
	id: string
	idUser: string
	text: string
	createdAt: string
	sender?: IUser
}
