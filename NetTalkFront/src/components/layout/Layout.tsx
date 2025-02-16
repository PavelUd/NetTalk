'use client'

import { usePathname } from 'next/navigation'
import type { PropsWithChildren } from 'react'
import { Toaster } from 'react-hot-toast'
import styles from './Layout.module.scss'
import MainProvider from './MainProvider'
import { Sidebar } from './sidebar/Sidebar'

export default function LayoutClient({ children }: PropsWithChildren<unknown>) {
	const pathname = usePathname()
	const hideSidebarRoutes = ['/login', '/register']
	return (
		<MainProvider>
			<main className={styles.layout}>
				{!hideSidebarRoutes.includes(pathname) && <Sidebar />}
				<section>{children}</section>
				<Toaster position='top-right' />
			</main>
		</MainProvider>
	)
}
