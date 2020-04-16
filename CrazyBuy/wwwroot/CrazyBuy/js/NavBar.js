var NavBar = {
    navbarBotton: { "btn_nav_home": "./index.html", "btn_nav_product": "./products.html", "btn_nav_statement": "./announcement.html", "btn_nav_FQA": "questions.html" },

    Init() {
        let role = Utils.getRole();
        NavBar.InitHeaderButtons();
        NavBar.InitToolButtons(role);
        $('[data-userauthority]').hide();
        $('[data-authority]').hide();
        $('[data-userauthority="' + role + '"]').show()
        $('[data-authority="' + role + '"]').show();
    },

    InitHeaderButtons() {
        let html = "";
        for (let key in NavBar.navbarBotton) {
            // let tenantId = Utils.GetUrlParameter("tenantId");

            // let url = NavBar.navbarBotton[key] + "?tenantId=" + tenantId;
            html += '<li class="nav-item ">'
                + '<a class="nav-link " href = "' + NavBar.navbarBotton[key] + '" > ' + i18next.t(key) + '</a ></li > ';
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
                NavBar.getCartData();
                break;
            case Utils.ROLE_ADMIN:
                html = NavBar.getAdminToolButton();
                break;
        }
        $('.navbar-status-content').html(html);
    },

    getGuestToolButton() {
        let html = '<div id="tourists-viewport" data-authority="guest">                 '
            + '  <button class="register-link register-modal-link border-0" type="button">'
            + '    登入會員/註冊                                                          '
            + '  </button>                                                                '
            + '</div>                                                                     ';
        return html;
    },

    InitLoginModel() {
        let html = '<div class="alert alert-primary fade" role="alert">'
            + '  A simple primary alert with <a href="#" class="alert-link">an example link</a>. Give it a click if you like.'
            + '</div>'
            + '<div class="modal" id="register-modal" tabindex="-1" role="dialog"  aria-hidden="true">           '
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
        $("body").on("click", '.register-modal-link', function () {
            NavBar.switchLoginModelStatus(true);
        });

    },

    switchLoginModelStatus(show) {
        if (show) {
            event.stopPropagation();
            $("#register-modal").modal('show');
            $('#forget-password-modal').modal('hide');
        } else {
            event.stopPropagation();
            $('#forget-password-modal').modal('show');
            $("#register-modal").modal('hide');
        }
    },


    getMemberToolButton() {
        let count = 0;
        let html = ' <div id="member-viewport" data-authority="member">'
            + '  <nav class="nav">'
            + '    <div class="btn-group nav-cart-group"> '
            + '    </div>'
            + NavBar.getUserGroupButton()
            + '  </nav>                                                                                                                 '
            + '</div>                                                                                                                   '
        return html;
    },
    getCartData() {
        Utils.ProcessAjax("/api/ShopCart", "GET", true, "",
            function (ret) {
                let html = '';
                let amount = 0;
                for (let i = 0; i < ret.data.length; i++) {
                    let item = ret.data[i];
                    html += NavBar.getCartHtml(item);
                    amount += item.amount;
                }
                let data = {
                    cartList: html,
                    amount: amount,
                    count: ret.data.length
                }


                NavBar.RefreshCart(data);

            },
            function (error) {
                //alert("tenant error [Cart]")
                alert(i18next.t("msg_service_error"));
            }
        );

    },

    getCartHtml(data) {
        let url = "./product-inner.html?id=" + data.productId;
        let imageUrl = "./images/noitem.jpg";
        if (data.prdImages) {
            let images = data.prdImages ? JSON.parse(data.prdImages) : "";
            imageUrl = Utils.BackendImageUrl + "id=" + data.productId + "&filename=" + images[0].filename;
        }
        let html = '<div class="row mx-0 nav-cart-item py-2">'
            + ' <a href="' + url + '" class="col-sm-3 col-2 px-0 d-flex align-items-center">'
            + '   <div class="navs-cart-item-bg" style="background-image: url(' + imageUrl + ');"></div>'
            + ' </a> '
            + ' <a  href="' + url + '" class="col-sm-8 col-10 nav-cart-item-info nav-link category"> '
            + '   <div class="d-flex flex-wrap w-100">'
            + '    <p class="mb-1 w-100">' + data.name + '</p>'
            + '    <p class="mb-0 w-100 nav-cart-item-price">' + data.count + ' <i class="fas fa-times"></i> <span class="price">$' + data.amount / data.count + '</span></p>'
            + '   </div>'
            + ' </a>'
            + ' <div class="nav-cart-close" onclick=NavBar.delCart(\'' + data.id + '\')>x</div>'
            + '</div>';


        return html;
    },


    RefreshCart(data) {
        let html = '<a  class="nav-link pl-0" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">'
            + '        <i class="fas fa-shopping-cart"></i>' + i18next.t("btn_nav_cart") + "(" + data.count + ")"
            + '      </a>'
            + '      <div class="dropdown-menu rounded-0 nav-cart-items px-2"> '
            + data.cartList
            + '       <div class="ml-auto my-2 col-6">'
            + '         <div class="row"> '
            + '           <div class="col-6 nav-items-num">小計</div> '
            + '           <div class="col-sm-6 col-12 text-right">'
            + '            <span class="nav-items-num-mobile">小計</span> '
            + '            <span class="price">' + data.amount + '</span> '
            + '           </div>'
            + '         </div>'
            + '       </div> '
            + '       <div class="ml-auto my-2 col-6">'
            + '         <div class="row">'
            + '           <div class="col-sm-6 col-4 nav-cart-items-clear-All" data-i18n="btn_nav_checkout" onclick="NavBar.clearCarts()">清空</div>'
            + '           <div class="col-sm-6 col-4 px-0"><a class="btn btn-register navs-btn-checkout" data-i18n="btn_nav_clean" href="./cart.html">結帳</a></div>'
            + '         </div>'
            + '       </div>'
            + '      </div>'
        $('.nav-cart-group').html(html);
    },

    delCart(id) {
        Utils.ProcessAjax("/api/ShopCart/" + id, "DELETE", true, "",
            function (ret) {
                if (ret.code == 1) {
                    NavBar.getCartData();
                    //alert("delete Cart Success");
                }
            }
        );
    },

    clearCarts() {
        Utils.ProcessAjax("/api/ShopCart/all", "DELETE", true, "",
            function (ret) {
                if (ret.code == 1) {
                    NavBar.getCartData();
                    //alert("Clean Cart Success");
                    alert(i18next.t("msg_cart_clear_success"));
                }
            }
        );
    },

    getAdminToolButton() {
        let html = '<div id="admin-viewport" data-authority="admin">'
            + '     <nav class="nav">'
            + '     	<a class="nav-link pl-0" href="" onclick=" Utils.openBankend()">'
            + '	            <i class="fas fa-rss"></i> 我訂閱的商店'
            + '     	</a>                                                                                    '
            + NavBar.getUserGroupButton()
            + '     </nav>'
            + '</div>';
        return html;
    },

    getUserGroupButton() {
        let user = JSON.parse(Utils.GetCookie("user"));
        let html = '<div class="btn-group">                                                                 '
            + '	<a class="nav-link" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">'
            + '		<i class="far fa-user"></i> ' + decodeURI(user.name) + ' <i class="fas fa-angle-down"></i>'
            + '	</a>'
            + '	<div class="dropdown-menu dropdown-menu-right rounded-0"> '
            + '		<a class="nav-link" data-i18n="btn_nav_order" href="./order.html">查詢我的訂單</a> '
            + '		<a class="nav-link" data-i18n="btn_nav_user" href="./member-info.html">維護會員資料</a> '
            + '     <a class="nav-link" data-i18n="btn_nav_logout" href="javascript:NavBar.doLogout()">登出</a>'
            + '	</div>'
            + '</div>';
        return html;
    },

    DoLogin() {
        let accountant = $("#accountant").val();
        let pwd = $("#password").val();
        if (NavBar.login(accountant, pwd)) {
            window.location.reload();
        } else {
            //           alert("User Authorization Error, Check  Again Accountant  Password.");
            alert(i18next.t("msg_login_error"));
        }

    },

    login(accountant, pwd) {
        let tenantCode = Utils.GetUrlParameter("tenantCode");
        let data = {
            "user": accountant,
            "pwd": pwd,
            "tenantCode": tenantCode
        };

        let result = Utils.AsyncProcessAjax("/api/auth/Login", "Post", "", data);

        if (result.code == "1") {
            Utils.SetCookie("token", result.token);
            result.name = encodeURI(result.name);
            Utils.SetCookie("user", JSON.stringify(result));
        } else {
            // alert("User Authorization Error, Login Again Please.");
            alert(i18next.t("msg_login_error"));
        }
        return result.type && (Utils.ROLE_GUEST != result.type);

    },

    doLogout() {
        Utils.ClearToken();
        window.location.reload();
    }




};