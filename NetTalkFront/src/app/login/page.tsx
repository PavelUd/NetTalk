import type { Metadata } from 'next'

import { NO_INDEX_PAGE } from '@/const/seo.const'
import { Auth } from '@/features/auth/ui/SignInPage'

export const metadata: Metadata = {
	title: 'Login',
	...NO_INDEX_PAGE,
}

export default function LoginPage() {
	return <Auth />
}
