import { messages } from './message'
import { users } from './user'

export const chats = Array.from({ length: 5 }, (_, index) => {
	const user1 = users[index % users.length]
	const user2 = users[(index + 1) % users.length]
	const chatMessages = messages.filter(
		msg => msg.sender.id === user1.id || msg.sender.id === user2.id
	)

	return {
		id: (index + 1).toString(),
		avatar: '',
		name: `Chat ${index + 1}`,
		type: 'personal',
		messages: chatMessages,
		users: [user1, user2],
	}
})
