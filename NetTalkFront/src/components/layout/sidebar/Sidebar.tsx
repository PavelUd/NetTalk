'use client'

import Image from 'next/image'
import { BiSolidMessageMinus } from 'react-icons/bi'
import { BsBookmarkDashFill } from 'react-icons/bs'
import { CiLogout } from 'react-icons/ci'
import { IoIosCall } from 'react-icons/io'
import { RiGroupFill, RiSettingsLine } from 'react-icons/ri'
import styles from './Sidebar.module.scss'

export function Sidebar() {
	return (
		<aside className={styles.sidebar}>
			<div>
				<div className='p-1'>
					<Image
						className='mb-7 opacity-20 scale-100 hover:opacity-100 hover:scale-110 transition-all duration-500'
						src='/logo.svg'
						priority
						alt=''
						width={40}
						height={40}
					/>
					<Image
						src='https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava1-bg.webp'
						priority
						alt=''
						className=''
						width={45}
						height={45}
					/>
				</div>
				<div className='border-b-2 border-[#2a2931] p-4 mt-4 mb-4'>
					<BiSolidMessageMinus
						size={24}
						style={{ transform: 'scaleX(-1)', marginBottom: '1.5rem' }}
					/>
					<BsBookmarkDashFill
						size={22}
						style={{
							marginBottom: '1rem',
						}}
					/>
				</div>
				<div className='border-b-2 border-[#2a2931] p-4 mt-4 mb-4'>
					<RiGroupFill
						size={24}
						style={{
							marginBottom: '1rem',
						}}
					/>
				</div>
				<div className='border-b-2 border-[#2a2931] p-4 mt-4 mb-4'>
					<IoIosCall size={27} style={{ marginBottom: '1rem' }} />
				</div>
				<div className='p-4 mt-4 mb-4'>
					<RiSettingsLine
						size={25}
						style={{ transform: 'scaleX(-1)', marginBottom: '1rem' }}
					/>
					<div className='relative group'>
						<CiLogout
							size={25}
							className='transition-colors duration-300 group-hover:text-red-500'
						/>
					</div>
				</div>
			</div>
		</aside>
	)
}
