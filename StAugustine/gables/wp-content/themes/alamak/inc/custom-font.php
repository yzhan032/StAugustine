<?php
	function rdn_fonts_custom() {
		if  ( function_exists( 'ot_get_option' )){
			$heading_css = ot_get_option( 'heading_font_family' ); 
			$heading_font = "
			h1, h2, h3, h4, h5, h6 { $heading_css }
			";
			$paragraph_css = ot_get_option( 'paragraph_font_family' ); 
			$paragraph_font = "
			body { $paragraph_css }
			";
			if ( function_exists( 'ot_get_option' ) && ot_get_option( 'heading_font_family' )) {             
			wp_add_inline_style( 'custom-style', $heading_font );
			}
			if ( function_exists( 'ot_get_option' ) && ot_get_option( 'paragraph_font_family' )) {             
			wp_add_inline_style( 'custom-style', $paragraph_font );
			}
		}
	}	
		
