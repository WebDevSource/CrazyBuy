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
		let role = Utils.getRole();
		let data = Products.getProduct();
		let html = '';
		for (let item in data) {
			html += ProductCard.getHtml(data[item], role);
		}
		$('.items-card-row').html(html);
	},

	getProduct() {
		return [
			{
				"prices": { "會員價": "$160", "一般價": "$170" },
				"title": "富貴團圓年菜9件組(12/10)",
				"url": "./images/item-1.png",
				"id": 1,
				"tags":["冷藏", "獨立下單", "特殊運件"]
			},
			{
				"prices": { "會員價": "$160", "一般價": "$170" },
				"title": "富貴團圓年菜9件組(12/10)",
				"url": "./images/item-1.png",
				"id": 2,

				"tags":["冷凍", "買一件免運"]
			},
			{
				"prices": { "會員價": "$160", "一般價": "$170" },
				"title": "富貴團圓年菜9件組(12/10)",
				"url": "./images/item-1.png",
				"id": 3,
				"tags":["冷藏", "獨立下單", "特殊運件"]
			},
			{
				"prices": { "會員價": "$160", "一般價": "$170" },
				"title": "富貴團圓年菜9件組(12/10)",
				"url": "./images/item-1.png",
				"id": 4,
				"tags":["冷藏", "獨立下單", "特殊運件"]
			},
			{
				"prices": { "會員價": "$160", "一般價": "$170" },
				"title": "富貴團圓年菜9件組(12/10)",
				"url": "./images/item-1.png",
				"id": 5
			},
			{
				"prices": { "會員價": "$160", "一般價": "$170" },
				"title": "富貴團圓年菜9件組(12/10)",
				"url": "./images/item-1.png",
				"id": 6
			},
			{
				"prices": { "會員價": "$160", "一般價": "$170" },
				"title": "富貴團圓年菜9件組(12/10)",
				"url": "./images/item-1.png",
				"id": 7
			},
			{
				"prices": { "會員價": "$160", "一般價": "$170" },
				"title": "富貴團圓年菜9件組(12/10)",
				"url": "./images/item-1.png",
				"id": 8
			},
			{
				"prices": { "會員價": "$160", "一般價": "$170" },
				"title": "富貴團圓年菜9件組(12/10)",
				"url": "./images/item-1.png",
				"id": 9
			}
		];
	}


};

window.onload = Products.doLoad;