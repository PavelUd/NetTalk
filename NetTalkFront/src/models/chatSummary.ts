import { IMessage } from './message'

export interface IChatSummary {
	type: string
	name: string
	id: string
	lastMessage: IMessage | null | undefined
	avatar: string | null | undefined
}
