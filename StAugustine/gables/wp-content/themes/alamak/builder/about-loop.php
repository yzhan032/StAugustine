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