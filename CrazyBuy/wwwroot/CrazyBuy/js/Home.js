var Home = {
    doLoad() {
        Utils.Initial();

        Utils.InitI18next("zh-TW", "home", Home.InitModule);
    },


    InitModule() {
        NavBar.Init();
        Home.InitView();
    },

	InitView() {
		Home.InitProductData();
    },

	InitProductData(role) {
		
		Utils.ProcessAjax("/api/prd/getHomePrdList", "GET", true, "",
			function (ret) {
				Home.initProductList(ret.data);
			},
			function (error) {alert("tenant error") }
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

window.onload = Home.doLoad;