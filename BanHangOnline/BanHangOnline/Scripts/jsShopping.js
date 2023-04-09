$(document).ready(function () {
    ShowCount();
    $('body').on("click", ".btnAddToCart", function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        var quantity = 1;
        var tQuantity = $('#quantity_value').text();
        if (tQuantity != '') {
            quantity = parseInt(tQuantity)
        }
        $.ajax({
            type: "POST",
            url: "/shoppingcart/addtocart",
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.Success) {
                    $("#checkout_items").html(rs.Count)
                    alert(rs.msg);
                }
            }
        });
    });

    $('body').on("click", ".btnDelete", function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        var conf = confirm("Bạn có muốn xóa sản phẩm này không?")
        if (conf) {
            $.ajax({
                type: "POST",
                url: "/shoppingcart/Delete",
                data: { id: id },
                success: function (rs) {
                    if (rs.Success) {
                        $("#checkout_items").html(rs.Count)
                        $("#trow_" + id).remove();
                        alert(rs.msg)
                        loadCart();
                    }
                }
            });
        }
    });

    $('body').on("click", ".btnUpdate", function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        var quantity = $("#Quantity_" + id).val();
        Update(id, quantity);
    });

    $('body').on("click", ".btnDeleteAll", function (e) {
        e.preventDefault();
        var conf = confirm("Bạn có muốn xóa hết sản phẩm trong giỏ hàng không?")
        if (conf) {
            DeleteAll();
        }
    });
})

function Update(id , quantity) {
    $.ajax({
        type: "POST",
        url: "/shoppingcart/Update",
        data: { id: id, quantity: quantity },
        success: function (rs) {
            if (rs.Success) {
                loadCart();
            }
        }
    });
}

function DeleteAll() {
    $.ajax({
        type: "POST",
        url: "/shoppingcart/DeleteAll",
        success: function (rs) {
            if (rs.Success) {
                loadCart();
            }
        }
    });
}

function ShowCount() {
    $.ajax({
        type: "GET",
        url: "/shoppingcart/ShowCount",
        success: function (rs) {
            $("#checkout_items").html(rs.Count)
        }
    });
}

function loadCart() {
    $.ajax({
        type: "GET",
        url: "/shoppingcart/Partial_Item_Cart",
        success: function (rs) {
            $("#load-data").html(rs)
        }
    });
}