var NavBar = {
    Init() {
        let role = Utils.getRole();
        NavBar.InitHeaderButtons()
        NavBar.InitToolButtons(role)
        $('[data-userauthority]').hide();
        $('[data-authority]').hide();
        $('[data-userauthority="' + role + '"]').show()
        $('[data-authority="' + role + '"]').show();
    },

  InitHeaderButtons() {
        let html = "";
        for (let key in Utils.navbarBotton) {
            html += '<li class="nav-item ">'
                + '<a class="nav-link " href = "' + Utils.navbarBotton[key] + '" > ' + i18next.t(key) + '</a ></li > ';
        }
        $(".navbar-nav").html(html);
    },

    InitToolButtons(role) {
        let html = '';
        switch (role) {
            case Utils.ROLE_GUEST:
                NavBar.InitLoginModel();
                html = NavBar.getGuestToolButton();
                break;
            case Utils.ROLE_MEMBER:
                html = NavBar.getMemberToolButton();
                break;
            case Utils.ROLE_ADMIN:
                html = NavBar.getAdminToolButton();
                break;
        }
        $('.navbar-status-content').html(html);
    },

    getGuestToolButton() {
        let html ='<div id="tourists-viewport" data-authority="guest">                 '
            + '  <button class="register-link register-modal-link border-0" type="button">'
            + '    登入會員/註冊                                                          '
            + '  </button>                                                                '
            + '</div>                                                                     ';
        return html;
    },

    InitLoginModel() {
        let html ='<div class="modal" id="register-modal" tabindex="-1" role="dialog"  aria-hidden="true">           '
            + '  <div class="modal-dialog modal-dialog-centered" role="document">                                '
            + '    <div class="modal-content">                                                                   '
            + '      <div class="modal-header justify-content-center border-0 mb-20">                            '
            + '        <h5 class="modal-title text-center" id="register-modal-title">登入會員</h5>               '
            + '      </div>                                                                                      '
            + '      <form class="col-11 mx-auto modal-body">                                                    '
            + '        <div class="form-group row">                                                              '
            + '          <label for="accountant" class="col-sm-2 col-2 col-form-label text-nowrap">帳號</label>  '
            + '          <div class="col-8">                                                                     '
            + '            <input type="accontant" class="form-control rounded-0" id="accountant">               '
            + '          </div>                                                                                  '
            + '        </div>                                                                                    '
            + '        <div class="form-group row">                                                              '
            + '          <label for="password" class="col-sm-2 col-2 col-form-label text-nowrap">密碼</label>    '
            + '          <div class="col-8">                                                                     '
            + '            <input type="password" class="form-control rounded-0" id="password">                  '
            + '          </div>                                                                                  '
           //old 忘記密碼
 /*           + '          <div class="d-flex pl-0 col-2">                                                         '
            + '            <button id="forget-password-btn" class="text-nowrap" type="button" onclick="NavBar.switchLoginModelStatus(false)">                   '
            + '              忘記密碼                                                                            '
            + '            </button>                                                                             '
            + '          </div>'
 */           //new忘記密碼
            + '          </div> '
            + '         <div class="form-group row" > '
            + '            <button id="forget-password-btn" class="text-nowrap" type="button" onclick="NavBar.switchLoginModelStatus(false)">                   '
            + '              忘記密碼                                                                            '
            + '            </button>                                                                             '
            + '          </div>      '                                                                           
            + '      </form>                                                                                     '
            + '                                                                                                  '
            + '      <div class="modal-footer justify-content-center border-0">                                  '
            + '        <a href="./register.html" class="btn btn-outline-register register-btn-size">註冊會員</a>                '
            + '        <button class="btn btn-register btn-signin register-btn-size" onclick="NavBar.DoLogin()">登入</button>               '
            + '      </div>                                                                                      '
            + '    </div>                                                                                        '
            + '  </div>                                                                                          '
            + '</div>                                                                                            '
            + '<div class="modal" id="forget-password-modal" tabindex="-1" role="dialog" style="display: none;" aria-hidden="true">                   '
            + '  <div class="modal-dialog modal-dialog-centered" role="document">                                                                     '
            + '    <div class="modal-content">                                                                                                        '
            + '      <div class="modal-header justify-content-center border-0 mb-20">                                                                 '
            + '        <h5 class="modal-title text-center" id="register-modal-title">忘記密碼</h5>                                                    '
            + '      </div>                                                                                                                           '
            + '      <form class="col-11 mx-auto modal-body">                                                                                         '
            + '        <label for="exist-accountant" class="col-12 col-form-label pl-0">輸入會員帳號</label>                                          '
            + '        <div class="form-group row">                                                                                                   '
            + '          <div class="col-9">                                                                                                          '
            + '            <input type="text" class="form-control" id="exist-accountant">                                                             '
            + '          </div>                                                                                                                       '
            + '          <div class="col-3 px-0">                                                                                                     '
            + '            <button class="w-100 btn btn-register btn-signin register-btn-size">送出</button>                                          '
            + '          </div>                                                                                                                       '
            + '        </div>                                                                                                                         '
            + '      </form>                                                                                                                          '
            + '                                                                                                                                       '
            + '      <div class="modal-footer justify-content-center border-0">                                                                       '
            + '        <button class="register-modal-link btn btn-outline-register register-btn-size">回登入畫面</button>                             '
            + '      </div>                                                                                                                           '
            + '    </div>                                                                                                                             '
            + '  </div>                                                                                                                               '
            + '</div>                                                                                                                                 ';

        $('body').prepend(html);
        $("body").on("click", '.register-modal-link',function () {
            NavBar.switchLoginModelStatus(true);
        });

    },

    switchLoginModelStatus(show) {
        if (show) {
            event.stopPropagation();
            $("#register-modal").modal('show');
            $('#forget-password-modal').modal('hide');
        } else{
            event.stopPropagation();
            $('#forget-password-modal').modal('show');
            $("#register-modal").modal('hide');
        }
    },


     getMemberToolButton() {
         let html =' <div id="member-viewport" data-authority="member">'
            + '  <nav class="nav">'
            + '    <div class="btn-group nav-cart-group"> '
            + '      <a  class="nav-link pl-0" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">'
            + '        <i class="fas fa-shopping-cart"></i> 購物車(2)'
            + '      </a>'
            + '      <div class="dropdown-menu rounded-0 nav-cart-items px-2"> '
            + '       <div class="row mx-0 nav-cart-item py-2">'
            + '         <div class="col-sm-3 col-2 px-0 d-flex align-items-center">                                                     '
            + '           <div class="navs-cart-item-bg" style="background-image: url(\'./images/item-1.png\');"></div>'
            + '         </div>'
            + '         <div class="col-sm-8 col-10 nav-cart-item-info"> '
            + '           <div class="d-flex flex-wrap w-100"> '
            + '            <p class="mb-1 w-100">南門市場60年老店上海火腿湖南臘肉300g</p> '
            + '            <p class="mb-0 w-100 nav-cart-item-price">1 <i class="fas fa-times"></i> <span class="price">$160</span></p> '
            + '           </div>'
            + '         </div> '
            + '         <div class="nav-cart-close">x</div> '
            + '       </div>'
            + '       <div class="row mx-0 nav-cart-item py-2"> '
            + '         <div class="col-sm-3 col-2 px-0 d-flex align-items-center"> '
            + '           <div class="navs-cart-item-bg" style="background-image: url(\'./images/item-2.png\');"></div>'
            + '         </div>'
            + '         <div class="col-sm-8 col-10 nav-cart-item-info">'
            + '           <div class="d-flex flex-wrap w-100">'
            + '            <p class="mb-1 w-100">眷村私房菜老滷湯酸白菜鍋1000g(五～七人份) </p>'
            + '            <p class="mb-0 w-100 nav-cart-item-price">1 <i class="fas fa-times"></i> <span class="price">$160</span></p> '
            + '           </div> '
            + '         </div>'
            + '         <div class="nav-cart-close">x</div> '
            + '       </div>'
            + '       <div class="ml-auto my-2 col-6">'
            + '         <div class="row"> '
            + '           <div class="col-6 nav-items-num">小計</div> '
            + '           <div class="col-sm-6 col-12 text-right">'
            + '            <span class="nav-items-num-mobile">小計</span> '
            + '            <span class="price">$320</span> '
            + '           </div>'
            + '         </div>'
            + '       </div> '
            + '       <div class="ml-auto my-2 col-6">'
            + '         <div class="row">'
            + '           <div class="col-sm-6 col-4 nav-cart-items-clear-All">清空</div>'
             + '           <div class="col-sm-6 col-4 px-0"><a class="btn btn-register navs-btn-checkout" href="./cart.html">結帳</a></div>'
            + '         </div>'
            + '       </div>'
            + '      </div>'
            + '    </div>'
            + NavBar.getUserGroupButton()                                                                                                       
            + '  </nav>                                                                                                                 '
            + '</div>                                                                                                                   '
        return html;
    },
     
  getAdminToolButton() {
        let html = '<div id="admin-viewport" data-authority="admin">'
            + '     <nav class="nav">'
            + '     	<a class="nav-link pl-0" href="#">'
            + '	            <i class="fas fa-rss"></i> 我訂閱的商店'
            + '     	</a>                                                                                    '
            + NavBar.getUserGroupButton()
            + '     </nav>'
            + '</div>';
        return html;
    },

    getUserGroupButton() {
        let html = '<div class="btn-group">                                                                 '
            + '	<a class="nav-link" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">'
            + '		<i class="far fa-user"></i> Helena <i class="fas fa-angle-down"></i>'
            + '	</a>'
            + '	<div class="dropdown-menu dropdown-menu-right rounded-0"> '
            + '		<a class="nav-link" href="./order.html">查詢我的訂單</a> '
            + '		<a class="nav-link" href="./member-info.html">維護會員資料</a> '
            + '     <a class="nav-link" href="javascript:NavBar.doLogout()">登出</a>'  
            + '	</div>'
            + '</div>';
        return html;
    },

    DoLogin() {
        let accountant = $("#accountant").val();
        let password = $("#password").val();
 /*       let data = {
            "account": accountant,
            "pwd": password
        };

        let result = Utils.AsyncProcessAjax("/api/auth/Login", "Post", "", data);

        if (result.code == 1) {
            if (Utils.ROLE_GUEST == result.type) {
                alert("User Authorization Error, check account or password Please.");
            } else {
 */               if (accountant.toLowerCase() == "admin") {
                    Utils.SetCookie("role", Utils.ROLE_ADMIN);
                } else {
                    Utils.SetCookie("role", Utils.ROLE_MEMBER);
                }
                window.location.reload();
 /*           }
           
        } else {
            alert("User Authorization Error, check account or password Please.");
        }
 */       
      
    },

    doLogout() {
        Utils.SetCookie("role", Utils.ROLE_GUEST);
        window.location.reload();
    }




};