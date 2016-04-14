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
                            <?php  if ( has_post_format( 'video' )) { ?>
                            <article id="post-<?php  the_ID(); ?>" <?php  post_class('clearfix post-content white-bg'); ?>>
                                <div class="video clearfix">
                                    <iframe width="570" src="<?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'embed_post', true)); ?>?wmode=opaque;vq=hd720;rel=0;showinfo=0;" height="300"></iframe> 
                                </div> 
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
                                         <p>
                                         <span class="strong">Tags:</span>
                                         <span class="condensed"><?php the_tags('', ', '); ?></span>
                                         </p>
                                     </div> <!--/.margin-->
                                </div><!--/.post-inner-->
                            </article><!--/.post-content-->
                                  
                            <?php } else if ( has_post_format( 'audio' )) { ?>
                            <article id="post-<?php the_ID(); ?>" <?php post_class('clearfix post-content white-bg'); ?>> 
                                <div class="audio clearfix">
                                    <?php echo apply_filters('the_content', get_post_meta($post->ID, 'embed_post', true)); ?>
                                </div><!--/.audio-->
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
                                         <p>
                                         <span class="strong">Tags:</span>
                                         <span class="condensed"><?php the_tags('', ', '); ?></span>
                                         </p>
                                     </div> <!--/.margin-->
                                </div><!--/.post-inner-->
                            </article><!--/.post-content-->
                            
                            <?php } else if ( has_post_format( 'quote' )) { ?>
                            <article id="post-<?php the_ID(); ?>" <?php post_class('clearfix post-with-quote white-bg'); ?>>
                                <blockquote>
                                    <?php the_content(); ?>
                                    <small><?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'quote_author', true)); ?></small>
                                </blockquote>
                                <i class="fa fa-quote-right quote-icon"></i> 
                            </article><!--/.quote--> 
                            
                            <?php } else if ( has_post_format( 'link' )) { ?>
                            <article id="post-<?php the_ID(); ?>" <?php post_class('clearfix link white-bg'); ?>>
                                <div class="link-image clearfix">
                                    <?php $linkcode = apply_filters('get_the_content', get_post_meta($post->ID, 'link_post_url', true)); ?>
                                    <a href="<?php echo $linkcode ?>" target="_blank">
                                    <div class="image-link"><?php the_post_thumbnail(); ?></div>
                                    <p><i class="fa fa-link"></i> <?php echo $linkcode ?></p>
                                    </a>
                                </div><!--/.link-image-->  
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
                                         <p>
                                         <span class="strong">Tags:</span>
                                         <span class="condensed"><?php the_tags('', ', '); ?></span>
                                         </p>
                                     </div> <!--/.margin-->
                                </div><!--/.post-inner-->
                            </article><!--/.link-->   
                                  
                            <?php } else if ( has_post_format( 'aside' )) { ?>
                            <article id="post-<?php the_ID(); ?>" <?php post_class('clearfix aside-body white-bg'); ?>>
                                <div class="image-aside"><?php the_post_thumbnail(); ?></div>
                                <aside class="aside">
                                    <div class="aside-title clearfix">
                                        <h2 class="content-title"><?php the_title (); ?></h2>
                                        <div class="spacing40"></div>
                                    </div><!--/.post-title--> 
                                    <?php the_content(); ?>
                                    <div class="spacing40"></div>
                                </aside><!--/.aside-->
                            </article><!--/.aside-body-->  
                                       
                            <?php } else if ( has_post_format( 'gallery' )) { ?>
                            <article id="post-<?php the_ID(); ?>" <?php post_class('clearfix gallery-content white-bg'); ?>>
                                <div class="gallery-inner clearfix">
                                    <?php echo apply_filters('the_content', get_post_meta($post->ID, 'gallery_post', true)); ?>
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
                                         <p>
                                         <span class="strong">Tags:</span>
                                         <span class="condensed"><?php the_tags('', ', '); ?></span>
                                         </p>
                                     </div> <!--/.margin-->
                                </div><!--/.post-inner-->
                            </article><!--/.gallery-content-->
                                     
                            <?php } else { ?>
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
                                         <p>
                                         <span class="strong">Tags:</span>
                                         <span class="condensed"><?php the_tags('', ', '); ?></span>
                                         </p>
                                     </div> <!--/.margin-->
                                </div><!--/.post-inner-->
                            </article><!--/.post-content-->
         
                            <?php  } ?>
                            
                            <div class="spacing40"></div>
                        
                        <?php if ( comments_open() ) { ?>
                            <div id="comments" class="comments clearfix"><?php comments_template(); ?></div>
                        <?php } ?>
                        <div class="spacing40"></div>
                        <div class="post-pagination clearfix">
                            <div class="pull-left"><?php previous_post_link('%link','&larr; Previous Post', '0'); ?></div>
                            <div class="pull-right"><?php next_post_link('%link','Next Post &rarr;', '0'); ?> </div>
                        </div>
                        <?php  endwhile; ?>
                    </div><!--/.span8-->  
                    <?php get_sidebar(); ?>
                </div><!--/.row-->
            </div><!--/.container-->
            <div class="spacing80 clearfix"></div>
		</div><!--/.white-bg-->
<?php  get_footer(); ?>