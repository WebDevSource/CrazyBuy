var Register = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "register", Register.InitModule);
    },


    InitModule() {
        NavBar.Init();
        Register.InitView();
    },

	InitView() {
	
    }
};

window.onload = Register.doLoad;