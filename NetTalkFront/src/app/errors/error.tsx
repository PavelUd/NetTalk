import { useSearchParams } from 'next/navigation'

export default function ErrorPage() {
	const searchParams = useSearchParams()
	const error = searchParams.get('error')

	return (
		<div>
			<h1>Ошибка аутентификации</h1>
			{error && <p>Причина: {error}</p>}
		</div>
	)
}
