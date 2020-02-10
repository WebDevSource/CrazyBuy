var OrderDetail = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "orderDetail", OrderDetail.InitModule);
    },


    InitModule() {
        NavBar.Init();
        OrderDetail.InitView();
    },

	InitView() {
	
    }
};

window.onload = OrderDetail.doLoad;