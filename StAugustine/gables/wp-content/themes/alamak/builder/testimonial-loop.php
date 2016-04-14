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