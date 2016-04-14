<?php 
	


			

//script for single portfolio	
function rdn_single_portfolio() {
	if( is_page_template( 'homepage-slider.php' ) || is_page_template( 'homepage-video.php' ) || is_page_template( 'homepage-youtube.php' ) || is_page_template( 'homepage-custom.php' )
	|| is_page_template( 'portfolio-list.php' )){
?>
		<script type="text/javascript">
					(function ($) {
					"use strict";
					
						
						
						//portfolio ajax setting
						$(document).ready(function () {
							$('.more,.cat-icon').click(function () {
						
								var toLoad = $(this).attr('data-link') + ' .worksajax > *';
								$('.worksajax').slideUp('slow', loadContent);
						
								function loadContent() {
									$('.worksajax').load(toLoad, '', showNewContent)
								}
						
								function showNewContent() {
									$.getScript("<?php echo get_template_directory_uri(); ?>/js/portfolio.js");
									$('.worksajax').slideDown('slow');
						
								}
						
								return false;
						
						
							});
						
						});
					})(jQuery);
		</script>
<?php }
}