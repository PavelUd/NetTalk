import { IChatSummary } from '@/models/chatSummary'
import { chats } from './chat'

export const chatSummaries: IChatSummary[] = chats.map(chat => {
	const lastMessage =
		chat.messages.length > 0 ? chat.messages[chat.messages.length - 1] : null

	return {
		type: chat.type,
		name: chat.name,
		id: chat.id,
		lastMessage: lastMessage,
		avatar: chat.avatar,
	}
})
