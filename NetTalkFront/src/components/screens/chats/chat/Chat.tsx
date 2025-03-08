'use state'
'use client'

import { useAuth } from '@/hooks/useAuth'
import { useChatStore } from '@/store/chatStore'
import dayjs from 'dayjs'
import Image from 'next/image'
import React, { useEffect, useRef } from 'react'
import { IoCallOutline, IoEllipsisVerticalOutline } from 'react-icons/io5'
import { RiLayout4Fill } from 'react-icons/ri'
import { SlMagnifier } from 'react-icons/sl'
import { MessageField } from '../MessageField'
import styles from './Chat.module.css'
import { Message } from './Message'

export function Chat() {
	const { currentChatId, currentChat, setMessages } = useChatStore()
	const selectedChatId = currentChatId
	const messagesEndRef = useRef<HTMLDivElement>(null)
	const { user, isLoggedIn } = useAuth()
	/*	const { data, isLoading, isFetching } = useQuery({
		queryKey: ['chat'],
		queryFn: () => $fetch.get('api/chats/e9d352e1-2c12-4008-b55f-2bcc28ba82e3'),
		enabled: isLoggedIn,
	})
*/
	useEffect(() => {
		if (messagesEndRef.current) {
			messagesEndRef.current.scrollIntoView({ behavior: 'smooth' })
		}
	}, [currentChat?.messages])

	if (!selectedChatId) {
		return (
			<div
				style={{
					display: 'flex',
					justifyContent: 'center',
					alignItems: 'center',
					height: '100vh',
					width: '100%',
					color: '#474646',
				}}
			>
				<p>Chat not selected</p>
			</div>
		)
	}
	return (
		<div>
			<div className={styles.chatContainer}>
				<nav>
					<div className={`${styles.navLeft}`}>
						<Image
							src='https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava1-bg.webp'
							priority
							alt='Avatar'
							className=''
							width={45}
							height={45}
						/>
						<div className={`${styles.teamInfo}`}>
							<p>{currentChat?.name}</p>
							<div className={`${styles.gradientText}`}>
								{currentChat?.type === 'group' && (
									<div>{currentChat.users.length} Members</div>
								)}
							</div>
						</div>
					</div>
					<div className={styles.navRight}>
						<IoCallOutline size={25} className='ml-2' />
						<SlMagnifier size={25} className='m-4' />
						<IoEllipsisVerticalOutline size={25} />
						<div className={styles.iconWrapper}>
							<RiLayout4Fill size={25} />
						</div>
					</div>
				</nav>

				<div className={styles.messageList}>
					<div className='scroll-container'>
						{currentChat?.messages.map((element, index) => {
							const sender = element.sender
								? element.sender
								: currentChat.users.find(us => us.id === element.idUser)
							return (
								<React.Fragment key={index}>
									<Message
										username={sender?.username}
										avatarUrl={sender?.avatar}
										time={dayjs(element.createdAt).format('HH:mm')}
										message={element.text || 'default message'}
										isSelf={sender?.username == 'Darza'}
									/>
								</React.Fragment>
							)
						})}
						<div ref={messagesEndRef} />
					</div>
				</div>
			</div>
			<MessageField />
		</div>
	)
}
