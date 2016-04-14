(function ($) {
    "use strict";
//portfolio close button 
$('.close').click(function () {
    $('.worksajax').slideUp();
    return false;
});

//remove br in gallery
    $(".gallery-inner .gallery br,.gallery-portfolio .gallery br").remove();
    $(".gallery-portfolio  style,.gallery-inner  style").remove();
    $(".gallery-inner .gallery,.gallery-portfolio .gallery").addClass("owl-carousel");

    $(".gallery-inner .gallery,.gallery-portfolio .gallery").owlCarousel({
        navigation: true, // Show next and prev buttons
        slideSpeed: 300,
        paginationSpeed: 400,
        autoPlay: 5000,
        navigationText: ['<span class="slide-nav inleft"><i class="fa fa-chevron-left"></i></span >', '<span class="slide-nav inright"><i class="fa fa-chevron-right"></i></span >'],
        singleItem: true
    });

			
// portfolio Video responsive
        $(".worksajax .video").fitVids();
	
// script prettyphoto 
$(document).ready(function () {
    $("a[data-rel^='prettyPhoto'],.gallery-portfolio a").prettyPhoto({
        social_tools: false,
        deeplinking: false
    });
});

//easing portfolio scrolling
$(function () {
    $('.close').bind('click', function (event) {
        var $anchor = $('#portfolio');

        $('html, body').stop().animate({
            scrollTop: $($anchor).offset().top - 89
        }, 1000, 'linear');
        event.preventDefault();
    });
});


// Video responsive
$(".white-bg").fitVids();

})(jQuery);