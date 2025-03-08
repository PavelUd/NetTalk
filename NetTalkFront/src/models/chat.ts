import { IMessage } from './message'
import { IUser } from './user'

export interface IChat {
	type: string
	name: string
	id: string
	isActive: boolean
	messages: IMessage[]
	owner: string
	users: IUser[]
}
