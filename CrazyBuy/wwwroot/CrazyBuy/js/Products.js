var Products = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "products", Products.InitModule);
    },


    InitModule() {
        NavBar.Init();
        Products.InitView();
    },

	InitView() {
		Products.InitProductData();
	},

	InitProductData() {

		Utils.ProcessAjax("/api/prd/getHomePrdList", "GET", true, "",
			function (ret) {
				Products.initProductList(ret.data);
			},
			function (error) { alert("tenant error") }
		);

	},

	initProductList(data) {
		let role = Utils.getRole();
		let html = '';
		for (let item in data) {
			html += ProductCard.getHtml(data[item], role);
		}
		$('.items-card-row').html(html);
	}

};

window.onload = Products.doLoad;