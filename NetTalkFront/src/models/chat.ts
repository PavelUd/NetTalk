import { IMessage } from './message'
import { IUser } from './user'

export interface IChat {
	type: string
	id: string
	avatar: string
	name: string
	messages: IMessage[]
	participants: IUser[]
}
