var Cart = {
    doLoad() {
        Utils.Initial();

        Utils.InitI18next("zh-TW", "cart", Cart.InitModule);
    },


    InitModule() {
        NavBar.Init();
        Cart.InitView();
    },

	InitView() {
		
    },

   
};

window.onload = Cart.doLoad;