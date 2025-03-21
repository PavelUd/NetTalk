import type { Metadata } from 'next'

import { Auth } from '@/components/screens/auth/Auth'
import { NO_INDEX_PAGE } from '@/const/seo.const'

export const metadata: Metadata = {
	title: 'Register',
	...NO_INDEX_PAGE,
}

export default function RegisterPage() {
	return <Auth type='Register' />
}
