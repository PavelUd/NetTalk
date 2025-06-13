'use client'
import Avatar from '@/components/ui/avatar/Avatar'
import { useAuthStore } from '@/features/auth/model/authStore'
import { useChatStore } from '@/features/chats/stores/chatStore'
import { IChatSummary } from '@/models/chatSummary'
import dayjs from 'dayjs'

interface ChatListItemProps {
	chatSummary: IChatSummary
}

/* 
	TODO:
		[ ] - Search chats,
		[ ] - Exit button,
		[ ] - Socket chat (messages)
		[ ] - Send message,
		[ ] - List friends
		[ ] - Toggle friend
		[ ] - Settings profile
		[ ] - Get avatar in chat list item
*/

export const ChatListItem: React.FC<ChatListItemProps> = ({ chatSummary }) => {
	const { setCurrentChat } = useChatStore()
	const { accessToken } = useAuthStore()
	const handleClick = () => {
		setCurrentChat(chatSummary.id, accessToken)
	}
	return (
		<div
			key={chatSummary.id}
			onClick={handleClick}
			className='p-layout flex items-center border-b border-border duration-300 ease-linear transition-colors hover:bg-border'
		>
			<Avatar
				name={`${chatSummary.name}`}
				size={50}
				bgColor='#a70db5'
				textColor='#fff'
			/>
			<div className='text-sm w-full pl-2' style={{ fontWeight: 300 }}>
				<div className='flex items-center justify-between pb-2'>
					<span>{`${chatSummary.name}`}</span>
					<span className='text-xs opacity-30'></span>
				</div>
				<div className='flex items-center justify-between '>
					<div className='opacity-30 mt-0.2'>
						{chatSummary.lastMessage.text}
					</div>
					<div className='opacity-30 mt-0.2'>
						{dayjs(chatSummary.lastMessage.createdDate).format('HH:mm')}
					</div>
				</div>
			</div>
		</div>
	)
}
