import { create } from 'zustand'

interface IUpdateMessageStore {
	idMessage: string
	text: string
	idChat: string
	isUpdating: boolean

	setUpdateMessage: (data: {
		idMessage: string
		text: string
		idChat: string
	}) => void

	updateText: (text: string) => void
	clearUpdateMessage: () => void
	setIsUpdating: (isUpdating: boolean) => void
}

export const useUpdateMessageStore = create<IUpdateMessageStore>(set => ({
	idMessage: '',
	text: '',
	idChat: '',
	isUpdating: false,

	setUpdateMessage: ({ idMessage, text, idChat }) =>
		set({ idMessage, text, idChat, isUpdating: true }),

	updateText: (text: string) => set({ text }),

	clearUpdateMessage: () =>
		set({ idMessage: '', text: '', idChat: '', isUpdating: false }),

	setIsUpdating: isUpdating => set({ isUpdating }),
}))
