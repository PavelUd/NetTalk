import LayoutClient from '@/components/layout/Layout'
import type { Metadata } from 'next'
import './globals.css'

export const metadata: Metadata = {
	title: 'NET Talk',
	description: 'Best social media web app for everyone!',
	icons: '/logo.svg',
}

export default function RootLayout({
	children,
}: {
	children: React.ReactNode
}) {
	return (
		<html lang='en'>
			<body>
				<LayoutClient>{children}</LayoutClient>
			</body>
		</html>
	)
}
