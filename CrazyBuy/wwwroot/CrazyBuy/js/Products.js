var Products = {

	catalogs: {},

	doLoad() {
		Utils.Initial();
		Utils.InitI18next("zh-TW", "products", Products.InitModule);
	},


	InitModule() {
		NavBar.Init();
		Products.InitView();
	},

	InitView() {
		Products.InitCatalogData();
		Products.InitProductData();
	},

	InitProductData() {

		Utils.ProcessAjax("/api/prd/getHomePrdList", "GET", true, "",
			function (ret) {
				Products.initProductList(ret.data);
			}
		);

	},

	initProductList(data) {
		let role = Utils.getRole();
		let html = '';
		for (let item in data) {
			html += ProductCard.getHtml(data[item], role);
		}
		$('.items-card-row').html(html);
	},

	InitCatalogData() {

		Utils.ProcessAjax("/api/prd/getPrdCats", "GET", true, "",
			function (ret) {
				if (ret.code == 1) {
					Products.catalogs = ret.data
					let parents = new Map();
					for (let i = 0; i < ret.data.length; i++) {
						let item = ret.data[i];
						parents.set(item.parentId, item);
					}
					Products.InitCatalogList("pc-");
				}
			}
		);

	},

	InitCatalogList(driver) {
		let role = Utils.getRole();
		let html = '';
		for (let i = 0; i < Products.catalogs.length; i++) {
			let item = Products.catalogs[i];
			html = Products.getCatalogHtml(item, role, "category-accordion", driver);
			if (item.parentId) {
				$("#" +driver+ item.parentId).append(html);
			} else {
				$("#category-accordion").append(html);
			}
		}

	},

	getCatalogHtml(item, role, rootId, driver) {
		let collapseId = driver + item.id;
		let html = '<a  href="javascript" class="nav-link category" data-toggle="collapse" data-value="' + item.id + '" data-toggle="collapse" data-target="#' + collapseId + '">'
			+ item.name
			+ ((role != Utils.ROLE_GUEST) ? '<span data-userAuthority="member" > (1980)</span > ' : '')
			+ '</a>'
			+ '  <div id="' + collapseId + '" class="collapse" data-parent="#' + (item.parentId ? collapseId: rootId) + '">'
			+ '</div> ';

		return html;
	}




};

window.onload = Products.doLoad;