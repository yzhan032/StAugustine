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
    <div class="container worksajax">
    <?php 
    while (have_posts()) :
    the_post();
    ?>
        <div class="white-bg clearfix">
        <div class="row">
         <?php if ( has_post_format( 'video' )) { ?>
            <div class="col-md-12"> 
                <div class="video clearfix">
                <iframe width="570" src="<?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'embed_post', true)); ?>?wmode=opaque;vq=hd720;rel=0;showinfo=0;controls=0" height="300">
                </iframe> 
                </div>  
            </div>
         <?php } else if( has_post_format( 'gallery' )) { ?>       
         <div class="col-md-12 gallery-portfolio">
            <?php echo apply_filters('the_content', get_post_meta($post->ID, 'gallery_port', true)); ?>
         </div>
         
         <?php } else if( has_post_format( 'audio' )) { ?>
         <div class="col-md-12 audio">
             <div class="audio-inner">
                 <?php echo apply_filters('the_content', get_post_meta($post->ID, 'embed_post', true)); ?>
             </div>
         </div>
         <?php } else { ?>
            <div class="col-md-12 work-img"> 
                <?php  the_post_thumbnail('full'); ?>
                
            </div>
          <!--/.span6-->
         <?php } ?>     
          
          	<div class="col-md-4">
                <div class="padding">
                    <p class="port-date"><?php  the_time(get_option('date_format')); ?></p><p class="client-name"><?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'client_name', true)); ?></p>
                    <h2 class="portajax-title"><?php the_title (); ?></h2>
                </div><!--/.padding-->
            </div><!--/col-md-4-->
            <div class="col-md-8">
                <div class="padding">
                     <?php the_content(); ?>
                     <div class="spacing40 clearfix"></div>
                     	<!--Only show button when button link or button name not empty-->
                     	<?php
						 $btn = get_post_meta($post->ID, 'btn_link', true);
						 $btname = get_post_meta($post->ID, 'btn_name', true);
						 if ( $btn != "" &&  $btname != "" ) 
						{ ?>
						  <a class="view-more" href="<?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'btn_link', true)); ?>">
                     		<?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'btn_name', true)); ?>
                     	  </a> 
						  <?php
						}
						
						?>
                     
                </div><!--/.padding-->
            </div><!--/col-md-8-->
            <div class="clearfix"></div>
            <div class="col-md-12">
            	<div class="margin align-right">
                	<a class="close" href="#"><i class="fa fa-times"></i></a>
                    
            		<p>
                        <span class="strong">Category:</span>
                        <?php $taxonomy = 'portfolio_category'; $terms = wp_get_post_terms($post->ID,$taxonomy);  ?> 
                        <span class="condensed"><?php $cats = array();  foreach ( $terms as $term ) { $cats[] =   $term->name ;   } echo implode(', ', $cats);?></span>
                    </p>
                </div><!--/.margin-->
            </div><!--/col-md-12-->
          </div><!--/.row-->
      </div><!--/white-bg-->
      <?php  endwhile; ?> 
    </div><!--/.worksajax-->
    
    
    
    
    

<?php  get_footer(); ?>