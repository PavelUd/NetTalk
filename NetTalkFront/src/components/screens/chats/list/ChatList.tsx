'use client'

import Field from '@/components/ui/field/Field'
import { useAuth } from '@/hooks/useAuth'
import { useChatStore } from '@/store/chatStore'
import { PiHashFill } from 'react-icons/pi'
import { SlMagnifier } from 'react-icons/sl'
import { TbEditCircle } from 'react-icons/tb'
import styles from './ChatList.module.scss'
import { ChatListItem } from './ChatListItem'

export function ChatsList() {
	const { chats } = useChatStore()
	console.log(chats)
	const { user, isLoggedIn } = useAuth()
	/*
	const { data, isLoading, isFetching } = useQuery({
		queryKey: ['chats'],
		queryFn: () => $fetch.get('api/chats'),
		enabled: isLoggedIn,
	})
		<div>
					{isLoading || isFetching ? (
						<div className='p-layout'>
							<Loader />
						</div>
					) : (
						<div>
							{data?.data.map((chatSummary: IChatSummary) => (
								<ChatListItem
									key={`${chatSummary.id}`}
									chatSummary={chatSummary}
								/>
							))}
						</div>
					)}
				</div>
*/
	return (
		<div className={styles.container}>
			<div className={styles.headerContainer}>
				<div className={styles.headerContent}>
					<div>
						<span className={styles.title}>Messages</span>
						<span className={styles.newMessagesCount}>
							<span className={styles.countNumber}>48</span>
							<span className={styles.newLabel}>New</span>
						</span>
					</div>
					<TbEditCircle className={styles.editIcon} size={25} />
				</div>

				<div className={styles.searchBar}>
					<Field
						className={styles.searchField}
						placeholder='Search...'
						value={''}
					/>
					<SlMagnifier className={styles.magnifierIcon} size={20} />
				</div>
			</div>

			<div className={styles.chatListContainer}>
				<div className={styles.groupsHeader}>
					<div className={styles.groupsLabelContainer}>
						<PiHashFill className={styles.hashIcon} size={20} />
						<span className={styles.groupsLabel}>GROUPS & CHANNELS</span>
					</div>
					{chats.map(chatSummary => (
						<ChatListItem
							key={`${chatSummary.name}-12233332`}
							chatSummary={chatSummary}
						/>
					))}
				</div>
				<div className={styles.allMessagesLabel}>All MESSAGES</div>
			</div>
		</div>
	)
}
