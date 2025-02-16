import LayoutClient from '@/components/layout/Layout'
import { Poppins } from '@next/font/google'
import type { Metadata } from 'next'
import './globals.css'

export const metadata: Metadata = {
	title: 'NET Talk',
	description: 'Best social media web app for everyone!',
	icons: '/logo.svg',
}

const poppins = Poppins({
	subsets: ['latin'],
	weight: ['100', '200', '300', '400', '500', '700'], // Укажи толщины текста
})

export default function RootLayout({
	children,
}: {
	children: React.ReactNode
}) {
	return (
		<html lang='en'>
			<body className={poppins.className}>
				<LayoutClient>{children}</LayoutClient>
			</body>
		</html>
	)
}
