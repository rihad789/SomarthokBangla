// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Navbar Jquery
// jquery ready start
$(document).ready(function () {
    // jQuery code

    //////////////////////// Prevent closing from click inside dropdown
    $(document).on('click', '.dropdown-menu', function (e) {
        e.stopPropagation();
    });

    // make it as accordion for smaller screens
    if ($(window).width() < 992) {
        $('.dropdown-menu a').click(function (e) {
            e.preventDefault();
            if ($(this).next('.submenu').length) {
                $(this).next('.submenu').toggle();
            }
            $('.dropdown').on('hide.bs.dropdown', function () {
                $(this).find('.submenu').hide();
            })
        });
    }


}); // jquery end



$(document).ready(function () {
    // jQuery code


    $("#SpecialTagId").change(function () {
        //alert("Handler for .change() called.");

        var tag = $("#SpecialTagId").val();
        $.ajax({
            type: "GET",
            url: "/Inventory/Products/getCategoryList",
            data: {id:tag},
            success: function (data) {
                var s = '<option value="-1">Select a Category</option>';
                for (var i = 0; i < data.length; i++) {
                    s += '<option value="' + data[i].value + '">' + data[i].text + '</option>';
                }
                $("#ProductCategoryId").html(s);
            }
        });  

    });

});

$(document).ready(function () {
    // jQuery code


    $("#ProductCategoryId").change(function () {
        //alert("Handler for .change() called.");

        var catID = $("#ProductCategoryId").val();


        $.ajax({
            type: "GET",
            url: "/Inventory/Products/getProductTypeList",
            data: { id: catID },
            success: function (data) {
                var s = '<option value="-1">Select a ProductType</option>';
                for (var i = 0; i < data.length; i++) {
                    s += '<option value="' + data[i].val + '">' + data[i].text + '</option>';
                }
                $("#ProductTypeId").html(s);
            }
        });

    });

});



