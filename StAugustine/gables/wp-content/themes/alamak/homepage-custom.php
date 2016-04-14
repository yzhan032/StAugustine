<?php  
/*
* Template Name: Alamak Homepage Custom Layout
* Description: Alamak Homepage Custom Layout
*/
get_header(); ?>

        <!--HOME SECTION START-->
    	<section id="home" class="clearfix">
        	<div class="header clearfix">
                <div class="container">
                    <div class="row">
                        <div class="col-xs-5 col-sm-4">
							<?php if ( function_exists( 'ot_get_option' ) ) :if (ot_get_option( 'logo_web')) :  ?>
                                <a  class="logo" href=" <?php  echo home_url(); ?> "><img src='<?php echo ot_get_option( 'logo_web' ); ?>' alt='logo'></a>
                            <?php  else :  ?>
                                <a  class="logo" href=" <?php  echo home_url(); ?> "><img src="<?php echo get_template_directory_uri().'/images/logo.png' ?>" alt="logo"/></a>
                            <?php endif ; endif; ?>
                        </div><!--/.col-md-4-->
                        <div class="col-md-8">
                        	 <!--MENU BUTTON(ON TABLET/MOBILE) START-->
                        	<div class="menu-btn" data-target=".nav-collapse" data-toggle="collapse">
                                <span class="fa fa-th"></span>
                            </div>
                            <!--MENU BUTTON END-->
                            
                            <!--NAVIGATION START-->
                            <?php wp_nav_menu( array( 'items_wrap' => '<ul id="%1$s" class="navigation desktop-menu %2$s">%3$s</ul>', 'theme_location' => 'header-menu' ) ); ?>
                            <ul class="nav-collapse mobile-menu"></ul>
                            <!--NAVIGATION END-->
                             
                             
                        </div><!--/.col-md-4-->
                    </div><!--/.row-->
                </div><!--/.container-->   
            </div><!--/.header-->
            <div class="clearfix"></div>
            <div class="container">
            	<div class="row">
                	
                    <div class="col-md-6"> 
                    	<div class="slider-content clearfix">
                            <!--SLIDER CAPTION START-->  
                            <div class="title-caption">
                                <h4><?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'slider_cap_top' )) { echo ot_get_option( 'slider_cap_top' ); 
								} else { echo "We are creative agency";} ?></h4>
                            </div><!--/.title-caption-->
                            <?php if (ot_get_option( 'home_setting' ) == 'slider_bg_home') {?>
                            <div id="slidecaption"></div> 
                            <?php } else { ?>
                            <div id="slidecaption">
                                <h2>
                                <?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'caption_text_bottom' )) { echo ot_get_option( 'caption_text_bottom' ); 
								} else { echo "Bunch of Creative<br/>People & Idea";} ?>
                                </h2>
                            </div><!--/slidecaption-->
							<?php } ?>
                            <!--SLIDER CAPTION END--> 
                            
                            <div class="centering">
                                <div id="progress-back">
                                    <div id="progress-bar"></div><!--SLIDER PROGRESS BAR-->
                                </div>
                            </div><!--/.centering-->
                        </div><!--/.slider-content-->
                    </div><!--/.col-sm-6-->  
                	
                </div><!--/row-->
            </div><!--/container-->
        </section><!--/home-->
        <!--HOME SECTION END-->
        
<!--BUILDER START-->
<?php if ( function_exists( 'ot_get_option' ) ) {
  
  /* get the slider array */
  $sections = ot_get_option( 'custom_layout', array() );
  
  if ( ! empty( $sections ) ) {
    foreach( $sections as $section ) {
		if ( $section['section_type'] == 'about_content' ) {
		   get_template_part( 'builder/about', 'loop' ); 
		} else
		if ( $section['section_type'] == 'contact_content' ) {
		   get_template_part( 'builder/contact', 'loop' ); 
		} else
		if ( $section['section_type'] == 'portfolio_content' ) {
		   get_template_part( 'builder/portfolio', 'loop' ); 
		} else
		if ( $section['section_type'] == 'services_content' ){
			 get_template_part( 'builder/services', 'loop' ); 
		}
		if ( $section['section_type'] == 'twitter_content' ){
			 get_template_part( 'builder/twitter', 'loop' ); 
		}
		if ( $section['section_type'] == 'quote_content' ){
			 get_template_part( 'builder/quote', 'parallax' ); 
		}
		if ( $section['section_type'] == 'testimonial_content' ){
			 get_template_part( 'builder/testimonial', 'loop' ); 
		}
    }
  }
  
}
?>
<!--BUILDER END-->

<?php get_footer(); ?>