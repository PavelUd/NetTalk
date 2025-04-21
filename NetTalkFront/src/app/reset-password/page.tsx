'use client'

import { AnimatePresence, motion } from 'framer-motion'
import { useState } from 'react'

export default function RegisterPage() {
	const [step, setStep] = useState(0)
	return (
		<div className='min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-200 to-purple-300'>
			<div className='relative w-[400px] h-[300px] bg-white rounded-2xl shadow-xl overflow-hidden p-6'>
				<AnimatePresence mode='wait'>
					{step === 0 ? (
						<motion.div
							key='form1'
							initial={{ x: 600, opacity: 0 }}
							animate={{ x: 0, opacity: 1 }}
							exit={{ x: -300, opacity: 0 }}
							transition={{ duration: 0.4 }}
							className='absolute w-full h-full'
						>
							<h2 className='text-xl font-bold mb-4'>Форма 1</h2>
							<input
								type='text'
								placeholder='Введите имя'
								className='w-full p-2 border rounded mb-4'
							/>
							<button
								onClick={() => setStep(1)}
								className='bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600'
							>
								Далее →
							</button>
						</motion.div>
					) : (
						<motion.div
							key='form2'
							initial={{ x: 300, opacity: 0 }}
							animate={{ x: 100, opacity: 1 }}
							exit={{ x: -300, opacity: 0 }}
							transition={{ duration: 0.4 }}
							className='absolute w-full h-full'
						>
							<h2 className='text-xl font-bold mb-4'>Форма 2</h2>
							<input
								type='email'
								placeholder='Введите email'
								className='w-full p-2 border rounded mb-4'
							/>
							<button
								onClick={() => setStep(0)}
								className='bg-purple-500 text-white px-4 py-2 rounded hover:bg-purple-600'
							>
								← Назад
							</button>
						</motion.div>
					)}
				</AnimatePresence>
			</div>
		</div>
	)
}
