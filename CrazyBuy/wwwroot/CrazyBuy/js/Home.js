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
		let role = Utils.getRole();
		let data = Home.getProduct();
		let html = '';
		for (let item in data) {
			html += ProductCard.getHtml(data[item],role);
		}
		$('.items-card-row').html(html);
    },

	getProduct() {
		
		//Utils.ProcessAjax("/api/prd/getHomePrdList/FE883257-2183-4562-8F73-DDD7B4F27F1E", "GET", true, null, function (ret) { console.log("suuccess", ret) }, function (error) { console.log("error", error) })
		return [
		/*	{
				"prices": { "會員價": "$160", "一般價": "$170" },
				"id": "80ff9a2e-6a95-413a-8cca-adfb11e99d7b",
				"name": "富士山蘋果",
				"prdCode": "20200202235553",
				"tenantId": "fe883257-2183-4562-8f73-ddd7b4f27f1e",
				"summary": "好吃的富士山蘋果",
				"prdImages": "{\"fileName\":\"apple.jpg\",\"type\":\"jpg\"}"
				
				"tags": ["冷藏", "獨立下單", "特殊運件"]
			}

*/

			{
				"prices": { "會員價": "$160", "一般價": "$170" },
				"title": "富貴團圓年菜9件組(12/10)",
				"url": "./images/item-1.png",
				"id": 1,
				"tags": ["冷藏", "獨立下單", "特殊運件"]
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
				"id": 5,
				"tags": ["買一件免運費喔", "獨立下單", "特殊運件"]
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
			}
		];
    }


};

window.onload = Home.doLoad;