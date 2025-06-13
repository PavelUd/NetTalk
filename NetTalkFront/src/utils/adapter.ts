import { IMessage } from '@/models/message'
import { IUser } from '@/models/user'

export const adaptToMessage = async (
	id: string,
	idUser: string,
	text: string,
	createdDate: string,
	updatedDate: string,
	sender?: IUser
) => {
	const message = {
		id: id,
		idUser: idUser,
		text: text,
		createdDate: createdDate,
		updatedDate: updatedDate,
		sender: sender,
	} as IMessage
	return message
}

export const adaptToUser = async (
	id: string,
	username: string,
	email: string,
	avatar: string
) => {
	const user = {
		id: id,
		username: username,
		email: email,
		avatar: avatar,
	} as IUser

	return user
}
