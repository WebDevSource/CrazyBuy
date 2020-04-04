ProductCard = {

    getHtml(data, role) {

        let imageUrl = Utils.GetImageUrl(data);
        let detailUrl = "./product-inner.html?id=" + data.id
        let html = '<div class="items-card col px-4 mb-4">                                                                                                        '
            + '  <div class="card h-100 border-0">                                                                                                            '
            + '    <div class="card-img-top">                                                                                                                 '
            + ProductCard.getImgBtnHtml(data, role)
            + '         <a href="' + detailUrl + '">                                                                                                                                      '
            + '             <img src="' + imageUrl + '" class="img-fluid card-img-bg" alt="">                                                                   '
            + '         </a>                                                                                                                                     '
            + '    </div>'
            + ProductCard.getTagHtml(data, role)
            + '    <div class="card-body">                                                                                                                    '
            + '      <h5 class="card-title product-title">' + data.name + '</h5>                                                                               '
            + '       <div class="text-center">'
            + ProductCard.getPriceHtml(data, role)
            + ProductCard.getPriceBtnHtml(data, role)
            + '      </div>                                                                                                                               '
            + '    </div>                                                                                                                                     '
            + '  </div>                                                                                                                                       '
            + '</div>                                                                                                                                         '
        return html;
    },

    getPriceHtml(data, role) {
        let html = '';
        if (role == Utils.ROLE_GUEST) {
            html += '<p class="card-text price">' + i18next.t("price_hide") + '</p>';
        } else {
            let prices = data.prices;
            let i = 0;
            let len = Object.keys(prices).length;
            for (let price in prices) {
                i++;
                html += '<p class="card-text product-price ' + ((i == len) ? '' : 'mb-0') + '">' + i18next.t(price) + prices[price] + '</p>';
            }
        }
        return html;
    },

    getImgBtnHtml(data, role) {
        let html = '';
        if (role == Utils.ROLE_ADMIN) {
            html += '    <div class="product-item-edit" data-authority="admin">                                                                                     '
                + '      <a class="d-block mb-2" href="" onclick="Utils.openBankend()" ><i class="fas fa-edit" ></i><span>' + i18next.t("btn_product_edit") + '</span></a>                         '
                + '      <a class="d-block" href="" onclick=" Utils.openBankend()"><i class="fas fa-image"></i><span>' + i18next.t("btn_product_addImg") + '</span></a>                           '
                + '    </div> ';
        }
        return html;
    },

    getPriceBtnHtml(data, role) {
        let html = '';
        if (role == Utils.ROLE_GUEST) {
            return html;
        }
 //       if (true) {
 //           html += '<button class="product-addto-cart" onclick="ProductCard.addCart(\'' + data.id + '\')">' + i18next.t("btn_product_addCart") + '</button>     ';
//        } else {
 //           html += '<button class="product-addto-cart product-soldout-cart" onclick="">' + i18next.t("btn_product_soldout") + '</button>     ';
 //       }
        if (role == Utils.ROLE_ADMIN) {
            html += '<button class="btn btn-outline-register btn-admin-edit btn-product-edit px-3 py-0">' + i18next.t("btn_edit")
                + '</button>';
        }
        return html;
    },


    getTagHtml(data, role) {
        let html = '';
        if (role == Utils.ROLE_GUEST) {
        } else {
            let tags = JSON.parse(data.tags);
            if (!tags) {
                return html;
            }
            html += '<div class="product-badge" >';
            let i = 1;
            let hasOnly = false;
            let len = Object.keys(tags).length;
            for (let tag in tags) {
                let name = tags[tag];
                if (name.indexOf(i18next.t("tag_factory")) > -1) {
                    if (hasOnly) {
                        continue;
                    } else {
                        hasOnly = true;
                        name = i18next.t("tag_only");
                    }
                } else if (name.indexOf(i18next.t("tag_only")) > -1) {
                    if (hasOnly) {
                        continue;
                    } else {

                        hasOnly = true;
                    }
                }
                i++;
                html += '  <span class="badge ' + ((1 == i % 2) ? 'badge-no-discount' : 'badge-no-freight') + '">' + name + '</span>';
            }
            html += '</div>';
        }
        return html;
    },

    addCart(id) {

        let data = {
            productId: parseInt(id),
            count: 1
        }
        Utils.ProcessAjax("/api/ShopCart", "PUT", true, data,
            function (ret) {
                if (ret.code == 1) {
                    NavBar.getCartData();
                    alert(i18next.t("msg_cart_add_success"));
                }
            }
        );

    }





};
