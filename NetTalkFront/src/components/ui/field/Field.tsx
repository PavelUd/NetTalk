import cn from 'clsx'
import { forwardRef } from 'react'

import styles from './field.module.scss'
import { TypeInputProps } from './field.types'

const Field = forwardRef<HTMLInputElement, TypeInputProps>(
	({ error, style, Icon, className, ...rest }, ref) => {
		return (
			<label className={cn(styles.field, className)} style={style}>
				{Icon && (
					<div className={styles.icon}>
						<Icon />
					</div>
				)}
				<input className='w-full' ref={ref} {...rest} />
				{error && <div className={styles.error}>{error}</div>}
			</label>
		)
	}
)

Field.displayName = 'Field'

export default Field
