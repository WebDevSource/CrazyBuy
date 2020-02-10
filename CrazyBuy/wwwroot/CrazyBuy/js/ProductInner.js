var ProductInner = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "products", ProductInner.InitModule);
    },


    InitModule() {
        NavBar.Init();
        ProductInner.InitView();
    },

	InitView() {
	
    }
};

window.onload = ProductInner.doLoad;