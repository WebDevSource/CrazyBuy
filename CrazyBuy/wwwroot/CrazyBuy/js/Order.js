var Order = {
    doLoad() {
        Utils.Initial();

        Utils.InitI18next("zh-TW", "order", Order.InitModule);
    },


    InitModule() {
        NavBar.Init();
        Order.InitView();
    },

	InitView() {
		
    },

   
};

window.onload = Order.doLoad;