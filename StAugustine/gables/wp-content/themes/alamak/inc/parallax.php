<?php 
//parallax (quote) background setting
function rdn_parallax_quote() {
	if  ( function_exists( 'ot_get_option' )){
        $quote_img =  ot_get_option( 'parallax_quote' );
        $quote_bg = "
		.bg1{background-image: url('$quote_img');}
		"; 
		if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'parallax_quote' )) {           
        wp_add_inline_style( 'custom-style', $quote_bg );
		} 
		$testi_img =  ot_get_option( 'parallax_testi' );
        $testi_bg = "
		.bg2{background-image: url('$testi_img');}
		"; 
		if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'parallax_testi' )) {           
        wp_add_inline_style( 'custom-style', $testi_bg );
		}
		$tweet_img =  ot_get_option( 'parallax_tweet' );
        $tweet_bg = "
		.twitter-feed{background-image: url('$tweet_img');}
		"; 
		if  ( function_exists( 'ot_get_option' ) && ot_get_option( 'parallax_tweet' )) {           
        wp_add_inline_style( 'custom-style', $tweet_bg );
		}
	}
		 
}
