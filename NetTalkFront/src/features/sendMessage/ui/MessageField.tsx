'use client'
import Field from '@/components/ui/field/Field'
import { useAuthStore } from '@/features/auth/model/authStore'
import { useChatListStore } from '@/features/chatList/store/ChatListStore'
import { useChatStore } from '@/features/chats/stores/chatStore'
import { ArrowRightToLine } from 'lucide-react'
import Image from 'next/image'
import { useEffect, useState } from 'react'
import { useForm } from 'react-hook-form'
import { BsSendFill } from 'react-icons/bs'
import { IoCheckmarkCircleSharp } from 'react-icons/io5'
import { useUpdateMessageStore } from '../model/updateMessageStorage'
import styles from './MessageFiled.module.css'

export function MessageField() {
	const [message, setMessage] = useState('')
	const { accessToken } = useAuthStore()
	const {
		sendMessage,
		currentChatId,
		receiveMessages,
		receiveDeleteMessage,
		receiveUpdateMessage,
		updateMessage,
	} = useChatStore()

	const {
		idChat,
		text,
		idMessage,
		isUpdating,
		updateText,
		clearUpdateMessage,
	} = useUpdateMessageStore()
	const { getChats } = useChatListStore()
	const handleClick = async () => {
		if (isUpdating) {
			await updateMessage(idChat, idMessage, text)
			clearUpdateMessage()
		} else {
			await sendMessage(currentChatId, message)
		}
		setMessage('')
	}

	const handleOnChangeText = (text: string) => {
		if (isUpdating) {
			updateText(text)
		} else {
			setMessage(text)
		}
	}

	useEffect(() => {
		if (isUpdating) {
			setMessage(text)
		}
		if (!accessToken) return
		receiveDeleteMessage()
		receiveUpdateMessage()
		receiveMessages(getChats)
	})
	const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
		if (e.key === 'Enter' && !e.shiftKey) {
			e.preventDefault()
			handleClick()
		}
	}
	const {} = useForm<object>()

	return (
		<div className={styles.messageFiledContainer}>
			<Image
				src='https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava1-bg.webp'
				priority
				alt=''
				className='m-6'
				width={40}
				height={40}
			/>
			<div
				className='border border-border border-[#5789b69b] flex items-center justify-between mr-6 p-3 w-full rounded-lg'
				style={{ background: 'var(--chatListBackground)' }}
			>
				<Field
					className='w-full'
					placeholder='Write a message...'
					Icon={ArrowRightToLine}
					value={message}
					onChange={e => handleOnChangeText(e.target.value)}
					onKeyDown={handleKeyDown}
				></Field>
				{isUpdating ? (
					<button onClick={handleClick}>
						<IoCheckmarkCircleSharp size={30} color='#539edf' />
					</button>
				) : (
					<button onClick={handleClick}>
						<BsSendFill size={30} color='#539edf' />
					</button>
				)}
			</div>
		</div>
	)
}
