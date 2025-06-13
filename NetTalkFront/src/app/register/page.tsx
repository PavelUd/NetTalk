'use client'
import { Confirm } from '@/features/auth/ui/ConfirmPage'
import { Register } from '@/features/auth/ui/RegisterPage'
import { AnimatePresence } from 'framer-motion'
import { useState } from 'react'

export default function Home() {
	const [step, setStep] = useState(0)

	return (
		<div className='flex w-screen h-screen items-center justify-center bg-[#0f0f10]'>
			<div
				className='border-t border-border p-7 rounded-xl shadow-[15px_15px_45px_#010101] relative overflow-hidden'
				style={{ background: '#18181c', width: '450px', minHeight: '300px' }}
			>
				<AnimatePresence mode='wait'>
					{step === 0 ? (
						<Register key='register' onClick={() => setStep(1)} />
					) : (
						<Confirm key='confirm' onClick={() => setStep(0)} />
					)}
				</AnimatePresence>
			</div>
		</div>
	)
}
