'use client'
import Avatar from '@/components/ui/avatar/Avatar'
import { IChatSummary } from '@/models/chatSummary'
import Link from 'next/link'

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
	return (
		<Link
			href={`/chat/${chatSummary.id}`}
			className='p-layout flex items-center border-b border-border duration-300 ease-linear transition-colors hover:bg-border'
		>
			<Avatar
				name={`${chatSummary.name}`}
				size={50}
				bgColor='#127f73'
				textColor='#fff'
			/>
			<div className='text-sm w-full pl-2' style={{ fontWeight: 300 }}>
				<div className='flex items-center justify-between pb-2'>
					<span>{`${chatSummary.name}`}</span>
					<span className='text-xs opacity-30'></span>
				</div>
				<div className='flex items-center justify-between '>
					<div className='opacity-30 mt-0.2'>{`hello`}</div>
					<div className='opacity-30 mt-0.2'>6:55</div>
				</div>
			</div>
		</Link>
	)
}
