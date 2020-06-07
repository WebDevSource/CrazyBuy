
var ProductInner = {

    id: 0,

    isRadio: false,

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
                alert(i18next.t("msg_service_error"));
            }
        });
    },

    InitProductItem(item) {
        let role = Utils.getRole();
        let urlItems = ["./images/noitem.jpg"];
        if (item.prdImages && role != Utils.ROLE_GUEST) {
            let baseUrl = Utils.BackendImageUrl;
            urlItems = [];
            let urls = JSON.parse(item.prdImages);
            for (let key in urls) {
                //urlItems.push(baseUrl + urls[key].filename);
                urlItems.push(baseUrl + "&id=" + item.id + "&filename=" + urls[key].filename);
            }
        }

        if (!item.isOpenOrder) {

        } else if (item.isTakeOff){
            $(".soldoutCart").text(item.takeOffMessage);
            $(".soldoutCart").show();
        }
        else if (item.count < 1 && item.zeroStock) {
            $(".soldoutCart").text(item.zeroStock);
            $(".soldoutCart").show();
        } else {
            $(".addCart").show();
        }

        ProductInner.InitImages(urlItems);
        item.tags = ProductInner.getTagsHtml(item.tags, role);
        item.prices = ProductInner.getPricesHtml(item.prices, role);
        item.shipType = JSON.parse(item.shipType) ? JSON.parse(item.shipType).toString() : "";
        item.paymentType = JSON.parse(item.paymentType) ? JSON.parse(item.paymentType).toString() : "";
        item.summary = Utils.parseTextToHtml(item.summary);
        item.desc = Utils.parseTextToHtml(item.desc);
        item.sepc = ProductInner.getSepcHtml(item.sepc);
        ProductInner.setFBSEOContent(urlItems[0],item);
        for (let key in item) {
            $("[data-name=" + key + "]").append(item[key]);
        }

    },

    setFBSEOContent(imageUrl, item) {
        let html = "<meta property='og:title' content='" + item.name + "'/>";
        html += "<meta property='og:description' content='" + item.summary + "' />";
        html += "<meta property='og:image' content='" + imageUrl + "'/>";
        html += "<meta property='og:url' content='" + location.href + "'/>";
        html += "<meta property='og:type' content='website'/>";
        html += "<meta property='og:site_name' content='" + $('title').text() + "'/>";

        //let html = "<meta property='og:title' content='Apple'/>";
        //html += "<meta property='og:description' content='Discover the innovative world of Apple and shop everything iPhone, iPad, Apple Watch, Mac, and Apple TV, plus explore accessories, entertainment, and expert device support.'/>";
        //html += "<meta property='og:image' content='https://www.apple.com/ac/structured-data/images/open_graph_logo.png?201703170823'/>";
        //html += "<meta property='og:url' content='https://www.apple.com/'/>";
        //html += "<meta property='og:type' content='website'/>";
        //html += "<meta property='og:site_name' content='Apple'/>";
        $("head").append(html);
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
                head += '<th>' + i18next.t(price) + '</th>';
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

    getSepcHtml(items) {
        items = JSON.parse(items);
        let html = "";
        if (!items) {
            return html;
        }
        if (items[0].type == i18next.t("prd_sepc_type_radius")) {
            for (let i in items) {
                html += "<input type='radio' value='" + JSON.stringify(items[i]) + "' name='sepc' " + (i == 0 ? 'checked' : '') + "/> "
                    + "<label>" + items[i].name + "</label> &nbsp;";
            }
            ProductInner.isRadio = true;
        } else {

            html += "<select class='checkout-select' name='sepc' style='width:150px;'>";
            for (let i in items) {
                html += "<option value='" + JSON.stringify(items[i]) + "'> " + items[i].name + "</option>";
            }
            html += "</select>"
        }
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
            alert(i18next.t("msg_register_member"));
            return;
        }

        let sepc = ProductInner.isRadio ? $('input[name=sepc]:checked').val() : $('[name=sepc]').val();

        let data = {
            productId: parseInt(ProductInner.id),
            count: parseInt($(".product-nums").val()),
            sepc: sepc
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

window.onload = ProductInner.doLoad;