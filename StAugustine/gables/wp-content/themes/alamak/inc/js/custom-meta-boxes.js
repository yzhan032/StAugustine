(function ($) {

    function checkMetaboxSettings() {
        if ($('#post-format-video,#post-format-audio').is(":checked")) {
            $('#post-details').css('display', 'block');
            $('#embed-post-code').css('display', 'block');
			$('#gallery-code').css('display', 'none');
			$('#quote-code').css('display', 'none');
			$('#link-post-url').css('display', 'none');
		}
		else if ($('#post-format-gallery').is(":checked")) {
            $('#gallery-code').css('display', 'block');
			$('#post-details').css('display', 'block');
            $('#link-post-url').css('display', 'none');
			$('#embed-post-code').css('display', 'none');
			$('#quote-code').css('display', 'none');
        } 
		else if ($('#post-format-quote').is(":checked")) {
            $('#quote-code').css('display', 'block');
			$('#post-details').css('display', 'block');
            $('#link-post-url').css('display', 'none');
			$('#embed-post-code').css('display', 'none');
			$('#gallery-code').css('display', 'none');
        } 
		else if ($('#post-format-link').is(":checked")) {
            $('#quote-code').css('display', 'none');
			$('#post-details').css('display', 'block');
            $('#link-post-url').css('display', 'block');
			$('#embed-post-code').css('display', 'none');
			$('#gallery-code').css('display', 'none');
        } 
		else {
            $('#post-details').css('display', 'none');
        }
    }

    checkMetaboxSettings();

    $('.post-format').change(function () {
        checkMetaboxSettings();
    });
})(jQuery);



