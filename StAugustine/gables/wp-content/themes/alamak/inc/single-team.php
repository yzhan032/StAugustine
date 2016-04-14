<?php 
	


			

//script for single team	
function rdn_single_team() {
	if( is_page_template( 'homepage-slider.php' ) || is_page_template( 'homepage-video.php' ) || is_page_template( 'homepage-youtube.php' )|| is_page_template( 'homepage-custom.php' )){
?>
		<script type="text/javascript">
					(function ($) {
					"use strict";
					
						
						
						//portfolio ajax setting
						$(document).ready(function () {
							$('.team-ajax,.team-icon').click(function () {
						
								var toLoad = $(this).attr('data-link') + ' .teamajax > *';
								$('.teamajax').slideUp('slow', loadContent);
						
								function loadContent() {
									$('.teamajax').load(toLoad, '', showNewContent)
								}
						
								function showNewContent() {
									$.getScript("<?php echo get_template_directory_uri(); ?>/js/team.js");
									$('.teamajax').slideDown('slow');
						
								}
						
								return false;
						
						
							});
						
						});
					})(jQuery);
		</script>
<?php }
}