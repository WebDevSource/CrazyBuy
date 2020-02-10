var Announcement = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "announcement", Announcement.InitModule);
    },


    InitModule() {
        NavBar.Init();
        Announcement.InitView();
    },

	InitView() {
	
    }
};

window.onload = Announcement.doLoad;