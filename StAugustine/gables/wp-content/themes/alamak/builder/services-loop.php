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
            
        </section><!--services-->
        <!--SERVICES SECTION START-->