'use client'

import { IChatSummary } from '@/models/chatSummary'
import Image from 'next/image'
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
			<Image
				src={`${chatSummary.lastMessage.sender.avatar}`}
				alt={'fg'}
				width={45}
				height={45}
				className='mr-4'
			/>
			<div className='text-sm w-full' style={{ fontWeight: 300 }}>
				<div className='flex items-center justify-between pb-2'>
					<span>{`${chatSummary.lastMessage.sender.username}`}</span>
					<span className='text-xs opacity-30'></span>
				</div>
				<div className='flex items-center justify-between '>
					<div className='opacity-30 mt-0.2'>
						{`${chatSummary.lastMessage.text}`}
					</div>
					<div className='opacity-30 mt-0.2'>6:55</div>
				</div>
			</div>
		</Link>
	)
}
