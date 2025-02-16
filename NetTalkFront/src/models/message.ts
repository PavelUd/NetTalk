import { IUser } from './user'

export interface IMessage {
	id: string
	text: string
	createdAt: string
	sender: IUser
}
