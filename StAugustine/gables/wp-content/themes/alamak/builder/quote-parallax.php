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