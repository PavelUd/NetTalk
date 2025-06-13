import { IChatSummary } from '@/models/chatSummary'
import { create } from 'zustand'
import {
	createChatConnection,
	invokeGetChats,
	startChatConnection,
	stopChatConnection,
	subscribeToChats,
} from '../api/chatListSocket'

export interface IChatListStore {
	chats: IChatSummary[]
	connect: (token: string) => Promise<void>
	disconnect: () => Promise<void>
	getChats: () => Promise<void>
	setChats: (chats: IChatSummary[]) => void
	handleChatListSubscription: () => void
}

export const useChatListStore = create<IChatListStore>(set => ({
	chats: [],

	connect: async (token: string) => {
		createChatConnection(token)
		useChatListStore.getState().handleChatListSubscription()
		await startChatConnection()
		await invokeGetChats()
	},

	disconnect: async () => {
		await stopChatConnection()
	},

	getChats: async () => {
		await invokeGetChats()
	},

	setChats: (chats: IChatSummary[]) => {
		set({ chats })
	},
	handleChatListSubscription: () => {
		subscribeToChats((chats: IChatSummary[]) => {
			useChatListStore.getState().setChats(chats)
		})
	},
}))
