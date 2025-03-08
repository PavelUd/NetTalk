import Image from 'next/image'
import styles from './Message.module.css'

interface MessageProps {
	username?: string
	avatarUrl?: string
	time: string
	message: string
	isSelf?: boolean
}

export function Message({
	username,
	avatarUrl,
	time,
	message,
	isSelf = false,
}: MessageProps) {
	return (
		<div
			className={`${styles.messageContainer} ${
				isSelf ? styles.selfMessageContainer : ''
			}`}
		>
			<div
				className='flex items-start mt-6'
				style={{ display: isSelf ? 'none' : 'flex' }}
			>
				<Image
					src='https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava1-bg.webp'
					alt='avatar'
					width={45}
					height={45}
				/>
			</div>
			<div className='items-center ml-3'>
				<div
					className={isSelf ? styles.selfMessageInfo : styles.otherMessageInfo}
				>
					<span className={styles.username}>{username}</span>
					<span className={styles.time}>{time}</span>
					<Image
						src='https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava1-bg.webp'
						alt='avatar'
						width={30}
						height={30}
						style={{ display: !isSelf ? 'none' : 'flex' }}
					/>
				</div>
				<div
					className={`${styles.messageTextContainer} ${
						isSelf ? styles.selfMessageTextContainer : ''
					}`}
				>
					{message}
				</div>
			</div>
		</div>
	)
}
