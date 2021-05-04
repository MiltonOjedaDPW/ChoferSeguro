$(window).on('scroll', function() {
    $('.salto').each(function() {
        if($(window).scrollTop() >= $(this).offset().top) {          
            var text = $(this).attr('data-title');
            $('#title').text(text);
        }
    });
});