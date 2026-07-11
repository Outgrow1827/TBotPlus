window.tbotplusLocale = {
	key: "tbotplus_locale",
	get: function () {
		return window.localStorage.getItem(this.key);
	},
	set: function (code) {
		window.localStorage.setItem(this.key, code);
	}
};
