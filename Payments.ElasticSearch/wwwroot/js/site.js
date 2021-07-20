// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {

    $('.btn-payment-import').click(function () {
            $.get('/api/payment/fakeimport/', function () {
                location.reload();
            });
    });


    $('.btn-payment-reindex').click(function () {
        $.get('/api/search/reindex', function () {
            location.reload();
        });
    });


    $('#txt-payment-search').keyup(function () {
        if ($(this).val().length >= 2) {
            $.get('/api/search/find?query=' + $(this).val(), function (data) {
                $('.search-result').html('');
                $(data).each(function (index, element) {
                    var itemTpl = $($('#searchitem').html());
                    itemTpl.find('.payment-id').html(element.id);
                    itemTpl.find('.payment-name').html(element.name);
                    itemTpl.find('.payment-category').html(element.category);
                    itemTpl.find('.payment-description').html(element.description);
                    itemTpl.find('.payment-releaseDate').html(element.releaseDate);
                    $('.search-result').append(itemTpl);
                })
                $('.search-result').show();
            });
        }
        else {
            $('.search-result').hide();
        }
    })

});