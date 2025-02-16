'use client'
import Field from '@/components/ui/field/Field'
import { ArrowRightToLine } from 'lucide-react'
import Image from 'next/image'
import { useState } from 'react'
import { useForm } from 'react-hook-form'
import { BsSendFill } from 'react-icons/bs'
import styles from './MessageFiled.module.css'

export function MessageField() {
	const [message, setMessage] = useState('')
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
					onChange={e => setMessage(e.target.value)}
				></Field>
				<button>
					<BsSendFill size={20} color='#539edf' />
				</button>
			</div>
		</div>
	)
}
