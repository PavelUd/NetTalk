'use state'

import dayjs from 'dayjs'
import Image from 'next/image'
import React from 'react'
import { IoCallOutline, IoEllipsisVerticalOutline } from 'react-icons/io5'
import { RiLayout4Fill } from 'react-icons/ri'
import { SlMagnifier } from 'react-icons/sl'
import { MessageField } from '../MessageField'
import styles from './Chat.module.css'
import { Message } from './Message'

export function Chat({ chatData }) {
	const { name, messages } = chatData

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
							<p>{name}</p>
							<div className={`${styles.gradientText}`}>
								<div>22 Members</div>
								<div className='ml-1'>Online</div>
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
						{messages.map((element, index) => (
							<React.Fragment key={index}>
								<Message
									username={element.sender.username}
									avatarUrl={element.sender.avatar}
									time={dayjs(element.createdAt).format('HH:mm')}
									message={element.text || 'default message'}
									isSelf={element.sender.username == 'Darza'}
								/>
							</React.Fragment>
						))}
					</div>
				</div>
			</div>
			<MessageField />
		</div>
	)
}
