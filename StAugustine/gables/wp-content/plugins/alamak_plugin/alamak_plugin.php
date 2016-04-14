<?php
/**
 * Plugin Name: Alamak WordPress Theme Plugin Bundle
 * Plugin URI: http://themeforest.net/user/ridianur
 * Description: This is plugin to create custom post type, shortcode, google map for <strong>Alamak WordPress Theme</strong>.
 * Author: ridianur
 * Author URI: http://themeforest.net/user/ridianur
 * Version: 1.2
 */



//CUSTOM POST TYPE SETTING
//include portfolio custom post type
include('inc/portfolio.php');
//include about custom post type
include('inc/about.php');
//include team custom post type
include('inc/team.php');
//include testimonial custom post type
include('inc/testimonial.php');
//include services custom post type
include('inc/services.php');
//include contact custom post type
include('inc/contact.php');
//include pricing table custom post type
include('inc/pricing-table.php');
//SHORTCODE
//include google map shortcode
include('inc/google-map.php');
//include list shortcode shortcode
include('inc/shortcode.php');
//include flickr feed
include('inc/flickr-feed.php');

//adding shortcode into widget
add_filter('widget_text', 'do_shortcode');




