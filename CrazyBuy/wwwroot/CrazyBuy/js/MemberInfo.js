var MemberInfo = {
    doLoad() {
        Utils.Initial();

		Utils.InitI18next("zh-TW", "memberInfo", MemberInfo.InitModule);
    },


    InitModule() {
        NavBar.Init();
		MemberInfo.InitView();
    },

	InitView() {
		
    },

   
};

window.onload = MemberInfo.doLoad;