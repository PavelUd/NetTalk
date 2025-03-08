import { IMessage } from '@/models/message'
import { users } from './user'

export const messages: IMessage[] = Array.from({ length: 25 }, (_, index) => {
	const userIndex = index % users.length
	const randomText = `Message ${index + 1}`
	const randomDate = new Date(
		Date.now() - Math.floor(Math.random() * 100000000)
	).toISOString()

	return {
		id: (index + 1).toString(),
		text: randomText,
		createdAt: randomDate,
		idUser: String(userIndex),
		sender: users[userIndex],
	}
})
