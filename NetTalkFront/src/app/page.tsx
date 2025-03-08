import { Chat } from '@/components/screens/chats/chat/Chat'
import { ChatsList } from '@/components/screens/chats/list/ChatList'

export default function ChatsPage() {
	return (
		<div
			className='grid'
			style={{
				gridTemplateColumns: '2.5fr 8fr',
			}}
		>
			<ChatsList />
			<Chat />
		</div>
	)
}
