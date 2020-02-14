var ProductInner = {
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
        let id = Utils.GetUrlParameter("id");
        let result = Utils.AsyncProcessAjax("/api/prd/getHomePrdItem/" + id, "GET", true, "");
        if (result.code == "1") {
            ProductInner.InitProductItem(result.data);
        } else {
            alert("system error reload Please");
        }
    },

    InitProductItem(item) {
        let fakeImages = ["./images/1200x800.jpg", "./images/1200x800.jpg", "./images/1200x800.jpg", "./images/1200x800.jpg", "./images/1200x800.jpg", "./images/1200x800.jpg", "./images/1200x800.jpg", "./images/1200x800.jpg", "./images/1200x800.jpg", "./images/1200x800.jpg", "./images/1200x800.jpg", "./images/1200x800.jpg", "./images/1200x800.jpg", "./images/1200x800.jpg"];
        ProductInner.InitImages(fakeImages);
    },

    InitImages(items) {
        let imageHtml = '<div class="carousel-inner" >';
        let i = 0;
        for (let key in items) {
            let item = items[key];
            let url = item;
            imageHtml += '<div class="carousel-item ' + (i == 0 ? 'active' : "") +'" data-slide-number="' + i++ + '" >'
                + '	<img src="' + url + '" class="d-block w-100" alt="..." data-remote="' + url + '/"> '
                + '</div>';
        }
        imageHtml += "</div>";

        let iconHtml = ProductInner.getThumbsHtml(items);

        $("#proudctCarousel").html(imageHtml);
        $("#carousel-thumbs").prepend(iconHtml);

    },
    getThumbsHtml(items) {
        html = '<ol class="carousel-indicators">';
        for (let i = 0; i < items.length; i++) {
            html += ' <li id="carousel-selector-' + i + '" data-target="#proudctCarousel" data-slide-to="' + i + '" class="' + (i == 0 ?'active':"")+'"></li> ';
        }
        html += '</ol> ';
        html += '<div class="desktop-navs-indicators">'
        for (let i = 0; i < items.length; i++) {
            let item = items[i];
            if (i == 0) {
                html += '<div class="carousel-item active">'
                    + '    <div class="row mx-0">';
            } else if(i % 4 == 0) {
                html += '</div></div><div class="carousel-item">'
                    + '    <div class="row mx-0">';
            }
            html += '<div id="carousel-selector-' + i + '" class="thumb col-3 px-1 py-2 selected" data-target="#proudctCarousel" data-slide-to="' + i + '" class="carousel-item"> '
                + '     <img src="' + item + '" class="img-fluid" alt="..."> '
                + ' </div>'
        }
        return html;
    }


};

window.onload = ProductInner.doLoad;