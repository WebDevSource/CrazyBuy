var Products = {

	catalogs: {},
	sortType: 1,
	currentCat: 0,
	pageIndex: 1,

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
		//Products.InitProductData();
	},

	getProductData() {
		let data = {
			"sortType": parseInt(Products.sortType),
			"count": 12,
			"page": parseInt(Products.pageIndex)
		}
		let url = "/api/prd/getPrdByCatId/" + Products.currentCat;

		Utils.ProcessAjax(url, "GET", true, data,
			function (ret) {
				Products.initProductList(ret.data);
			}
		);

	},

	initProductList(data) {
		let role = Utils.getRole();
		let pcHtml = '';
		let mobileHtml = '';
		let items = data.result;
		for (let item in items) {
			pcHtml += ProductCard.getHtml(items[item], role);
			mobileHtml += ProductItem.getHtml(items[item], role);
		}
		$('.items-card-row').html(pcHtml);
		$("#products-row").html(mobileHtml);
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
					Products.InitCatalogList("pc-", "category-accordion");
					Products.InitCatalogList("m-", "category-accordion-m");
					Products.currentCat = ret.data[0].id;
					Products.getProductData(1);
				}
			}
		);

	},

	InitCatalogList(driver, rootId) {
		let role = Utils.getRole();
		let html = '';
		for (let i = 0; i < Products.catalogs.length; i++) {

			let item = Products.catalogs[i];
			let catalogId = item.parentId ? driver + item.parentId : rootId;
			html = Products.getCatalogHtml(item, role, rootId, driver);
			/*			if (item.parentId) {
							$("#" +driver+ item.parentId).append(html);
						} else {
							$("#" + rootId).append(html);
						}
			*/
			$("#" + catalogId).append(html);
		}

	},

	getCatalogHtml(item, role, rootId, driver) {
		let collapseId = driver + item.id;
		let html = '<div  style="cursor:pointer;" onClick="Products.getCatalogProducts(this)" class="nav-link category" data-toggle="collapse" data-value="' + item.id + '" data-toggle="collapse" data-target="#' + collapseId + '">'
			+ item.name
			+ ((role != Utils.ROLE_GUEST) ? '<span data-userAuthority="member" >(' + item.count + ')</span > ' : '')
			+ '</div>'
			+ '  <div id="' + collapseId + '" class="collapse" data-parent="#' + (item.parentId ? collapseId : rootId) + '">'
			+ '</div> ';

		return html;
	},

	getCatalogProducts(me) {
		let id = $(me).attr("data-value");
		Products.currentCat = id;
		Products.pageIndex = 1;
		Products.getProductData();
	},

	sortData(type) {
		Products.sortType = type;
		Products.getProductData();
	},


};

window.onload = Products.doLoad;