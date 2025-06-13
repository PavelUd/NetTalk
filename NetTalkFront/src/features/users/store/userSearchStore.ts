import { $fetch } from '@/api/api.fetch'
import { UUID } from 'crypto'
import { create } from 'zustand'

interface UserSearch {
	username: string
	email: string
	avatar: string
	id: UUID
}

interface UserSearchStore {
	users: UserSearch[]
	isLoading: boolean
	error: string | null
	searchUsers: (query: string) => Promise<void>
	clearUsers: () => void
}

interface UserSearchStoreResult {
	data: UserSearch[]
}

export const useUserSearchStore = create<UserSearchStore>(set => ({
	users: [],
	isLoading: false,
	error: null,
	clearUsers: () => set({ users: [] }),
	searchUsers: async (query: string) => {
		set({ isLoading: true, error: null })

		try {
			const response = await $fetch.get<UserSearchStoreResult>(
				`users?login=${query}`,
				true
			)
			set({ users: response.data, isLoading: false })
		} catch (err: any) {
			set({ error: err.message || 'Search failed', isLoading: false })
		}
	},
}))
