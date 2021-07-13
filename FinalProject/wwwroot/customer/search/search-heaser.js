function Search() {
        var input = $('#txtSearch').val();
       $.ajax({
            url: '/Search/SearchAjax',
            method: 'GET',
            async: true,
            data: { text: input },
            success: function (data) {
                var listProduct = JSON.parse(data);
                var str = '';
                for (let item of listProduct) {
                    str += '<div class="flex-row">'+
                       ' <img src="./images/"' + item.ProductImage+  'width="50px" height="80px">'+
                           ' <div class="flex-column">'+
                        ' <span onclick="' + 'window.location="' + '/Product/Detail?Id=' + item.ProductId + '" class="name">'+ item.ProductName+'</span>' +
                               '<span>' + item.Price + '</span>'+
                           ' </div></div>'
                }
                $('#Rule').html(str);
                alert(data);
            }
        })
    }