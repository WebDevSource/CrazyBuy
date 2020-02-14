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
		//Products.InitProductData();
	},

	getProductData(id) {

		Utils.ProcessAjax("/api/prd/getPrdByCatId/"+id, "GET", true, "",
			function (ret) {
				Products.initProductList(ret.data);
			}
		);

	},

	initProductList(data) {
		let role = Utils.getRole();
		let pcHtml = '';
		let mobileHtml = '';
		for (let item in data) {
			pcHtml += ProductCard.getHtml(data[item], role);
			mobileHtml += ProductItem.getHtml(data[item], role);
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
					Products.getProductData(ret.data[0].id);
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
		let html = '<div  onClick="Products.getProductData(\''+item.id +'\')" class="nav-link category" data-toggle="collapse" data-value="' + item.id + '" data-toggle="collapse" data-target="#' + collapseId + '">'
			+ item.name
			+ ((role != Utils.ROLE_GUEST) ? '<span data-userAuthority="member" > (1980)</span > ' : '')
			+ '</div>'
			+ '  <div id="' + collapseId + '" class="collapse" data-parent="#' + (item.parentId ? collapseId: rootId) + '">'
			+ '</div> ';

		return html;
	}




};

window.onload = Products.doLoad;