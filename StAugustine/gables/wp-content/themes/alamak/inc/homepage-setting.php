<?php 
//slider supersized function
function rdn_supersized_style()  {
	wp_register_style
    ('supersized', 
    get_template_directory_uri() . '/css/supersized.css', 
    array(), 
    '1', 
    'all' );
	
	wp_register_style
    ('supersized-shutter', 
    get_template_directory_uri() . '/css/supersized.shutter.css', 
    array(), 
    '1', 
    'all' );
	if (is_page_template( 'homepage-slider.php' ) || ( is_page_template( 'homepage-custom.php' ) && (ot_get_option( 'home_setting' ) == 'slider_bg_home') ) ){
	wp_enqueue_style( 'supersized' );
	wp_enqueue_style( 'supersized.shutter' );
	}
}

function rdn_youtube_bg_image() {
	if  ( function_exists( 'ot_get_option' )){
			$youtube_bg = ot_get_option( 'youtube_bg_image' ); 
			$youtube_bg_css = "
			@media (max-width:979px){
			#home{
				background-image: url('$youtube_bg');
				background-position: center center;
				background-repeat: no-repeat;
				background-size: auto auto;
				}
			}
			";
			if ( function_exists( 'ot_get_option' ) && ot_get_option( 'youtube_bg_image' )) {    
				if (is_page_template( 'homepage-youtube.php' )|| ( is_page_template( 'homepage-custom.php' ) && (ot_get_option( 'home_setting' ) == 'youtube_bg_home') ) ){         
					wp_add_inline_style( 'custom-style', $youtube_bg_css );
				}
			}
	}
	
	wp_enqueue_style( 'rdn_youtube_bg_image' );
	
}
	
function rdn_homepage_script() {
	if (is_page_template( 'homepage-slider.php' )|| ( is_page_template( 'homepage-custom.php' ) && (ot_get_option( 'home_setting' ) == 'slider_bg_home') )  ){
		wp_enqueue_script( 'supersized', get_template_directory_uri() . '/js/supersized.3.2.7.min.js',array(),'', 'in_footer');
		wp_enqueue_script( 'supersized_shutter', get_template_directory_uri() . '/js/supersized.shutter.js',array(),'', 'in_footer');
		wp_enqueue_script( 'testimonial_ticker', get_template_directory_uri() . '/js/testimonial.js',array(),'', 'in_footer');
	} else if (is_page_template( 'homepage-video.php' ) || ( is_page_template( 'homepage-custom.php' ) && (ot_get_option( 'home_setting' ) == 'video_bg_home') )  ){
		wp_enqueue_script( 'jquery_ui', get_template_directory_uri() . '/js/jquery-ui-1.8.22.custom.min.js',array(),'', 'in_footer');
		wp_enqueue_script( 'images-loaded', get_template_directory_uri() . '/js/jquery.imagesloaded.min.js',array(),'', 'in_footer');
		wp_enqueue_script( 'video', get_template_directory_uri() . '/js/video.js',array(),'', 'in_footer');
		wp_enqueue_script( 'bigvideo', get_template_directory_uri() . '/js/bigvideo.js',array(),'', 'in_footer');
		wp_enqueue_script( 'testimonial_ticker', get_template_directory_uri() . '/js/testimonial.js',array(),'', 'in_footer');
	} else if (is_page_template( 'homepage-youtube.php' ) || ( is_page_template( 'homepage-custom.php' ) && (ot_get_option( 'home_setting' ) == 'youtube_bg_home') ) ){
		wp_enqueue_script( 'tubular', get_template_directory_uri() . '/js/jquery.tubular.1.0.js',array(),'', 'in_footer');
		wp_enqueue_script( 'testimonial_ticker', get_template_directory_uri() . '/js/testimonial.js',array(),'', 'in_footer');
	} 
}


//Homepage Setting
function rdn_homepage_setting() {
	if  ( function_exists( 'ot_get_option' )){
		if (is_page_template( 'homepage-slider.php' ) || ( is_page_template( 'homepage-custom.php' ) && (ot_get_option( 'home_setting' ) == 'slider_bg_home') ) ){
			$slides = ot_get_option( 'slider_setting', array() );
			
				  ?>
					<script type="text/javascript">
						(function ($) {
						"use strict";
						jQuery(document).ready(function($) {
						
							$.supersized({
						
								// Functionality
								slide_interval: 7000, // Length between transitions
								transition: 1, // 0-None, 1-Fade, 2-Slide Top, 3-Slide Right, 4-Slide Bottom, 5-Slide Left, 6-Carousel Right, 7-Carousel Left
								transition_speed: 600, // Speed of transition
						
								// Components							
								slide_links: 'false', // Individual links for each slide (Options: false, 'num', 'name', 'blank')
								slides: [ // Slideshow Images
									
										<?php
										if ( ! empty( $slides ) ) {
				foreach( $slides as $slide ) { ?> {
										image: '<?php echo $slide['slider_image']  ?>',
										title: '<h2><?php echo $slide['title']  ?></h2>' }, <?php } ?>
									
								]
						
							});
						});
						})(jQuery);
					</script>
					<?php ;
				}
		}	
		else if (is_page_template( 'homepage-video.php' ) || ( is_page_template( 'homepage-custom.php' ) && (ot_get_option( 'home_setting' ) == 'video_bg_home') ) ){ ?>
		<script type="text/javascript">
						(function ($) {
						"use strict";
						
							var BV = new $.BigVideo();
							BV.init();
							if (Modernizr.touch) {
								BV.show("<?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'video_image' )) { echo ot_get_option( 'video_image' ); } ?>");
							} else {
								BV.show("<?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'video_path' )) { echo ot_get_option( 'video_path' ); } ?>",{ambient:true});
							} 
							})(jQuery);
						
		</script>
		<?php } else if (is_page_template( 'homepage-youtube.php' )|| ( is_page_template( 'homepage-custom.php' ) && (ot_get_option( 'home_setting' ) == 'youtube_bg_home') )  ){ ?>
			<script type="text/javascript">
			(function ($) {
			"use strict";
				$(document).ready(function () {
					$().ready(function() {
							$('#home').tubular({videoId: '<?php 
							if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'youtube_video_bg_link' )) { echo ot_get_option( 'youtube_video_bg_link' ); } 
							else { echo 'c9MnSeYYtYY'; }?>'});
					}); 
				});
			})(jQuery);
		</script>
		<?php } 
	}
}
	
