import { IUser } from './user.types'

export interface IMessage {
	id: string
	text: string
	createdAt: string
	sender: IUser
}

export interface IChat {
	id: string
	messages: IMessage[]
	participants: IUser[]
}

export interface IStrapiChat {
	id: number
	messages: IMessage[]
	participants: IUser[]
}

export interface IStrapiResponse<T> {
	attributes: T
	id: number
}
