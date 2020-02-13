ProductCard = {

    getHtml(data, role) {
/*        {
            "prices": { "會員價": "$160", "一般價": "$170" },
            "title": "富貴團圓年菜9件組(12/10)",
                "url": "./images/item-1.png",
                    "id": 8
        }

        prices: {normal: "$100"}
id: "80ff9a2e-6a95-413a-8cca-adfb11e99d7b"
name: "富士山蘋果"
prdCode: "20200202235553"
tenantId: "fe883257-2183-4562-8f73-ddd7b4f27f1e"
summary: "好吃的富士山蘋果"
prdImages: "{"fileName":"apple.jpg","type":"jpg"}"
paymentType: "["ATM轉帳"]"
shipType: "["貨源"]"
tags: "["冷藏","獨立下單,"特殊運件"]"
		];
*/
        let images = JSON.parse(data.prdImages);
        let html = '<div class="items-card col px-4 mb-4">                                                                                                        '
            + '  <div class="card h-100 border-0">                                                                                                            '
            + '    <div class="card-img-top">                                                                                                                 '
            +           ProductCard.getImgBtnHtml(data,role)
            + '         <a href="./product-inner.html">                                                                                                                                      '
            + '             <img src="'+ images.fileName +'" class="img-fluid card-img-bg" alt="">                                                                   '
            + '         </a>                                                                                                                                     '
            + '    </div>'   
            +       ProductCard.getTagHtml(data, role)
            + '    <div class="card-body">                                                                                                                    '
            + '      <h5 class="card-title product-title">' + data.name + '</h5>                                                                               '
            +'       <div class="text-center">'
            + ProductCard.getPriceHtml(data, role)
            + ProductCard.getPriceBtnHtml(data,role)
            + '      </div>                                                                                                                               '
            + '    </div>                                                                                                                                     '
            + '  </div>                                                                                                                                       '
            + '</div>                                                                                                                                         '
        return html;
    },

    getPriceHtml(data, role) {
        let html = '';
        if (role == Utils.ROLE_GUEST) {
            html += '<p class="card-text price" data-authority="guest">' + i18next.t("price_hide") + '</p>';
        } else {
            let prices = data.prices;
            let i = 0;
            let len = Object.keys(prices).length;
            for (let price in prices) {
                i++;
                html += '<p class="card-text product-price ' + ((i == len) ? '' : 'mb-0') + '">' + i18next.t("price_"+price) + prices[price] + '</p>';
            }
        } 
        return html;
    },

    getImgBtnHtml(data, role) {
        let html = '';
        if (role == Utils.ROLE_ADMIN) { 
            html+= '    <div class="product-item-edit" data-authority="admin">                                                                                     '
                + '      <a class="d-block mb-2" href=""><i class="fas fa-edit"></i><span>' + i18next.t("btn_product_edit") + '</span></a>                         '
                + '      <a class="d-block" href=""><i class="fas fa-image"></i><span>' + i18next.t("btn_product_addImg") + '</span></a>                           '
                + '    </div> ';
        }
        return html;
    },

    getPriceBtnHtml(data, role) {
        let html = '';
        if (role == Utils.ROLE_GUEST) {
            return html;
        }
        html += '<button class="product-addto-cart">' + i18next.t("btn_product_addCart") + '</button>     '; 
        if (role == Utils.ROLE_ADMIN) {
                html += '<button class="btn btn-outline-register btn-admin-edit btn-product-edit px-3 py-0" onClick=ProductCard.openUrl("'+ "http://yahoo.com.tw" +'")>' + i18next.t("btn_edit")
                + '</button>';
        }
        return html;
    },


    getTagHtml(data,role) {
        let html = '';
        if (role == Utils.ROLE_GUEST) {
        } else {
            let tags = JSON.parse(data.tags);
            if (!tags) {
                return html;
            }
            html += '<div class="product-badge" >';
            let i = 1;
            let len = Object.keys(tags).length;
            for (let tag in tags) {
                i++;
                html += '  <span class="badge ' + ((1 == i % 2) ? 'badge-no-discount' : 'badge-no-freight') + '">' + tags[tag] + '</span>';
            }
            html += '</div>';
        }
        return html;                          
    }







};
