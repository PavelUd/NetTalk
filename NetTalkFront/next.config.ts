const nextConfig = {
	env: {
		API_URL: process.env.API_URL,
	},
	compiler: {
		removeConsole: false,
	},
}

module.exports = {
	images: {
		domains: ['mdbcdn.b-cdn.net'],
	},
}
