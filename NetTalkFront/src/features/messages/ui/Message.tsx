import Image from 'next/image'
import { useContextMenu } from 'react-contexify'
import styles from './Message.module.css'
import { MESSAGE_MENU_ID, MessageContextMenu } from './MessageContextMenu'

interface MessageProps {
	id: string
	username?: string
	avatarUrl?: string
	isEditing: boolean
	time: string
	message: string
	isSelf?: boolean
}

export function Message({
	id,
	username,
	avatarUrl,
	isEditing,
	time,
	message,
	isSelf = false,
}: MessageProps) {
	const { show } = useContextMenu({ id: MESSAGE_MENU_ID })
	const handleContextMenu = (e: React.MouseEvent) => {
		if (!isSelf) return
		e.preventDefault()
		show({ event: e, props: { username, message, id } })
	}

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
			<div className='items-center ml-3' onContextMenu={handleContextMenu}>
				<div
					className={isSelf ? styles.selfMessageInfo : styles.otherMessageInfo}
				>
					<span className={styles.username}>{username}</span>
					{isSelf && isEditing ? (
						<span className={styles.time}>edited</span>
					) : (
						''
					)}
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
			<MessageContextMenu />
		</div>
	)
}
