(function ($) {
    "use strict";

//portfolio close button 
$('.team-close').click(function () {
    $('.teamajax').slideUp();
    return false;
});

//remove br in gallery
$(".teamajax .gallery br").remove();
$(".teamajax .gallery-inner style").remove();
$( ".teamajax .gallery" ).addClass( "owl-carousel" );

$(".teamajax .gallery").owlCarousel({
	navigation : true, // Show next and prev buttons
	slideSpeed : 300,
	paginationSpeed : 400,
	autoPlay : 5000,
	navigationText :	['<span class="slide-nav inleft"><i class="fa fa-chevron-left"></i></span >','<span class="slide-nav inright"><i class="fa fa-chevron-right"></i></span >'],
	singleItem:true
});

// portfolio Video responsive
        $(".teamajax .video").fitVids();
	
// script prettyphoto 
$(document).ready(function () {
    $("a[data-rel^='prettyPhoto'],.gallery-portfolio a").prettyPhoto({
        social_tools: false,
        deeplinking: false
    });
});


//easing portfolio scrolling
$(function () {
    $('.team-close').bind('click', function (event) {
        var $anchor = $('#team');

        $('html, body').stop().animate({
            scrollTop: $($anchor).offset().top - 89
        }, 1000, 'linear');
        event.preventDefault();
    });
});


// Video responsive
$(".white-bg").fitVids();

})(jQuery);