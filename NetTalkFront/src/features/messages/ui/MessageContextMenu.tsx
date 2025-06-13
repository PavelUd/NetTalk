import { useChatStore } from '@/features/chats/stores/chatStore'
import { useUpdateMessageStore } from '@/features/sendMessage/model/updateMessageStorage'
import { Item, ItemParams, Menu, useContextMenu } from 'react-contexify'
import 'react-contexify/dist/ReactContexify.css'
import { FaRegTrashCan } from 'react-icons/fa6'
import { MdOutlineModeEditOutline } from 'react-icons/md'
export const MESSAGE_MENU_ID = 'message_menu'

export function MessageContextMenu() {
	const { hideAll } = useContextMenu({ id: MESSAGE_MENU_ID })

	const { setUpdateMessage } = useUpdateMessageStore()
	const { currentChatId, deleteMessage } = useChatStore()

	const handleEdit = ({ props }: ItemParams) => {
		console.log('Edit message:', props)
		setUpdateMessage({
			idMessage: props.id,
			text: props.message,
			idChat: currentChatId,
		})
	}

	const handleDelete = ({ props }: ItemParams) => {
		console.log('Delete message:', props.id)
		deleteMessage(currentChatId, props.id)
		hideAll(MESSAGE_MENU_ID)
	}

	return (
		<Menu id={MESSAGE_MENU_ID} theme='dark' animation='fade'>
			<Item onClick={handleEdit}>
				<MdOutlineModeEditOutline className='me-2' /> Редактировать
			</Item>
			<Item onClick={handleDelete} className='react-contexify__item--delete'>
				<FaRegTrashCan className='me-2' /> Удалить
			</Item>
		</Menu>
	)
}
function hide(MESSAGE_MENU_ID: string) {
	throw new Error('Function not implemented.')
}
