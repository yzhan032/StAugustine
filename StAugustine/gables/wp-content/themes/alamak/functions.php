<?php

	add_action( 'after_setup_theme', 'alamak_theme_setup' );

		function alamak_theme_setup() {
			/* Add filters, actions, and theme-supported features. */
			//THEME SUPORT FUNCTION
			//add thumbnail
			add_theme_support( 'post-thumbnails' );
			//custom background
			add_theme_support( 'custom-background' );
			//post format
			add_theme_support( 'post-formats', array( 'aside', 'gallery' ,'quote', 'video', 'audio' , 'link') );
			//automatic feed
			add_theme_support( 'automatic-feed-links' );
			//add menu homepage and blog
			add_action( 'init', 'register_rdn_menu' );
			add_action( 'init', 'register_rdn_menu_blog' );
			
			//width content
			if ( ! isset( $content_width ) )$content_width = 1170;
			
			//THEME SCRIPT AND STYLES
			//theme default script and styles
			add_action('wp_enqueue_scripts', 'rdn_theme_scripts');
			add_action('wp_enqueue_scripts', 'rdn_theme_styles');
			add_action('wp_enqueue_scripts', 'rdn_parallax_quote');
			//color scheme option styles
			add_action( 'wp_enqueue_scripts', 'rdn_color_scheme' );
			//preloader script and styles
			add_action( 'wp_enqueue_scripts', 'rdn_preloader_set' );
			add_action('wp_enqueue_scripts', 'rdn_preloader');
			//Homepage setting(slider,video,youtube background)
			add_action('wp_enqueue_scripts', 'rdn_supersized_style');
			add_action('wp_enqueue_scripts', 'rdn_youtube_bg_image');
			add_action('wp_enqueue_scripts', 'rdn_homepage_script');
			add_action( 'wp_footer', 'rdn_homepage_setting' , 99);
			//custom font styles
			add_action( 'wp_enqueue_scripts', 'rdn_fonts_custom' );  
			//script for about slider and portfolio ajax
			add_action( 'wp_footer', 'rdn_single_portfolio' , 100);
			add_action( 'wp_footer', 'rdn_single_team' , 101);
			// remove some post type from rss feed
			add_action( 'pre_get_posts', 'exclude_post_formats_from_feeds' );
			
			
			//CUSTOM FILTER
			//custom search setting
			add_filter( 'get_search_form', 'html5_search_form' );
			//custom excerpt
			add_filter( 'excerpt_length', 'rdn_excerpt_length', 10 );
			//remove [..] in excerpt
			add_filter('get_the_excerpt', 'trim_excerpt');
			//custom comment styles
			add_filter('comment_form_default_fields','wpsites_modify_comment_form_fields');
			//custom prev&next class
			add_filter('next_posts_link_attributes', 'posts_link_attributes');
			add_filter('previous_posts_link_attributes', 'posts_link_attributes');
			add_filter('next_post_link', 'post_link_attributes');
			add_filter('previous_post_link', 'post_link_attributes');
			
			//thumbnail setting
			add_image_size( 'portfolio-image', 600, 600, true );
			add_image_size( 'team-image', 768, 768, true );
			add_image_size( 'testi-image', 300, 300, true );
	}
	
	
	//add menu for homepage
	function register_rdn_menu() {
		register_nav_menu('header-menu',__( 'Main Menu in Homepage' ));
	}

	
	
	//add menu for blog
	function register_rdn_menu_blog() {
		register_nav_menu('header-menu-blog',__( 'Menu in Blog and Pages' ));
	}

	
	//adding sidebar widget
	if ( function_exists('register_sidebar') ) {
		register_sidebar(array(
		'name' => 'Default Sidebar',
		'id' => 'default-sidebar','description' => 'Appears as the sidebar on blog and pages',
		'before_widget' => '<div  id="%1$s" class="widget %2$s clearfix">','after_widget' => '</div>',
		'before_title' => '<h3 class="widgettitle">',
		'after_title' => '</h3>',));
	}

	
	//THEME SCRIPTS & STYLES
	// include theme-script
	include('inc/theme-script.php');
	// include theme-styles
	include('inc/theme-style.php');
	//preloader setting
	include('inc/preloader.php');
	//color scheme styles
	include('inc/color-scheme.php');
	//single portfolio setting
	include('inc/single-portfolio.php');
	//single team setting
	include('inc/single-team.php');
	//custom font styles
	include('inc/custom-font.php');
	//homepage setting
	include('inc/homepage-setting.php');
	//parallax setting
	include('inc/parallax.php');
	//pagination
	include('inc/pagination.php');
	//included theme import
	include('inc/theme-import.php');
	//include TGM activation
	include('inc/plugin-install.php');
	
	/* Replacing the default WordPress search form with an HTML5 version */
	function html5_search_form( $form ) {
		$form = '<form role="search" method="get" id="searchform" action="' . home_url( '/' ) . '" > 
		<input type="search" placeholder="'.__("Search and hit enter...").'" value="' . get_search_query() . '" name="s" id="s" />
		<input type="submit" id="searchsubmit" />
		</form>';
		return $form;
	}
	
	//add class into prev&next button(pages/posts)
	function posts_link_attributes() {
		return 'class="view-more"';
	}
	//add class into prev&next button(single post)
	function post_link_attributes($output) {
		$code = 'class="view-more"';
		return str_replace('<a href=', '<a '.$code.' href=', $output);
	}
	
	//custom excerpt function
	function rdn_excerpt_length( $length ) {
		return 40;
	}
	// Remove [...]
	function trim_excerpt($text) {
		$text = str_replace('[', '', $text);
		$text = str_replace(']', '', $text);
		return $text;
	}

	
	//comment include
	include('inc/comment-template.php');
	//metabox include
	include('inc/custom-meta-boxes.php');
	//remove read more link
	function remove_more_jump_link($link) {
		$offset = strpos($link, '#more-');
		
		if ($offset) {
			$end = strpos($link, '"',$offset);
		}

		
		if ($end) {
			$link = substr_replace($link, '', $offset, $end-$offset);
		}

		return $link;
	}

	//custom comment form
	function wpsites_modify_comment_form_fields($fields){
		$req = get_option('require_name_email');
		$commenter = wp_get_current_commenter();
		$aria_req = ( $req ? " aria-required='true'" : '' ); 
		
		$fields['author'] = '<p class="comment-form-author">' . '<label for="author">' . __( 'Name', 'wpsites.net' ) . '</label> ' . 
		
		( $req ? '' : '' ) .
						
		'<input id="author" name="author" type="text" placeholder="Your Name ..." value="' . 
						
		esc_attr( $commenter['comment_author'] ) . '" size="30"' . $aria_req . ' /></p>';
						
		$fields['email'] = '<p class="comment-form-email"><label for="email">' . __( 'Email', 'wpsites.net' ) . '</label> ' .
		
		( $req ? '' : '' ) .
		
		'<input id="email" name="email" type="text" placeholder="Your Email ..." value="' . 
		
		esc_attr(  $commenter['comment_author_email'] ) . '" size="30"' . $aria_req . ' /></p>';
		
		$fields['url'] = '<p class="comment-form-url"><label for="url">' . __( 'Website', 'wpsites.net' ) . '</label>' .
		
		'<input id="url" name="url" type="text" placeholder="Your Website ..." value="' . 
		
		esc_attr( $commenter['comment_author_url'] ) . '" size="30" /></p>';
		
		return $fields;
	}

	// Hide post formats from WordPress generated RSS feeds:
	function exclude_post_formats_from_feeds( &$wp_query ) {
		// Only do this for feed queries:
		
		if ( $wp_query->is_feed() ) {
			// Array of post formats to exclude, by slug,
			// e.g. "post-format-{format}"
			$post_formats_to_exclude = array('post-format-status','post-format-quote','post-format-link','post-format-aside');
			// Extra query to hack onto the $wp_query object:
			$extra_tax_query = array('taxonomy' => 'post_format','field' => 'slug','terms' => $post_formats_to_exclude,'operator' => 'NOT IN');
			$tax_query = $wp_query->get( 'tax_query' );
			
			if ( is_array( $tax_query ) ) {
				$tax_query = $tax_query + $extra_tax_query;
			} else {
				$tax_query = array( $extra_tax_query );
			}

			$wp_query->set( 'tax_query', $tax_query );
		}

	}




//adding optiontree into themes
/**
 * Optional: set 'ot_show_pages' filter to false.
 * This will hide the settings & documentation pages.
 */add_filter( 'ot_show_pages', '__return_false' );
	/**
 * Optional: set 'ot_show_new_layout' filter to false.
 * This will hide the "New Layout" section on the Theme Options page.
 */add_filter( 'ot_show_new_layout', '__return_false' );
	/**
 * Required: set 'ot_theme_mode' filter to true.
 */add_filter( 'ot_theme_mode', '__return_true' );
	/**
 * Required: include OptionTree.
 */load_template( trailingslashit( get_template_directory() ) . 'option-tree/ot-loader.php' );
	/**
 * Theme Options
 */load_template( trailingslashit( get_template_directory() ) . 'inc/theme-options.php' );