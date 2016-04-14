(function ($) {

    function checkMetaboxSettings() {
        if ($('#post-format-video,#post-format-audio').is(":checked")) {
            $('#post-details').css('display', 'block');
            $('#embed-post-code').css('display', 'block');
			$('#gallery-port').css('display', 'none');
		}
		else if ($('#post-format-gallery').is(":checked")) {
            $('#gallery-port').css('display', 'block');
			$('#post-details').css('display', 'block');
            $('#link-post-url').css('display', 'none');
			$('#embed-post-code').css('display', 'none');
			$('#quote-code').css('display', 'none');
        } 
		else {
			$('#embed-post-code').css('display', 'none');
			$('#gallery-port').css('display', 'none');
        }
    }

    checkMetaboxSettings();

    $('.post-format').change(function () {
        checkMetaboxSettings();
    });
})(jQuery);



