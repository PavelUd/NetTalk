'use client'

import Field from '@/components/ui/field/Field'
import { Loader } from '@/components/ui/loader/Loader'
import styles from '@/features/chatList/ui/ChatList.module.scss'
import { useChatStore } from '@/features/chats/stores/chatStore'
import * as Dialog from '@radix-ui/react-dialog'
import { AnimatePresence, motion } from 'framer-motion'
import { useRef, useState } from 'react'
import { SlMagnifier } from 'react-icons/sl'
import { usePeopleModalStore } from '../store/peopleModalStore'
import { useUserSearchStore } from '../store/userSearchStore'

export default function ModalWindow() {
	const { isOpen, close } = usePeopleModalStore()
	const { searchUsers, users, isLoading, clearUsers } = useUserSearchStore()

	const [query, setQuery] = useState('')
	const debounceTimer = useRef<NodeJS.Timeout | null>(null)
	const { setCurrentDirectByUserId } = useChatStore()

	const handleClick = (id: string, userName: string) => {
		console.log(id.toString())
		close()
		setCurrentDirectByUserId(id.toString(), userName)
	}

	const handleSearch = (query: string) => {
		if (debounceTimer.current) clearTimeout(debounceTimer.current)

		debounceTimer.current = setTimeout(() => {
			if (query.trim()) {
				searchUsers(query)
			}
		}, 300)
	}
	const handleClose = () => {
		clearUsers()
		setQuery('')
		close()
	}

	return (
		<Dialog.Root open={isOpen} onOpenChange={value => !value && handleClose()}>
			<Dialog.Portal>
				<Dialog.Overlay className='fixed inset-0 bg-black/40 z-40' />
				<AnimatePresence>
					{isOpen && (
						<Dialog.Content asChild forceMount>
							<motion.div
								initial={{ opacity: 0, scale: 0.95 }}
								animate={{ opacity: 1, scale: 1 }}
								exit={{ opacity: 0, scale: 0.8 }}
								transition={{ duration: 0.3 }}
								className='fixed left-1/3 top-6 transform -translate-x-1/2 -translate-y-1/2
	           p-6 rounded-md shadow-xl z-50 w-full max-w-md space-y-4 h-[750px]'
								style={{ background: '#1c1c25' }}
							>
								<Dialog.Title className='text-xl font-bold mb-2 text-white'>
									Найти Пользователя
								</Dialog.Title>

								<div className={styles.searchBar}>
									<Field
										className={styles.searchField}
										placeholder='Search...'
										value={query}
										onChange={e => {
											const value = e.target.value
											setQuery(value)
											handleSearch(value)
										}}
									/>
									<SlMagnifier className={styles.magnifierIcon} size={20} />
								</div>

								{/* Список пользователей */}
								<div className='space-y-3'>
									{isLoading ? (
										<div className='flex justify-center items-center h-[calc(750px-13rem)]'>
											<Loader />
										</div>
									) : users.length == 0 ? (
										<div
											className='flex justify-center items-center h-[calc(750px-13rem)]'
											style={{ color: '#474646' }}
										>
											Тут пока пусто
										</div>
									) : (
										users.map(user => (
											<div
												onClick={() => handleClick(user.id, user.email)}
												key={user.id}
												className='flex items-center justify-between bg-[#2a2a38] px-4 py-2 rounded-md text-white'
											>
												<div className='flex items-center gap-3'>
													<img
														src={user.avatar}
														alt={user.email}
														className='w-10 h-10 rounded-full object-cover'
													/>
													<span>{user.email}</span>
												</div>
											</div>
										))
									)}
								</div>

								<Dialog.Close asChild></Dialog.Close>
							</motion.div>
						</Dialog.Content>
					)}
				</AnimatePresence>
			</Dialog.Portal>
		</Dialog.Root>
	)
}
