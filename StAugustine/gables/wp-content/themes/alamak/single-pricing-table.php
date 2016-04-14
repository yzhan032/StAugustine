<?php  get_header(); ?>

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

    
    <div class="spacing40 clearfix"></div>       
    <div class="container">
				<?php 
                while (have_posts()) :
                the_post();
                ?>
    				<div class="col-md-4 col-md-offset-4"> 
						
						 <?php $p_type = apply_filters('get_the_content', get_post_meta($post->ID,  'pricing_table_type_meta_box' ,true ));
								if ($p_type == 'dark') {
								echo '<div class="padding align-center white-bg pricing-table special">';
                        	}   else { echo '<div class="padding align-center white-bg pricing-table">';
                        } ?>
                    	
                        	<h3><?php the_title(); ?></h3>
                            <i class="price-icon fa <?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'pricing_icon', true)); ?>"></i>
                            <p class="price"><?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'pricing_price', true)); ?></p>
                            <?php the_content(); ?>
                            <div class="spacing40"></div>
                            <a class="view-more" href="<?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'pricing_link', true)); ?>">
                            	<?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'pricing_button', true)); ?>
                            </a>
                        </div><!--/.pricing-table-->
                    </div><!--/col-sm-6-->
                    <?php endwhile;  wp_reset_query();  ?>
                    <!--PRICING TABLE LOOP END-->
                    <div class="spacing40 clearfix"></div>
                   
                </div><!--/.row-->
    </div><!--/.teamajax-->
    <div class="spacing40 clearfix"></div>
    
   
    
    

<?php  get_footer(); ?>