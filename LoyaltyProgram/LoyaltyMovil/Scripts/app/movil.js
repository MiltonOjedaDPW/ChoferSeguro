var GETREGISTROPORCHOFER; var choferRNTT; var GET_REGISTRO_RECOMPENSAS;
$(window).on('scroll', function () {
    $('.salto').each(function () {
        if ($(window).scrollTop() >= $(this).offset().top) {
            var text = $(this).attr('data-title');
            $('#title').text(text);
        }
    });
});


$(document).ready(function () {

    console.log(GETREGISTROPORCHOFER);

});
