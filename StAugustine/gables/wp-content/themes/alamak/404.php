<?php get_header(); ?>

	<!--HOME SECTION START-->
    	<section id="home" class="clearfix dark-bg">
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
                            <?php wp_nav_menu( array( 'items_wrap' => '<ul id="%1$s" class="navigation desktop-menu %2$s">%3$s</ul>', 'theme_location' => 'header-menu-blog' ) ); ?>
                            <ul class="nav-collapse mobile-menu"></ul>
                            <!--NAVIGATION END-->
                             
                             
                        </div><!--/.col-md-4-->
                    </div><!--/.row-->
                </div><!--/.container-->   
            </div><!--/.header-->
        </section><!--/home-->
        <!--HOME SECTION END-->
    <div class="clearfix white-bg">   
        <div class="spacing80"></div>        
        <div class="container">
            <div class="row">
                <div class="col-md-9">
                <div class="padding white-bg align-center">
                    <div class="spacing80 clearfix"></div>
                        <h2 class="big-404">404!</h2>
                        <h3 class="title-404"><i class="fa fa-exclamation-circle"></i> Im sorry, page not found</h3>
                        <p class="text-404">return to  <a href="<?php echo home_url(); ?>">homepage</a> now!</p>
                    <div class="spacing80 clearfix"></div>
                </div><!--/.padding-->
                </div><!--/.col-md-9-->
                <?php get_sidebar(); ?> 
            </div><!--/.row-->
        </div><!--/.container-->
        <div class="spacing80"></div>
    </div><!--/.white-bg-->
    

    
<?php get_footer(); ?> 