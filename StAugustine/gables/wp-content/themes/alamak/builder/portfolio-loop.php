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
                <ul class="port-filter">
                    	<li><a class="white-btn" data-filter="*" href="#">All</a></li>
                    	<?php
						$taxonomy = 'portfolio_category';
						$terms = get_terms($taxonomy); // Get all terms of a taxonomy
						if ( $terms && !is_wp_error( $terms ) ) :
							foreach ( $terms as $term ) { ?>
                                	<li><a class="white-btn" data-filter=".<?php echo  strtolower(preg_replace('/\s+/', '-', $term->name)); ?>" href="#">
									<?php echo $term->name; ?></a></li>
								<?php } 
						endif;?>
                </ul>
                <div class="row portfolio-body">
                    <!--PORTFOLIO LOOP START-->
                    <?php
					
					$paged = 1;
					if ( get_query_var('paged') ) $paged = get_query_var('paged');
					if ( get_query_var('page') ) $paged = get_query_var('page');
					$i = 0;
					if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'portfolio_home' )) { 
					$ports = new WP_Query( array( 'post_type' => 'portfolio', 'paged' => $paged, 'posts_per_page' =>  ot_get_option( 'portfolio_home' ) ) );
					} else {
					$ports = new WP_Query( array( 'post_type' => 'portfolio', 'paged' => $paged, 'posts_per_page' =>  6 ) );	}
					while ( $ports->have_posts() ) : $ports->the_post();
					// output ?>
                    <?php $terms = get_the_terms( get_the_ID(), 'portfolio_category' ); ?> 
                    
                    <div class="col-md-6 port-item <?php foreach ($terms as $term) { echo  strtolower(preg_replace('/\s+/', '-', $term->name)). ' '; } ?>
                    <?php $allClasses = get_post_class(); foreach ($allClasses as $class) { echo $class . " "; } ?>" id="post-<?php the_ID(); ?>">
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
					<?php
					$i++; endwhile;
					?>
                    <!--PORTFOLIO LOOP END-->
                   
                </div><!--/.row-->   
                <div class="pagination portfolio-page center">
                	<a class="inactive"href="<?php echo get_page_link( ot_get_option( 'portfolio_list' ) ); ?>">
						<?php if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'portfolio_list_title' )) { echo ot_get_option( 'portfolio_list_title' ); } else { echo "View All Works";} ?>
					</a>  
                </div>
            </div><!--/.container-->
        </section><!--/portfolio-->
  		<!--PORTFOLIO SECTION END-->