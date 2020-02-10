var Questions = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "questions", Questions.InitModule);
    },


    InitModule() {
        NavBar.Init();
        Questions.InitView();
    },

	InitView() {
	
    }
};

window.onload = Questions.doLoad;