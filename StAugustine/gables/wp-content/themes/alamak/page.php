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
        
        <div class="white-bg clearfix">
            <div class="container">
                <div class="spacing80"></div>   
                <div class="row">
                    <div class="col-md-9">
                        <?php while (have_posts()) : the_post(); ?>
                            <article id="post-<?php the_ID(); ?>" <?php post_class('clearfix post-content white-bg'); ?>>
                                <div class="post-image clearfix">
                                    <?php the_post_thumbnail(); ?>
                                    <div class="mask-post">
                                        <div class="mask-inner">
                                            <a title ="<?php the_title (); ?>" href="<?php $src = wp_get_attachment_image_src(get_post_thumbnail_id($post->ID), 'full', false, ''); echo $src[0]; ?>" 
                                            data-rel="prettyPhoto">
                                            <i class="fa fa-search"></i>
                                            </a> 
                                        </div><!--/.mask-inner-->
                                    </div><!--/.mask-post-->
                                </div><!--/.post-image-->
                                <div class="post-inner clearfix"> 
                                     <div class="padding">
                                         <p class="black-text"><?php the_time(get_option('date_format')); ?></p>
                                         <p class="gray-text"><?php the_category(', '); ?></p>
                                         <h2 class="content-title"><?php the_title (); ?></h2>
                                         <div class="spacing40"></div>
                                         <?php the_content() ; ?>
                                         <div class="spacing40 clearfix"></div>
                                     </div><!--/.padding-->
                                     
                                     <div class="margin align-right">
                                     	<div class="post-pager clearfix">
											<?php wp_link_pages(); ?>
                                        </div>
                                     </div> <!--/.margin-->
                                </div><!--/.post-inner-->
                            </article><!--/.post-content-->
                            
                            <div class="spacing40"></div>
                        
                        <?php if ( comments_open() ) { ?>
                            <div id="comments" class="comments clearfix"><?php comments_template(); ?></div>
                        <?php } ?>
                        <?php  endwhile; ?>
                    </div><!--/.span8-->  
                    <?php get_sidebar(); ?>
                </div><!--/.row-->
            </div><!--/.container-->
            <div class="spacing80 clearfix"></div>
		</div><!--/.white-bg-->
<?php  get_footer(); ?>