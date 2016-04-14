(function ($) {
    "use strict";
	//isotope setting
	var $container = $('.portfolio-body');
	
	$container.imagesLoaded(function () {
		$container.isotope();
	});
	$(window).on('resize', function(){
		$('.portfolio-body').isotope('reLayout');
	});
	// filter items when filter link is clicked
	$('.port-filter a').click(function () {
		var selector = $(this).attr('data-filter');
		$container.isotope({
			filter: selector
		});
		return false;
	});
})(jQuery);