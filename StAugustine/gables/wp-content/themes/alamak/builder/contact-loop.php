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
                        <div class="map_canvas"></div>
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