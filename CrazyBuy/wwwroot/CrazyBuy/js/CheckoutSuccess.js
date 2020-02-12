var CheckoutSuccess = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "checkoutSuccess", CheckoutSuccess.InitModule);
    },


    InitModule() {
        NavBar.Init();
        CheckoutSuccess.InitView();
    },

	InitView() {
	
    }
};

window.onload = CheckoutSuccess.doLoad;