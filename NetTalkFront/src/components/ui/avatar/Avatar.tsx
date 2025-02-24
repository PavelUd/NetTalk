const Avatar = ({ name, size = 40, bgColor = '#ccc', textColor = '#fff' }) => {
	// Получаем первую букву имени
	const initial = name ? name[0].toUpperCase() : ''

	return (
		<div
			style={{
				width: size,
				height: size - 5,
				borderColor: 'transparent',
				borderRadius: '50%', // Делаем круг
				backgroundColor: bgColor, // Цвет фона
				color: textColor, // Цвет текста
				display: 'flex',
				alignItems: 'center',
				justifyContent: 'center',
				fontSize: size * 0.5, // Размер текста зависит от размера аватара
				fontWeight: 'bold',
				fontFamily: 'Arial, sans-serif',
			}}
		>
			{initial}
		</div>
	)
}

export default Avatar
