
var ProductInner = {
    id: 0,

    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "products", ProductInner.InitModule);
    },


    InitModule() {
        NavBar.Init();
        ProductInner.InitView();
    },

    InitView() {
        ProductInner.getProductItem();
    },

    getProductItem() {
        ProductInner.id = Utils.GetUrlParameter("id");
        Utils.ProcessAjax("/api/prd/getHomePrdItem/" + ProductInner.id, "GET", true, "", function (ret) {
            if (ret.code == "1") {
                ProductInner.InitProductItem(ret.data);
            } else {
                alert("system error reload Please");
            }
        });
    },

    InitProductItem(item) {
        let urlItems = ["./images/noitem.jpg"];
        if (item.prdImages) {
            let baseUrl = Utils.BackendUrl;
            urlItems = [];
            let urls = JSON.parse(item.prdImages);
            for (let key in urls) {
                //urlItems.push(baseUrl + urls[key].filename);
                urlItems.push(baseUrl + "id=" + item.id + "&filename=" + urls[key].filename);
            }
        }
        if (true) {
            $(".addCart").show();
        } else {
            $(".soldoutCart").show();
        }
        let role = Utils.getRole();
        ProductInner.InitImages(urlItems);
        item.tags = ProductInner.getTagsHtml(item.tags, role);
        item.prices = ProductInner.getPricesHtml(item.prices, role);
        item.shipType = JSON.parse(item.shipType) ? JSON.parse(item.shipType).toString() : "";
        item.paymentType = JSON.parse(item.paymentType) ? JSON.parse(item.paymentType).toString() : "";
        for (let key in item) {
            $("[data-name=" + key + "]").append(item[key]);
        }

    },

    getTagsHtml(item, role) {
        let html = '';
        if (role == Utils.ROLE_GUEST) {
        } else {
            let tags = JSON.parse(item);
            if (!tags) {
                return html;
            }
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
        }
        return html;
    },

    getPricesHtml(item, role) {
        let head = "<thead><tr>";
        let body = "<tbody><tr>";
        if (role == Utils.ROLE_GUEST) {
            head += '<th>' + i18next.t("price_hide") + '</th>';
        } else {
            let i = 0;
            let len = Object.keys(item).length;
            for (let price in item) {
                head += '<th>' + i18next.t("price_" + price) + '</th>';
                body += '<td>' + item[price] + '</td>';
                i++;
            }
        }
        head += "</tr></thead>";
        body += "</tr></tbody>";
        return head + body;
    },

    InitImages(items) {
        let imageHtml = '<div class="carousel-inner" >';
        let i = 0;
        for (let key in items) {
            let item = items[key];
            let url = item;
            imageHtml += '<div class="carousel-item ' + (i == 0 ? 'active' : "") + '" data-slide-number="' + i++ + '" >'
                + '	<img src="' + url + '" class="d-block w-100" alt="" data-remote="' + url + '/"> '
                + '</div>';
        }
        imageHtml += "</div>";

        let iconHtml = ProductInner.getThumbsHtml(items);

        $("#proudctCarousel").html(imageHtml);
        $("#carousel-thumbs > .carousel-inner").prepend(iconHtml);

    },
    getThumbsHtml(items) {

        let html = '<ol class="carousel-indicators">';
        for (let i = 0; i < items.length; i++) {
            html += ' <li id="carousel-selector-' + i + '" data-target="#proudctCarousel" data-slide-to="' + i + '" class="' + (i == 0 ? 'active' : "") + '"></li> ';
        }
        html += '</ol> ';
        html += '<div class="desktop-navs-indicators">'
        for (let i = 0; i < items.length; i++) {
            let item = items[i];
            if (i == 0) {
                html += '<div class="carousel-item active">'
                    + '    <div class="row mx-0">';
            } else if (i % 4 == 0) {
                html += '</div></div><div class="carousel-item">'
                    + '    <div class="row mx-0">';
            }
            html += '<div id="carousel-selector-' + i + '" class="thumb col-3 px-1 py-2 selected" data-target="#proudctCarousel" data-slide-to="' + i + '" class="carousel-item"> '
                + '     <img src="' + item + '" class="img-fluid" alt=""> '
                + ' </div>'
        }
        html += '</div>';
        return html;
    },

    changeCount(add) {
        let elem = $(".product-nums");
        let count = elem.val();
        if (add) {
            count++;

        } else {
            if (count > 1) {
                count--;
            }
        }
        elem.val(count);
    },

    addCart() {
        if (Utils.ROLE_GUEST == Utils.getRole()) {
            alert("register Member frist ,Thanks!");
            return;
        }

        let data = {
            productId: ProductInner.id,
            count: parseInt($(".product-nums").val())
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

window.onload = ProductInner.doLoad;