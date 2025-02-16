import { LucideIcon } from 'lucide-react'
import { InputHTMLAttributes } from 'react'

export interface IFieldProps {
	placeholder: string
	error?: string
	Icon?: LucideIcon
}

export type TypeInputProps = InputHTMLAttributes<HTMLInputElement> & IFieldProps
