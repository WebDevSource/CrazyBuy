ProductItem = {

    getHtml(data, role) {
        let imageUrl = Utils.GetImageUrl(data);
        let detailUrl = "./product-inner.html?id=" + data.id
        html = '<div class="row product-col-one">'
            + '  <div class="col-md-3 col-4 product-col-img">'
            + '    <a class="left-product-img" href="' + detailUrl + '">'
            + '      <img src="' + imageUrl + '" class="img-fluid card-img-bg" alt="">'
            + '    </a>'
            + '  </div>'
            + '  <div class="col-md-9 col-8 product-col-info ">'
            + '    <h5 class="card-title products-title-color">' + data.name + '</h5>'
            + '    <p class="product-text-info">'
            + Utils.parseTextToHtml(data.summary)
            + '</p>'
            + '    <div class="d-flex flex-wrap">'
            + ProductItem.getTagHtml(data, role)
            + '      <div class="d-flex order-md-1 order-0">'
            + ProductItem.getPriceHtml(data, role)
            + '      </div>'
            + '    </div>'
            + ProductItem.getPriceBtnHtml(data, role)
            + '    <!-- 會員視角 -->'
            + '    <!-- <button class="product-addto-cart"> <span class="desktop-cart-btn">加入購物車</span> <i class="fas fa-cart-plus mobile-cart-btn"></i></button> --> '
            + ProductItem.getImgBtnHtml(data, role)
            + '  </div> '
            + '</div>';
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
                html += '<p class="card-text product-price " ' + ((i == len) ? '' : 'style="margin-right:10px"') + ' >' + i18next.t( price) + '<span>' + prices[price] + '</span></p>';
            }
        }
        return html;
    },

    getImgBtnHtml(data, role) {
        let html = '';
        if (role == Utils.ROLE_ADMIN) {
            html += '    <div">                                                                                     '
                + '      <a href="#" onclick=" Utils.openBankend()"><button class="product-admin-btn mt-2">' + i18next.t("btn_product_edit") + '</button></a>'
                + '      <a href="#" onclick=" Utils.openBankend()"><button class="product-admin-btn mt-2">' + i18next.t("btn_product_addImg") + '</button></a>'
                + '    </div> ';
        }
        return html;
    },

    getPriceBtnHtml(data, role) {
        let html = '';
        if (role == Utils.ROLE_GUEST) {
            return html;
        }
//        if (true) {
 //           html += '<button class="product-addto-cart" onclick="ProductCard.addCart(\'' + data.id + '\')"> <span class="desktop-cart-btn" >' + i18next.t("btn_product_addCart") + '</span ><i class="fas fa-cart-plus mobile-cart-btn"></i></button>';
 //       } else {
 //           html += '<button class="product-addto-cart product-soldout-cart"> <span class="desktop-cart-btn" >' + i18next.t("btn_product_soldout") + '</span ><i class="fas fa-shopping-cart mobile-cart-btn"></i></button>';
 //       }
        //html += '<button class="product-addto-cart" onclick="ProductCard.addCart(\'' + data.id + '\')"> <span class="desktop-cart-btn" >' + i18next.t("btn_product_addCart") + '</span ><i class="fas fa-cart-plus mobile-cart-btn"></i></button>';
        //       if (role == Utils.ROLE_ADMIN) {
        //               html += '<button class="btn btn-outline-register btn-admin-edit btn-product-edit px-3 py-0" onClick=ProductCard.openUrl("'+ "http://yahoo.com.tw" +'")>' + i18next.t("btn_edit")
        //               + '</button>';
        //       }
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
            html += '<div class="product-badge order-md-0 order-1 mb-20 w-100" >';
            let i = 1;
            let len = Object.keys(tags).length;
            let hasOnly = false;
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
            productId: id,
            count: 1
        }
        Utils.ProcessAjax("/api/ShopCart", "PUT", true, data,
            function (ret) {
                if (ret.code == 1) {
                    NavBar.getCartData();
                    alert("Add Cart Success");
                }
            }
        );

    }





};
