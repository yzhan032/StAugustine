(function ($) {
    "use strict";


    //sticky navigation
    $(document).ready(function () {
        $(".header").sticky({
            topSpacing: 0
        });
    });

	//script for navigation
    $('.desktop-menu ').superfish({
        delay: 400, //delay on mouseout
        animation: {
            opacity: 'show',
            height: 'show'
        }, // fade-in and slide-down animation
        animationOut: {
            opacity: 'hide',
            height: 'hide'
        },
        speed: 200, //  animation speed
        speedOut: 200,
        autoArrows: false // disable generation of arrow mark-up
    })
	
	//create aside background,link background from featured image
    $(".image-aside img,.image-link img").each(function (i, elem) {
        var img = $(elem);
        var div = $("<div />").css({
            background: "url(" + img.attr("src") + ") no-repeat",
            width: "100%",
            height: "100%"
        });

        div.addClass("image-background");

        img.replaceWith(div);
    });
	
    //toggle menu
    $('.menu-btn').on('click', function () {
        $('.mobile-menu').collapse({
            toggle: false
        });
    })
    //create menu for tablet/mobile
    
	$(".navigation").clone(false).find("ul,li").removeAttr("id").remove( ".sub-menu" ).appendTo($(".mobile-menu"));
	$(".mobile-menu .sub-menu").remove();
    $('.mobile-menu').on('show.bs.collapse', function () {
        $('body').on('click', function () {
            $('.mobile-menu').collapse('hide');
        })
    })

 	//menu for tablet/mobile scrolling
    $('.mobile-menu a').bind('click',function(event){
        var $anchor = $(this);
 
        $('html, body').stop().animate({
            scrollTop: $($anchor.attr('href')).offset().top - 80
        }, 800,'linear');
        event.preventDefault();
    });


    //Page scrolling
    $(document).ready(function () {
        $('.page-template-homepage-slider-php .navigation,.page-template-homepage-video-php .navigation,.page-template-homepage-youtube-php .navigation,.page-template-homepage-custom-php .navigation').onePageNav({
            filter: ':not(.external a)',
            scrollThreshold: 0.25,
            scrollOffset: 90
        });

    });



    // Video responsive
    $("body").fitVids();

    // script prettyphoto
    $(document).ready(function () {
        $("a[data-rel^='prettyPhoto'],.gallery-inner .gallery-icon a").prettyPhoto({
            social_tools: false,
            deeplinking: false
        });
    });
	 

    $(window).load(function () {
        $('.bwWrapper').BlackAndWhite({
            hoverEffect: true, // default true
            // set the path to BnWWorker.js for a superfast implementation
            webworkerPath: false,
            // for the images with a fluid width and height 
            responsive: true,
            // to invert the hover effect
            invertHoverEffect: false,
            // this option works only on the modern browsers ( on IE lower than 9 it remains always 1)
            intensity: 1,
            speed: { //this property could also be just speed: value for both fadeIn and fadeOut
                fadeIn: 200, // 200ms for fadeIn animations
                fadeOut: 800 // 800ms for fadeOut animations
            },
            onImageReady: function (img) {
                // this callback gets executed anytime an image is converted
            }
        });
    });




    //portfolio scrolling
    $(function () {
        $('.more,.more,.cat-icon').bind('click', function (event) {
            var $anchor = $('#workslist');

            $('html, body').stop().animate({
                scrollTop: $($anchor).offset().top - 89
            }, 1000, 'linear');
            event.preventDefault();
        });
    });

    //team scrolling
    $(function () {
        $('.team-ajax,.team-icon').bind('click', function (event) {
            var $anchor = $('#teamlist');

            $('html, body').stop().animate({
                scrollTop: $($anchor).offset().top - 89
            }, 1000, 'linear');
            event.preventDefault();
        });
    });
	
	//portfolio toggle class
    $(document).ready(function () {
        $(".white-btn").click(function (e) {
            if ($(this).hasClass("clicked")) {
                $(this).removeClass("clicked");
            } else {
                $(".white-btn").removeClass("clicked");
                $(this).addClass("clicked");
            }
        });
    });
	
    //remove br and empty p tag
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



    

    //reduce excerpt in portfolio
    $(".box-60 p").each(function () {
        var excerpt = $(this).text().substr(0, 80).lastIndexOf(" ");
        var eltexTo = $(this).text();
        var title = eltexTo;
        var shortText = title.trim().substring(0, excerpt).split(" ").slice(0, -1).join(" ") + "...";
        $(this).text(shortText);
    });

    //fix map in tab
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $('.map_canvas').gmap('refresh');
    })


})(jQuery);