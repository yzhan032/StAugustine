<?php 
//color scheme
function rdn_color_scheme() {
	if  ( function_exists( 'ot_get_option' )){		
		//header background color
		$header =  ot_get_option( 'header_color_bg' );
        $header_css = "
		.is-sticky .header{
			background-color:$header;
		}
		";   
		if ( function_exists( 'ot_get_option' ) && ot_get_option( 'header_color_bg' )) {           
        wp_add_inline_style( 'custom-style', $header_css );
		}
		//portfolio background color
		$portfolio =  ot_get_option( 'portfolio_color_bg' );
        $portfolio_css = "
		#portfolio,#portfolio .title-content .title-inner h3,#portfolio .title-content .title-inner p{
			background-color:$portfolio;
		}
		";   
		if ( function_exists( 'ot_get_option' ) && ot_get_option( 'portfolio_color_bg' )) {           
        wp_add_inline_style( 'custom-style', $portfolio_css );
		}
		//about background color
		$about =  ot_get_option( 'about_color_bg' );
        $about_css = "
		#about,#about .title-content .title-inner h3,#about .title-content .title-inner p{
			background-color:$about;
		}
		";   
		if ( function_exists( 'ot_get_option' ) && ot_get_option( 'about_color_bg' )) {           
        wp_add_inline_style( 'custom-style', $about_css );
		}
		//services background color
		$services =  ot_get_option( 'services_color_bg' );
        $services_css = "
		#services,#services .title-content .title-inner h3,#services .title-content .title-inner p{
			background-color:$services;
		}
		";   
		if ( function_exists( 'ot_get_option' ) && ot_get_option( 'services_color_bg' )) {           
        wp_add_inline_style( 'custom-style', $services_css );
		}
		//contact background color
		$contact =  ot_get_option( 'contact_color_bg' );
        $contact_css = "
		#contact,#contact .title-content .title-inner h3,#contact .title-content .title-inner p{
			background-color:$contact;
		}
		";   
		if ( function_exists( 'ot_get_option' ) && ot_get_option( 'contact_color_bg' )) {           
        wp_add_inline_style( 'custom-style', $contact_css );
		}
		//footer background color
		$footer =  ot_get_option( 'footer_color_bg' );
        $footer_css = "
		.footer{
			background-color:$footer;
		}
		";   
		if ( function_exists( 'ot_get_option' ) && ot_get_option( 'footer_color_bg' )) {           
        wp_add_inline_style( 'custom-style', $footer_css );
		}
		//content background color
		$content =  ot_get_option( 'content_color_bg' );
        $content_css = "
		.content .white-bg,.box-60{
			background-color:$content;
		}
		";   
		if ( function_exists( 'ot_get_option' ) && ot_get_option( 'content_color_bg' )) {           
        wp_add_inline_style( 'custom-style', $content_css );
		}
		//general  color
		$general =  ot_get_option( 'general_color_bg' );
        $general_css = "
		body{
			color:$general;
		}
		";   
		if ( function_exists( 'ot_get_option' ) && ot_get_option( 'general_color_bg' )) {           
        wp_add_inline_style( 'custom-style', $general_css );
		}
		
		
	}
}