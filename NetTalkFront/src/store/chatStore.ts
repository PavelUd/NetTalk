import { chats } from '@/mocks/chat'
import { chatSummaries } from '@/mocks/chatSummary'
import { IChat } from '@/models/chat'
import { IChatSummary } from '@/models/chatSummary'
import { IMessage } from '@/models/message'
import { create } from 'zustand'
interface ChatStore {
	chats: IChatSummary[]
	currentChatId: string | null
	currentChat: IChat | null
	setCurrentChat: (chatId: string) => void
	setMessages: (messages: IMessage) => void
}

export const useChatStore = create<ChatStore>(set => ({
	chats: chatSummaries,
	currentChat: null,
	currentChatId: null,
	setCurrentChat: chatId =>
		set({
			currentChatId: chatId,
			currentChat: chats.find(ch => ch.id == chatId),
		}),
	setMessages: message =>
		set(state => ({
			currentChat: {
				...state.currentChat,
				messages: [...state.currentChat.messages, message],
			},
		})),
}))
