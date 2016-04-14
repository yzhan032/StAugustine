<?php  
/*
* Template Name: Alamak Homepage Youtube Video
* Description: Alamak Homepage Youtube Video
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
                            <div id="slidecaption">
                                <h2>
                                <?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'caption_text_bottom' )) { echo ot_get_option( 'caption_text_bottom' ); 
								} else { echo "Bunch of Creative<br/>People & Idea";} ?>
                                </h2>
                            </div><!--/slidecaption-->
                            <div class="centering">
                                <div id="progress-back">
                                    <div id="progress-bar"></div>
                                </div><!--/progress-back-->
                            </div><!--/.centering-->
                        </div><!--/.slider-content-->
                    </div><!--/.col-sm-6-->  
                	
                </div><!--/row-->
            </div><!--/container-->
        </section><!--/home-->
        <!--HOME SECTION END-->
        
        <!--PORTFOLIO SECTION START--> 
        <section id="portfolio" class="clearfix content">
        	<div class="container">
            	<div class="title-content clearfix">
                	<div class="title-inner">
                    	<h3><?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'portfolio_title' )) { echo ot_get_option( 'portfolio_title' ); } else { echo "Our Recent Works";} ?></h3>
                        <p><?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'portfolio_small' )) { echo ot_get_option( 'portfolio_small' ); } else { echo "What we've done so far";} ?></p>
                    </div><!--/.title-inner-->
                </div><!--/.title-content-->
                
                <!--AJAX PORTFOLIO WILL SHOWN HERE-->
                <div id="workslist">
                     <div class="clearfix worksajax">
                     </div><!--/.worksajax -->
                </div><!--/workslist-->
                <!--AJAX PORTFOLIO END-->
                
                <div class="row">
                    <!--PORTFOLIO LOOP START-->
                    <?php  
					$ports = new WP_Query(array(  
							'post_type' =>  'portfolio',  
							'posts_per_page'  =>'-1'  
						)  
					);  
					
					if ($ports->have_posts()) : while  ($ports->have_posts()) : $ports->the_post();
					?>
                    <div class="col-md-6 port-item" id="post-<?php the_ID(); ?>">
                        <div class="port-inner white-bg clearfix">
                        	<!--get original featured image link-->
                        	<?php
							$image_url = wp_get_attachment_image_src( get_post_thumbnail_id($post->ID), 'full');
							?>
                            <a class="box-40 hovers bwWrapper" data-rel="prettyPhoto" href="<?php echo $image_url[0];  ?>" >
                            	<?php the_post_thumbnail('portfolio-image'); ?>
                            </a>
                            <a class="more" href="#" data-link="<?php the_permalink(); ?>"><span class="hov-desc">View Detail</span><i class="fa fa-chain"></i></a>
                            <div class="padding box-60">
                                <h3><?php the_title(); ?></h3>
                                <?php the_excerpt(); ?>
                                <?php  if ( has_post_format( 'video' )) { ?>
                                <i class="cat-icon fa fa-film" data-link="<?php the_permalink(); ?>"></i>
                                <?php } else if( has_post_format( 'gallery' )) { ?>
                                <i class="cat-icon fa fa-picture-o" data-link="<?php the_permalink(); ?>"></i>
                                <?php } else if( has_post_format( 'audio' )) { ?>
                                <i class="cat-icon fa fa-headphones" data-link="<?php the_permalink(); ?>"></i>
                                <?php } else { ?>
                                <i class="cat-icon fa fa-html5" data-link="<?php the_permalink(); ?>"></i>
                                <?php } ?>
                            </div><!--/.padding-->
                        </div><!--/.port-inner-->
                    </div><!--/.col-md-6-->
                     
                    <?php endwhile; endif; wp_reset_postdata();?>
                	<!--PORTFOLIO LOOP END-->
                    
                    
                </div><!--/.row-->    
            </div><!--/.container-->
        </section><!--/portfolio-->
  		<!--PORTFOLIO SECTION END-->
         
        <!--TEXT WITH DARK BG START--> 
		<section class="bg-dark bg1">
        	<div class="dark-mask"></div>
        	<div class="container align-center">
                <h3 class="slogan"><?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'para_quote_text' )) { echo ot_get_option( 'para_quote_text' ); }?></h3>
                <div class="slogan-divider"><span class="line-left"></span><span class="fa fa-quote-left"></span><span class="line-right"></span></div>
                <h4 class="author-slogan"><?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'para_quote_author' )) { echo ot_get_option( 'para_quote_author' ); }?></h4>
            </div><!--/.container-->
        </section><!--/.bg-dark-->
        <!--TEXT WITH DARK BG END-->
        
        <!--ABOUT SECTION START-->
        <section id="about" class="clearfix content">
        	<div class="container">
            	<div class="title-content clearfix">
                	<div class="title-inner">
                    	<h3><?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'about_title' )) { echo ot_get_option( 'about_title' ); } else { echo "Story About Us";} ?></h3>
                        <p><?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'about_small' )) { echo ot_get_option( 'about_small' ); } else { echo "The Things you Should Know";} ?></p>
                    </div><!--/.title-inner-->
                </div><!--/.title-content-->
                
                <!--ABOUT TAB START-->
            	<div id="tab-about" class="tab-content white-bg">
                  <!--TAB CONTENT START-->
                   <?php  
					$abouts = new WP_Query(array(  
							'post_type' =>  'about-us',  
							'posts_per_page'  =>'-1'  ,
							'order' => 'ASC'
						)  
					);  
					$i=1;
					if ($abouts->have_posts()) : while  ($abouts->have_posts()) : $abouts->the_post();
					?>
                  <div class="tab-pane fade <?php if ($i==1) { echo 'in active'; } ?>" id="post-<?php the_ID(); ?>">
                  	 <?php if ( has_post_format( 'video' )) { ?>
                     <div class="video clearfix">
                     	<iframe width="570" 
                        src="<?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'embed_post', true)); ?>?wmode=opaque;vq=hd720;rel=0;showinfo=0;controls=0" height="300">
                        </iframe> 
                     </div>  
                     <?php } else if( has_post_format( 'gallery' )) { ?>       
                     <div class="clearfix gallery-inner">
                        <?php echo apply_filters('the_content', get_post_meta($post->ID, 'gallery_port', true)); ?>
                     </div>
                     
                     <?php } else if( has_post_format( 'audio' )) { ?>
                     <div class="clearfix audio">
                         <div class="audio-inner">
                             <?php echo apply_filters('the_content', get_post_meta($post->ID, 'embed_post', true)); ?>
                         </div>
                     </div>
                     <?php } else { ?>
                        <?php  the_post_thumbnail(); ?>
                     <?php } ?> 
                     <div class="white-bg clearfix">
                         <div class="row clearfix">
                             <div class="col-md-4">
                                 <div class="padding">
                                     <p class="black-text"><?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'black_text', true)); ?></p>
                                     <p class="gray-text"><?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'grey_text', true)); ?></p>
                                     <h2 class="content-title"><?php the_title(); ?></h2>
                                 </div><!--/.padding-->
                             </div><!--/col-md-4-->
                             <div class="col-md-8">
                                 <div class="padding">
                                     <?php the_content() ?>
                                 </div><!--/.padding-->
                                 <div class="spacing40 clearfix"></div>
                              </div><!--/col-md-8-->
                     	</div><!--/.row-->
                     </div><!--/.white-bg--> 
                  </div><!--/.tab-pane-->
                  <?php $i++; endwhile; endif; wp_reset_query();?>
                  <!--TAB CONTENT END-->
                  
                  
                  
                </div><!--/.tab-content-->
                <!--ABOUT TAB END-->
                
                <!--ABOUT TAB NAVIGATION START-->
                <div class="white-bg clearfix">
                	<ul id="nav-about" class="nav margin align-right nav-tabs">
						<?php  
						$i=1;  
                        if ($abouts->have_posts()) : while  ($abouts->have_posts()) : $abouts->the_post();
                        ?>
                      <li class="<?php if ($i==1) { echo 'in active'; } ?>">
                      	<a href="#post-<?php the_ID(); ?>" data-toggle="tab"><?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'tab_title', true)); ?></a>
                      </li>
                      <?php $i++; endwhile; endif; wp_reset_query();?>
                    </ul>
                </div>
                <!--ABOUT TAB NAVIGATION END-->
                
                
                <!--TEAM LIST START-->
                <div class="row" id="team">
                	<?php  
					$teams = new WP_Query(array(  
							'post_type' =>  'team',  
							'posts_per_page'  =>'-1'  ,
							'order' => 'ASC'
						)  
					);  
					if ($teams->have_posts()) : while  ($teams->have_posts()) : $teams->the_post();
					?>
                	<div class="col-md-4 team-box">
                    	<a href="#" class="bwWrapper team-ajax" data-link="<?php the_permalink(); ?>">
                    		<?php the_post_thumbnail('team-image'); ?>
                        </a>
                        <div class="clearfix team-inner white-bg padding">
                        	<h3 class="team-name"><?php the_title(); ?></h3>
                            <p class="team-position"><?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'grey_text', true)); ?></p>
                            <i class="fa <?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'team_icon', true)); ?> team-icon" data-link="<?php the_permalink(); ?>"></i>
                        </div><!--/.white-bg-->
                    </div><!--/.col-md4-->
                    <?php endwhile; endif; wp_reset_query();?>
                    
                </div><!--/.row-->
                <!--TEAM LIST END-->
                
                <div class="spacing40 clearfix"></div>
                
                <!--AJAX TEAM WILL SHOW HERE-->
                <div id="teamlist">
                	<div class="clearfix teamajax"> </div>
                </div>
                <!--AJAX TEAM END-->

            </div><!--/.container-->
        </section><!--/about-->
		<!--ABOUT SECTION END-->
        
        <!--TESTIMONIAL WITH DARK BG START-->
		<section class="bg-dark bg2">
        	<div class="dark-mask"></div>
        	<div class="container align-center">
            <ul class="testimonial">
            	<!--TESTIMONIAL LOOP START-->
                <?php  
				$testi = new WP_Query(array(  
						'post_type' =>  'testimonial',  
						'posts_per_page'  =>'-1'  
					)  
				); 
				if ($testi->have_posts()) : while  ($testi->have_posts()) : $testi->the_post(); ?>
            	<li>
                    <h3 class="slogan"> <?php echo apply_filters("get_the_content",$post->post_content); ?></h3>
                    <div class="slogan-divider">
                    	<span class="line-left"></span>
                        <div class="img-border"><?php the_post_thumbnail('testi-image', array('class' => 'testi-img')); ?></div>
                        <span class="line-right"></span>
                    </div>
                    <h4 class="author-slogan"><?php the_title(); ?></h4>
                    <p><?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'client_position', true)); ?></p>
                </li>
				<?php  endwhile; endif; wp_reset_query();?>
                <!--TESTIMONIAL LOOP END-->
            </ul><!--/.testimonial-->
            </div><!--/.container-->
        </section><!--/.bg-dark-->
        <!--TESTIMONIAL WITH DARK BG END-->
        
        <!--SERVICES SECTION START-->
        <section id="services" class="clearfix content">
        	<div class="container">
            	<div class="title-content clearfix">
                	<div class="title-inner">
                    	<h3><?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'services_title' )) { echo ot_get_option( 'services_title' ); } else { echo "Our Services";} ?></h3>
                        <p><?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'services_small' )) { echo ot_get_option( 'services_small' ); } else { echo "What we really can provide";} ?></p>
                    </div><!--/.title-inner-->
                </div><!--/.title-content-->
                
                <!--SERVICES TAB START-->
            	<div id="tab-services" class="tab-content white-bg">
                  <!--TAB CONTENT START-->
                   <?php  
					$services = new WP_Query(array(  
							'post_type' =>  'services',  
							'posts_per_page'  =>'-1'  ,
							'order' => 'ASC'
						)  
					);  
					$i=1;
					if ($services->have_posts()) : while  ($services->have_posts()) : $services->the_post();
					?>
                  <div class="tab-pane fade <?php if ($i==1) { echo 'in active'; } ?>" id="post-<?php the_ID(); ?>">
                  	 <?php if ( has_post_format( 'video' )) { ?>
                     <div class="video clearfix">
                     	<iframe width="570" 
                        src="<?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'embed_post', true)); ?>?wmode=opaque;vq=hd720;rel=0;showinfo=0;controls=0" height="300">
                        </iframe> 
                     </div>  
                     <?php } else if( has_post_format( 'gallery' )) { ?>       
                     <div class="clearfix gallery-inner">
                        <?php echo apply_filters('the_content', get_post_meta($post->ID, 'gallery_port', true)); ?>
                     </div>
                     
                     <?php } else if( has_post_format( 'audio' )) { ?>
                     <div class="clearfix audio">
                         <div class="audio-inner">
                             <?php echo apply_filters('the_content', get_post_meta($post->ID, 'embed_post', true)); ?>
                         </div>
                     </div>
                     <?php } else { ?>
                        <?php  the_post_thumbnail(); ?>
                     <?php } ?> 
                     <div class="white-bg clearfix">
                         <div class="row clearfix">
                             <div class="col-md-4">
                                 <div class="padding">
                                     <p class="black-text"><?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'black_text', true)); ?></p>
                                     <p class="gray-text"><?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'grey_text', true)); ?></p>
                                     <h2 class="content-title"><?php the_title(); ?></h2>
                                 </div><!--/.padding-->
                             </div><!--/col-md-4-->
                             <div class="col-md-8">
                                 <div class="padding">
                                     <?php the_content() ?>
                                 </div><!--/.padding-->
                                 <div class="spacing40 clearfix"></div>
                              </div><!--/col-md-8-->
                     	</div><!--/.row-->
                     </div><!--/.white-bg--> 
                  </div><!--/.tab-pane-->
                  <?php $i++; endwhile; endif; wp_reset_query();?>
                  <!--TAB CONTENT END-->
                  
                </div><!--/.tab-content-->
                <!--SERVICES TAB END-->
                
                <!--SERVICES TAB NAVIGATION START-->
                <div class="white-bg clearfix">
                	<ul id="nav-services" class="nav margin align-right nav-tabs">
						<?php  
						$i=1;  
                        if ($services->have_posts()) : while  ($services->have_posts()) : $services->the_post();
                        ?>
                      <li class="<?php if ($i==1) { echo 'in active'; } ?>">
                      	<a href="#post-<?php the_ID(); ?>" data-toggle="tab"><?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'tab_title', true)); ?></a>
                      </li>
                      <?php $i++; endwhile; endif; wp_reset_query();?>
                    </ul>
                </div>
                <!--SERVICES TAB NAVIGATION END-->
                
                <div class="spacing40 clearfix"></div>
                
                <?php  if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'pricing_display' ) == 'yes') { 
                echo '<div class="row">';
                	
                	/*--PRICING TABLE LOOP START--*/
					  
                    $prices = new WP_Query(array(  
                                'post_type' =>  'pricing-table',  
                                'posts_per_page'  =>'-1'  ,
                                'order' => 'ASC'
                            )  
                    );
                    if ($prices->have_posts()) : while  ($prices->have_posts()) : $prices->the_post();
					$pricing = ot_get_option( 'pricing_col' ); 
                    if  ( $pricing  == '4') { echo '<div class="col-sm-6 col-md-3">'; } 
					else if ( $pricing  == '3') { echo '<div class="col-sm-6 col-md-4">'; }
					else if ( $pricing  == '2') { echo '<div class="col-sm-6 col-md-6">'; }
						
						 $p_type = apply_filters('get_the_content', get_post_meta($post->ID,  'pricing_table_type_meta_box' ,true ));
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
                    <?php endwhile; endif; wp_reset_query();  ?>
                    <!--PRICING TABLE LOOP END-->
                    <div class="spacing40 clearfix"></div>
                   
                </div><!--/.row-->
                <?php } ?>
                
            </div><!--/.container-->
            <div class="spacing40 clearfix"></div>
        </section><!--services-->
        <!--SERVICES SECTION START-->
        
        <!--TWITTER SECTION START-->
        <section class="bg-dark twitter-feed clearfix">
        	<div class="dark-mask"></div>
        	<div class="container align-center">
            	<div class="line-top clearfix"></div>
                <?php if  ( function_exists( 'ot_get_option' )){ $twits = ot_get_option( 'twit_shortcode' ); echo do_shortcode( $twits ); } ?>
                <div class="slogan-divider"><span class="line-left"></span><span class="fa fa-twitter"></span><span class="line-right"></span></div>
                <a href="<?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'twitter_feed_link' )) { echo ot_get_option( 'twitter_feed_link' ); } ?>" class="tweet-follow">
					<?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'twitter_feed_text' )) { echo ot_get_option( 'twitter_feed_text' ); } else { echo "Follow Us";} ?>
                </a>
            </div><!--/.container-->
        </section><!--/.bg-dark-->
        <!--TWITTER SECTION END-->

        <!--CONTACT SECTION START-->
        <section id="contact" class="clearfix content">
        	<div class="container">
            	<div class="title-content clearfix">
                	<div class="title-inner">
                    	<h3><?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'contact_title' )) { echo ot_get_option( 'contact_title' ); } else { echo "Our Contact";} ?></h3>
                        <p><?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'contact_small' )) { echo ot_get_option( 'contact_small' ); } else { echo "Start communicate with us";} ?></p>
                    </div><!--/.title-inner-->
                </div><!--/.title-content-->
                
                  <!--CONTACT TAB START-->
                  <div id="tab-contact" class="tab-content white-bg">
                  <!--TAB CONTENT START-->
                   <?php  
					$contacts = new WP_Query(array(  
							'post_type' =>  'contact',  
							'posts_per_page'  =>'-1'  ,
							'order' => 'ASC'
						)  
					);  
					$i=1;
					if ($contacts->have_posts()) : while  ($contacts->have_posts()) : $contacts->the_post();
					$gmaps = apply_filters('get_the_content', get_post_meta($post->ID, 'gmap_option', true))
					?>
                  <div class="tab-pane fade <?php if ($i==1) { echo 'in active'; } ?>" id="post-<?php the_ID(); ?>">
                  	 <?php if ( has_post_format( 'video' ) && $gmaps == 'no' ) { ?>
                     <div class="video clearfix">
                     	<iframe width="570" 
                        src="<?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'embed_post', true)); ?>?wmode=opaque;vq=hd720;rel=0;showinfo=0;controls=0" height="300">
                        </iframe> 
                     </div>  
                     <?php } else if( has_post_format( 'gallery' ) && $gmaps == 'no' ) { ?>       
                     <div class="clearfix gallery-inner">
                        <?php echo apply_filters('the_content', get_post_meta($post->ID, 'gallery_port', true)); ?>
                     </div>
                     
                     <?php } else if( has_post_format( 'audio' ) && $gmaps == 'no' ) { ?>
                     <div class="clearfix audio">
                         <div class="audio-inner">
                             <?php echo apply_filters('the_content', get_post_meta($post->ID, 'embed_post', true)); ?>
                         </div>
                     </div>
                     <?php } else if( $gmaps == 'yes' ){ ?>
                        <?php  echo do_shortcode('[g_maps]'); ?>
                     <?php } else { ?>
                        <?php  the_post_thumbnail(); ?>
                     <?php } ?> 
                     <div class="white-bg clearfix">
                         <div class="row clearfix">
                             <div class="col-md-4">
                                 <div class="padding">
                                     <p class="black-text"><?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'black_text', true)); ?></p>
                                     <p class="gray-text"><?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'grey_text', true)); ?></p>
                                     <h2 class="content-title"><?php the_title(); ?></h2>
                                 </div><!--/.padding-->
                             </div><!--/col-md-4-->
                             <div class="col-md-8">
                                 <div class="padding">
                                     <?php the_content() ?>
                                 </div><!--/.padding-->
                                 <div class="spacing40 clearfix"></div>
                              </div><!--/col-md-8-->
                     	</div><!--/.row-->
                     </div><!--/.white-bg--> 
                  </div><!--/.tab-pane-->
                  <?php $i++; endwhile; endif; wp_reset_query();?>
                  <!--TAB CONTENT END-->
                  
                </div><!--/.tab-content-->
                <!--CONTACT TAB END-->
                
                <!--CONTACT NAVIGATION TAB START-->
                <div class="white-bg clearfix">
                	<ul id="nav-contact" class="nav margin align-right nav-tabs">
						<?php  
						$i=1;  
                        if ($contacts->have_posts()) : while  ($contacts->have_posts()) : $contacts->the_post();
                        ?>
                      <li class="<?php if ($i==1) { echo 'in active'; } ?>">
                      	<a href="#post-<?php the_ID(); ?>" data-toggle="tab"><?php echo apply_filters('get_the_content', get_post_meta($post->ID, 'tab_title', true)); ?></a>
                      </li>
                      <?php $i++; endwhile; endif; wp_reset_query();?>
                    </ul>
                </div>
                <!--CONTACT NAVIGATION TAB END-->
                
            </div><!--/.container-->
        </section><!--contact-->
        <!--CONTACT SECTION END-->
        
<?php  get_footer(); ?>

