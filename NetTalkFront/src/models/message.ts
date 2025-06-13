import { IUser } from '@/types/user.types'

export interface IMessage {
	id: string
	idUser: string
	text: string
	createdDate: string
	updatedDate: string
	sender?: IUser
}
