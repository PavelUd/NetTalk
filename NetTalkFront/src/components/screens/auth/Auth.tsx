'use client'
import Field from '@/components/ui/field/Field'
import { AtSign, KeyRound } from 'lucide-react'
import { signIn } from 'next-auth/react'
import Image from 'next/image'
import { useRouter } from 'next/navigation'
import { useState } from 'react'
import { SubmitHandler, useForm } from 'react-hook-form'
import toast from 'react-hot-toast'
import { IAuthFormState } from './auth.types'

interface IAuth {
	type?: 'Login' | 'Register'
}

export function Auth({ type }: IAuth) {
	const [isLoading, setIsLoading] = useState(false)
	const { register, handleSubmit } = useForm<IAuthFormState>({
		mode: 'onChange',
	})

	const { push } = useRouter()

	const onSubmit: SubmitHandler<IAuthFormState> = async data => {
		setIsLoading(true)
		const response = await signIn('credentials', {
			redirect: false,
			login: data.email,
			password: data.password,
		})

		if (response?.error) {
			toast.error(response.error)
			setIsLoading(false)
			return
		} else {
			console.log(response)
		}

		setIsLoading(false)
		push('/')
	}

	const buttons = [
		{
			id: 'apple',
			imgSrc:
				'https://upload.wikimedia.org/wikipedia/commons/f/fa/Apple_logo_black.svg',
			alt: 'Apple',
		},
		{
			id: 'google',
			imgSrc:
				'https://upload.wikimedia.org/wikipedia/commons/5/53/Google_%22G%22_Logo.svg',
			alt: 'Google',
		},
		{
			id: 'x',
			imgSrc:
				'https://upload.wikimedia.org/wikipedia/commons/3/34/Twitter_logo.svg',
			alt: 'X',
		},
	]

	return (
		<div className='flex w-screen h-screen items-center justify-center'>
			<form
				onSubmit={handleSubmit(onSubmit)}
				className='block border-t border-border p-7 rounded-xl shadow-[15px_15px_45px_#010101]'
				style={{
					background: '#18181c',
					width: '450px',
				}}
			>
				<div
					className='bg-gradient-to-br from-[#1c1c1c] to-[#202020] shadow-[5px_5px_5px_#010101,-5px_-5px_10px_#282828] border-t-2 border-l-2 border-border rounded-xl flex items-center justify-center'
					style={{
						width: 50,
						height: 50,
						marginRight: 'auto',
						marginLeft: 'auto',
						marginBottom: '1rem',
					}}
				>
					<Image src='/logo.svg' priority alt='' width={40} height={40} />
				</div>
				<div
					className='text-center pb-4'
					style={{
						fontWeight: '500',
						fontSize: 24,
					}}
				>
					Welcome Back
				</div>
				<div className='text-center mb-6 text-sm' style={{ fontWeight: '400' }}>
					<span style={{ opacity: 0.4, paddingRight: '2px' }}>
						Dont't have an accaunt yet?{' '}
					</span>
					<span className='text-sm'>Sing up</span>
				</div>
				<Field
					{...register('email', {
						required: true,
					})}
					placeholder='Enw email'
					type=''
					Icon={AtSign}
					style={{ background: 'black' }}
					className='mb-4 border border-border p-2 rounded-xl'
				/>
				<Field
					{...register('password', {
						required: true,
						minLength: {
							value: 1,
							message: 'Min length 6 symbols!',
						},
					})}
					placeholder='Enter password'
					type='password'
					style={{ background: 'black' }}
					Icon={KeyRound}
					className='mb-8 border border-border p-2 rounded-xl'
				/>
				<div
					className='text-center p-1 rounded-lg'
					style={{
						background: 'linear-gradient(135deg, #0174dc, #0074db)',
						display: 'flex',
						justifyContent: 'center',
						alignItems: 'center',
					}}
				>
					<button disabled={isLoading} type='submit' style={{ width: '100%' }}>
						Login
					</button>
				</div>
				<div
					className='text-center mt-2 mb-2'
					style={{
						opacity: 0.5,
						padding: '0 10px',
						display: 'flex',
						alignItems: 'center',
						textAlign: 'center',
					}}
				>
					<hr style={{ flex: 1 }} />
					<span className='p-2'>OR</span>
					<hr style={{ flex: 1 }} />
				</div>
				<div className='flex gap-5'>
					{buttons.map(button => (
						<div
							key={`${button.id}`}
							className='w-60 h-10 shadow-[0_5px_5px_#010101] rounded-lg flex items-center justify-center'
							style={{
								background: '#252525',
								marginRight: 'auto',
								marginLeft: 'auto',
							}}
						>
							<Image src='/logo.svg' priority alt='' width={20} height={20} />
						</div>
					))}
				</div>
			</form>
		</div>
	)
}
