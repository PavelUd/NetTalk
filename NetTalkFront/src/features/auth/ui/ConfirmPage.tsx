'use client'
import { motion } from 'framer-motion'
import Image from 'next/image'
import { useRouter } from 'next/navigation'
import { useRef, useState } from 'react'
import { confirmRegister } from '../api/authService'
import { useRegistrationStore } from '../model/authStore'

export const Confirm = ({ onClick }: { onClick: () => void }) => {
	const [code, setCode] = useState(Array(6).fill(''))
	const inputsRef = useRef<Array<HTMLInputElement | null>>([])

	const { id, email } = useRegistrationStore()
	const router = useRouter()

	const handleClick = async e => {
		e.preventDefault()
		const number = Number(code.join(''))
		try {
			console.log(id, number)
			await confirmRegister(id, number)
			router.push('/')
		} catch (err) {
			console.error('Login error:', err)
		}
	}
	const handleChange = (value: string, index: number) => {
		if (!/^\d?$/.test(value)) return

		const newCode = [...code]
		newCode[index] = value
		setCode(newCode)

		if (value && index < 5) {
			inputsRef.current[index + 1]?.focus()
		}
	}

	const handleKeyDown = (e: React.KeyboardEvent, index: number) => {
		if (e.key === 'Backspace' && !code[index] && index > 0) {
			inputsRef.current[index - 1]?.focus()
		}
	}

	return (
		<motion.div
			key='confirm'
			initial={{ x: 300, opacity: 0 }}
			animate={{ x: 0, opacity: 1 }}
			exit={{ x: -300, opacity: 0 }}
			transition={{ duration: 0.4 }}
		>
			<form onSubmit={e => handleClick(e)}>
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
					className='text-center'
					style={{
						fontWeight: '500',
						fontSize: 26,
						marginBottom: '0.5rem',
					}}
				>
					Enter the code
				</div>
				<div
					className='text-center text-sm text-white'
					style={{
						opacity: 0.6,
						marginBottom: '1.5rem',
					}}
				>
					Code was sent to <span className='font-semibold'>{email}</span>
				</div>
				<div className='flex justify-center gap-3 mb-8'>
					{code.map((digit, idx) => (
						<input
							key={idx}
							ref={el => (inputsRef.current[idx] = el)}
							type='text'
							inputMode='numeric'
							maxLength={1}
							value={digit}
							onChange={e => handleChange(e.target.value, idx)}
							onKeyDown={e => handleKeyDown(e, idx)}
							className='w-12 h-14 text-center text-xl border border-border rounded-md bg-black text-white outline-none focus:ring-2 focus:ring-blue-500 transition-all'
						/>
					))}
				</div>

				<div
					className='text-center rounded-lg'
					style={{
						background: 'linear-gradient(135deg, #0174dc, #0074db)',
						display: 'flex',
						justifyContent: 'center',
						alignItems: 'center',
						marginBottom: '1.5rem',
					}}
				>
					<button type='submit' style={{ width: '80%', padding: '0.6rem 0' }}>
						Confirm
					</button>
				</div>

				<div
					className='text-center text-sm'
					style={{
						opacity: 0.6,
						marginTop: '2rem',
					}}
				>
					Wrong email?{' '}
					<button
						type='button'
						onClick={onClick}
						className='text-white underline'
					>
						Go back
					</button>
				</div>
			</form>
		</motion.div>
	)
}
