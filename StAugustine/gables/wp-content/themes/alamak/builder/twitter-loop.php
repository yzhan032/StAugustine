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
        <!--TWITTER SECTION END--