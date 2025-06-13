import { ChatsList } from '@/features/chatList/ui/ChatList'
import { Chat } from '@/features/chats/ui/Chat'

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
