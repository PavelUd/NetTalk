'use client'
import Field from '@/components/ui/field/Field'
import { motion } from 'framer-motion'
import Image from 'next/image'
import { useRouter } from 'next/navigation'

export const Register = ({ onClick }: { onClick: () => void }) => {
	const router = useRouter()
	return (
		<motion.div
			key='register'
			initial={{ x: 300, opacity: 0 }}
			animate={{ x: 0, opacity: 1 }}
			exit={{ x: -300, opacity: 0 }}
			transition={{ duration: 0.4 }}
		>
			<form onSubmit={e => e.preventDefault()}>
				<div
					className='bg-gradient-to-br from-[#1c1c1c] to-[#202020] shadow-[5px_5px_5px_#010101,-5px_-5px_10px_#282828] border-t-2 border-l-2 border-border rounded-xl flex items-center justify-center'
					style={{
						width: 60,
						height: 60,
						margin: '0 auto 1.5rem auto',
					}}
				>
					<Image src='/logo.svg' priority alt='Logo' width={40} height={40} />
				</div>

				<div
					className='text-center pb-6'
					style={{ fontWeight: '500', fontSize: 26 }}
				>
					Create an Account
				</div>

				<Field
					placeholder='Enter email'
					type='email'
					style={{ background: 'black' }}
					className='mb-5 border border-border p-2 rounded-xl'
				/>

				<Field
					placeholder='Enter login'
					style={{ background: 'black' }}
					className='mb-5 border border-border p-2 rounded-xl'
				/>

				<Field
					placeholder='Enter password'
					type='password'
					style={{ background: 'black' }}
					className='mb-5 border border-border p-2 rounded-xl'
				/>

				<Field
					placeholder='Confirm password'
					type='password'
					style={{ background: 'black' }}
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
					<button
						onClick={onClick}
						style={{ width: '100%', padding: '0.3rem 0' }}
					>
						Register
					</button>
				</div>

				<div className='text-center mt-6 text-sm' style={{ opacity: 0.6 }}>
					Already have an account?{' '}
					<button
						type='button'
						onClick={() => router.push('/login')}
						className='text-white underline'
					>
						Sign in
					</button>
				</div>
			</form>
		</motion.div>
	)
}
