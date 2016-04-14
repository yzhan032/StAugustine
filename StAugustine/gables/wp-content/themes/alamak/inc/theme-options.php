<?php
/**
 * Initialize the custom theme options.
 */
add_action( 'admin_init', 'custom_theme_options' );

/**
 * Build the custom settings & update OptionTree.
 */
function custom_theme_options() {
  /**
   * Get a copy of the saved settings array. 
   */
  $saved_settings = get_option( 'option_tree_settings', array() );
  
  /**
   * Custom settings array that will eventually be 
   * passes to the OptionTree Settings API Class.
   */
  $custom_settings = array( 
    'contextual_help' => array( 
      'sidebar'       => ''
    ),
    'sections'        => array( 
      array(
        'id'          => 'homepage_section',
        'title'       => '<i class="fa fa-home"></i>Homepage  Setting'
      ),
      array(
        'id'          => 'slider_opt',
        'title'       => '<i class="fa fa-picture-o"></i>Slider Option'
      ),
      array(
        'id'          => 'video_section',
        'title'       => '<i class="fa fa-video-camera"></i>VIdeo Option'
      ),
      array(
        'id'          => 'youtube_video_section',
        'title'       => '<i class="fa fa-youtube"></i> Youtube Video Option'
      ),
      array(
        'id'          => 'custom_layout_sec',
        'title'       => '<i class="fa fa-magic"></i> Homepage Custom Layout'
      ),
      array(
        'id'          => 'content_title_section',
        'title'       => '<i class="fa-flag fa"></i>Section Title'
      ),
      array(
        'id'          => 'pricing_section',
        'title'       => '<i class="fa fa-table"></i>Pricing Table Section'
      ),
      array(
        'id'          => 'parallax_section',
        'title'       => '<i class="fa fa-desktop"></i>Parallax Section'
      ),
      array(
        'id'          => 'twitter_section',
        'title'       => '<i class="fa fa-twitter"></i>Twitter Section'
      ),
      array(
        'id'          => 'footer_section',
        'title'       => '<i class="fa fa-sitemap"></i>Footer Section'
      ),
      array(
        'id'          => 'font_section',
        'title'       => '<i class="fa fa-pencil"></i>Fonts Setting'
      ),
      array(
        'id'          => 'style_setting',
        'title'       => '<i class="fa fa-css3"></i>Style Setting'
      ),
      array(
        'id'          => 'portfolio_section_setting',
        'title'       => '<i class="fa fa-cogs"></i> Portfolio Setting'
      )
    ),
    'settings'        => array( 
      array(
        'id'          => 'logo_web',
        'label'       => 'Website Logo',
        'desc'        => 'Upload your logo here.<br>
<i>Recommended size 180x50 </i>',
        'std'         => '',
        'type'        => 'upload',
        'section'     => 'homepage_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'favicon_logo',
        'label'       => 'Website Favicon',
        'desc'        => 'Insert your favicon logo here
<br><i>Recommended size 50x50px</i>',
        'std'         => '',
        'type'        => 'upload',
        'section'     => 'homepage_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'touch_logo',
        'label'       => 'Touch Icon',
        'desc'        => 'Insert your touch logo here
<br><i>Recommended size 130x130px</i>',
        'std'         => '',
        'type'        => 'upload',
        'section'     => 'homepage_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'preloader_set',
        'label'       => 'Loader Display',
        'desc'        => 'Choose how loader will be displayed',
        'std'         => '',
        'type'        => 'select',
        'section'     => 'homepage_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => '',
        'choices'     => array( 
          array(
            'value'       => 'show_home',
            'label'       => 'Only In Homepage',
            'src'         => ''
          ),
          array(
            'value'       => 'show_all',
            'label'       => 'All Page',
            'src'         => ''
          ),
          array(
            'value'       => 'hide_inall',
            'label'       => 'Hide in All Page',
            'src'         => ''
          )
        ),
      ),
      array(
        'id'          => 'loader_color',
        'label'       => 'Loader Background Color',
        'desc'        => 'Choose your background color for preloader',
        'std'         => '',
        'type'        => 'colorpicker',
        'section'     => 'homepage_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'loader_image',
        'label'       => 'Loader Image',
        'desc'        => 'Upload your preloader image here<br>
<i>Recommended size 128x128px </i>',
        'std'         => '',
        'type'        => 'upload',
        'section'     => 'homepage_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'slider_cap_top',
        'label'       => 'Top Caption Text',
        'desc'        => 'Insert your text for slider caption at the top. eg : <b>We are creative agency</b>',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'homepage_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'caption_text_bottom',
        'label'       => 'Bottom Caption Text',
        'desc'        => 'Insert your caption text here. <br>Its only for caption text in video and youtube background template.',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'homepage_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'slider_text_box',
        'label'       => 'Slider Text Box',
        'desc'        => '<br><br>
<h3>You must set the slider in here if you using "Alamak Homepage Slider" template for your homepage.</h3>
<h4 style="margin:0">If you using "Alamak Homepage Video" or "Alamak Homepage Youtube Video" template, you can leave this setting blank.</h4>',
        'std'         => '',
        'type'        => 'textblock',
        'section'     => 'slider_opt',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'slider_setting',
        'label'       => 'Slider Setting',
        'desc'        => 'You can set you slider list for homepage with slider style here.',
        'std'         => '',
        'type'        => 'list-item',
        'section'     => 'slider_opt',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => '',
        'settings'    => array( 
          array(
            'id'          => 'slider_image',
            'label'       => 'Image for slider',
            'desc'        => 'Upload Your Image Here',
            'std'         => '',
            'type'        => 'upload',
            'rows'        => '',
            'post_type'   => '',
            'taxonomy'    => '',
            'min_max_step'=> '',
            'class'       => ''
          )
        )
      ),
      array(
        'id'          => 'video_box',
        'label'       => 'Video Text Box',
        'desc'        => '<br><br>
<h3>You must set the video in here if you using "Alamak Homepage Video" template for your homepage.</h3>
<h4 style="margin:0">If you using "Alamak Homepage Slider" or "Alamak Homepage Youtube Video" template, you can leave this setting blank.</h4>',
        'std'         => '',
        'type'        => 'textblock',
        'section'     => 'video_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'video_path',
        'label'       => 'Video Source',
        'desc'        => 'Insert your video source here (only directlink). <br>eg: <b>http:\yoursite.com\video\video.mp4</b>',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'video_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'video_image',
        'label'       => 'Video Image',
        'desc'        => 'Upload/insert your image to replace the video in mobile/tablet view here.',
        'std'         => '',
        'type'        => 'upload',
        'section'     => 'video_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'youtube_box',
        'label'       => 'Youtube text box',
        'desc'        => '<br><br>
<h3>You must set the youtube video id in here if you using "Alamak Homepage Youtube  Video" template for your homepage.</h3>
<h4 style="margin:0">If you using "Alamak Homepage Slider" or "Alamak Homepage Video" template, you can leave this setting blank.</h4>',
        'std'         => '',
        'type'        => 'textblock',
        'section'     => 'youtube_video_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'youtube_video_bg_link',
        'label'       => 'Youtube Video Link',
        'desc'        => 'Insert Your Youtube Video Id here.<br>
Here the example youtube video http://www.youtube.com/watch?v=2Planmavwic , so you just have to insert <b>2Planmavwic</b>',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'youtube_video_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'youtube_bg_image',
        'label'       => 'Background Image',
        'desc'        => 'Upload or insert your picture here to substitute the youtube video in tablet/mobile view.',
        'std'         => '',
        'type'        => 'upload',
        'section'     => 'youtube_video_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'home_setting_box',
        'label'       => 'box text home setting',
        'desc'        => '<h3>This setting only applies on page with  "Alamak Homepage Custom Layout" template</h3>',
        'std'         => '',
        'type'        => 'textblock',
        'section'     => 'custom_layout_sec',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'custom_layout',
        'label'       => 'Section List',
        'desc'        => '',
        'std'         => '',
        'type'        => 'list-item',
        'section'     => 'custom_layout_sec',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => '',
        'settings'    => array( 
          array(
            'id'          => 'section_type',
            'label'       => 'Select Section',
            'desc'        => 'Select the section that you want to add in homepage.',
            'std'         => '',
            'type'        => 'select',
            'rows'        => '',
            'post_type'   => '',
            'taxonomy'    => '',
            'min_max_step'=> '',
            'class'       => '',
            'choices'     => array( 
              array(
                'value'       => 'about_content',
                'label'       => 'About Section',
                'src'         => ''
              ),
              array(
                'value'       => 'contact_content',
                'label'       => 'Contact Section',
                'src'         => ''
              ),
              array(
                'value'       => 'services_content',
                'label'       => 'Services Section',
                'src'         => ''
              ),
              array(
                'value'       => 'portfolio_content',
                'label'       => 'Portfolio Section',
                'src'         => ''
              ),
              array(
                'value'       => 'quote_content',
                'label'       => 'Parallax Quote Section',
                'src'         => ''
              ),
              array(
                'value'       => 'twitter_content',
                'label'       => 'Twitter Feed Section',
                'src'         => ''
              ),
              array(
                'value'       => 'testimonial_content',
                'label'       => 'Testimonial Section',
                'src'         => ''
              )
            ),
          )
        )
      ),
      array(
        'id'          => 'home_setting',
        'label'       => 'Home Section Background Setting',
        'desc'        => '<h3>Select Background type for home section here</h3>
You can set the slider/video/youtube background at the <strong>Slider/Video/Youtube Option</strong>',
        'std'         => '',
        'type'        => 'select',
        'section'     => 'custom_layout_sec',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => '',
        'choices'     => array( 
          array(
            'value'       => 'slider_bg_home',
            'label'       => 'Slider Background',
            'src'         => ''
          ),
          array(
            'value'       => 'video_bg_home',
            'label'       => 'Video Background',
            'src'         => ''
          ),
          array(
            'value'       => 'youtube_bg_home',
            'label'       => 'Youtube Video Background',
            'src'         => ''
          )
        ),
      ),
      array(
        'id'          => 'portfolio_title',
        'label'       => 'Portfolio Big Title',
        'desc'        => 'Input portfolio big title here.',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'content_title_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'portfolio_small',
        'label'       => 'Portfolio Small Title',
        'desc'        => 'Input portfolio small title here.',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'content_title_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'about_title',
        'label'       => 'About Big Title',
        'desc'        => 'Input big title for about section here',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'content_title_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'about_small',
        'label'       => 'About Small Title',
        'desc'        => 'Input about small title here.',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'content_title_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'services_title',
        'label'       => 'Services Big Title',
        'desc'        => 'Input services big title here.',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'content_title_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'services_small',
        'label'       => 'Services Small Title',
        'desc'        => 'Input services small title here.',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'content_title_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'contact_title',
        'label'       => 'Contact Big Title',
        'desc'        => 'Input contact big title here.',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'content_title_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'contact_small',
        'label'       => 'Contact Small Title',
        'desc'        => 'Input contact small title here.',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'content_title_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'pricing_display',
        'label'       => 'Display Pricing Table',
        'desc'        => 'Select yes if you want to show the pricing tables. Or select no if you dont want to show it.',
        'std'         => '',
        'type'        => 'select',
        'section'     => 'pricing_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => '',
        'choices'     => array( 
          array(
            'value'       => 'yes',
            'label'       => 'Yes',
            'src'         => ''
          ),
          array(
            'value'       => 'no',
            'label'       => 'No',
            'src'         => ''
          )
        ),
      ),
      array(
        'id'          => 'pricing_col',
        'label'       => 'Pricing Table Columns',
        'desc'        => 'Choose how many columns the pricing table will be displayed',
        'std'         => '',
        'type'        => 'select',
        'section'     => 'pricing_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => '',
        'choices'     => array( 
          array(
            'value'       => '2',
            'label'       => '2',
            'src'         => ''
          ),
          array(
            'value'       => '3',
            'label'       => '3',
            'src'         => ''
          ),
          array(
            'value'       => '4',
            'label'       => '4',
            'src'         => ''
          )
        ),
      ),
      array(
        'id'          => 'para_quote_text',
        'label'       => 'First Parallax (quote) Text',
        'desc'        => 'Insert your quote text here.',
        'std'         => '',
        'type'        => 'textarea-simple',
        'section'     => 'parallax_section',
        'rows'        => '3',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'para_quote_author',
        'label'       => 'First Parallax(quote) Author',
        'desc'        => 'Insert your author quote here.',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'parallax_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'parallax_quote',
        'label'       => 'First Parallax (quote) background image',
        'desc'        => 'Upload your image for parallax background image here.',
        'std'         => '',
        'type'        => 'upload',
        'section'     => 'parallax_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'parallax_testi',
        'label'       => 'Second Parallax (testimonial) background image',
        'desc'        => 'Upload your image for parallax background image here.',
        'std'         => '',
        'type'        => 'upload',
        'section'     => 'parallax_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'parallax_tweet',
        'label'       => 'Third Parallax (twitter feed) background image',
        'desc'        => 'Upload your image for parallax background image here.',
        'std'         => '',
        'type'        => 'upload',
        'section'     => 'parallax_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'twit_shortcode',
        'label'       => 'Insert Twitter Shortcode Here',
        'desc'        => 'Insert your twitter shortcode here',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'twitter_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'twitter_usage',
        'label'       => 'Usage:',
        'desc'        => 'After you finish set up the twitter setting in <strong>Settings-&gt;Rotating Tweets</strong> place the shortcode here.
<br>
Here is the sample format:
<br>
<strong> [rotatingtweets screen_name=\'envato\' show_meta_screen_name = \'0\' show_meta_via = \'0\' no_rotate = \'1\']</strong>
<br>
for more information about it you can view in:<br>
<strong>http://wordpress.org/plugins/rotatingtweets/installation/</strong>
<br><br>
**Remember to add
<strong>no_rotate = \'1\'</strong> inside the shortcode for the best display position.',
        'std'         => '',
        'type'        => 'textblock',
        'section'     => 'twitter_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'twitter_feed_link',
        'label'       => 'Insert Your Twitter Link Here',
        'desc'        => 'You can insert your twitter link in here.',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'twitter_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'twitter_feed_text',
        'label'       => 'Insert Your Twitter Text Here',
        'desc'        => 'You can insert your twitter text for twitter link here. eg:Follow Us',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'twitter_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'footer_text',
        'label'       => 'Footer Text',
        'desc'        => 'Input your text for footer here.<br> eg: <b>Copyright 2013 Your Website</b>',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'footer_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'social_facebook',
        'label'       => 'Facebook Link',
        'desc'        => 'Input Your Facebook Link Here',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'footer_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'social_google_plus',
        'label'       => 'Google Plus Link',
        'desc'        => 'Input Your Google Plus Link here',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'footer_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'social_twitter',
        'label'       => 'Twitter Link',
        'desc'        => 'Input Your Twitter Link here',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'footer_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'social_dribble',
        'label'       => 'Dribbble Link',
        'desc'        => 'Input Your Dribble Link here',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'footer_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'social_pinterest',
        'label'       => 'Pinterest Link',
        'desc'        => 'Input Your Pinterest Link here',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'footer_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'social_youtube',
        'label'       => 'Youtube Link',
        'desc'        => 'Input Your Youtube Link here',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'footer_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'another_social_link',
        'label'       => 'Another Social Icon and Link',
        'desc'        => 'You can create your own social icon and link here. For Icon list you can check this page <b>http://fortawesome.github.io/Font-Awesome/icons/</b>',
        'std'         => '',
        'type'        => 'list-item',
        'section'     => 'footer_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => '',
        'settings'    => array( 
          array(
            'id'          => 'own_link',
            'label'       => 'Your Social Icon Link',
            'desc'        => 'Input Your Social Icon Link here',
            'std'         => '',
            'type'        => 'text',
            'rows'        => '',
            'post_type'   => '',
            'taxonomy'    => '',
            'min_max_step'=> '',
            'class'       => ''
          ),
          array(
            'id'          => 'own_icon',
            'label'       => 'Your Social Icon',
            'desc'        => 'Input your Social Icon here. eg :<b>fa-vk</b>',
            'std'         => '',
            'type'        => 'text',
            'rows'        => '',
            'post_type'   => '',
            'taxonomy'    => '',
            'min_max_step'=> '',
            'class'       => ''
          )
        )
      ),
      array(
        'id'          => 'style_box',
        'label'       => 'style box',
        'desc'        => '<h3>You can use Google Web Fonts in here. </h3>
<br>You can search and use your google fonts in here <b>http://www.google.com/fonts/</b><br>
Input your font link from google fonts in <b>Heading/Paragraph Font link</b>. eg: 
&lt; <strong>link href=\'http://fonts.googleapis.com/css?family=Coming+Soon\' rel=\'stylesheet\' type=\'text/css\'</strong>&gt;
 <br>
Input your font family from google fonts in <b>Heading/Paragraph Font Family</b>. eg:
<b>font-family: \'Coming Soon\', cursive;</b>',
        'std'         => '',
        'type'        => 'textblock',
        'section'     => 'font_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'heading_font_link',
        'label'       => 'Heading Font Link',
        'desc'        => 'Input Your Heading Font here',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'font_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'heading_font_family',
        'label'       => 'Heading Font Family',
        'desc'        => 'Input Your Heading Font Family',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'font_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'paragraph_font_link',
        'label'       => 'Paragraph Font Link',
        'desc'        => 'Input Your Paragraph Font Link here.',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'font_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'paragraph_font_family',
        'label'       => 'Paragraph Font Family',
        'desc'        => 'Input Your Paragraph Font Family here',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'font_section',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'general_color_bg',
        'label'       => 'General Paragraph Color',
        'desc'        => 'Choose your general color for paragraph.',
        'std'         => '',
        'type'        => 'colorpicker',
        'section'     => 'style_setting',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'header_color_bg',
        'label'       => 'Header Background Color',
        'desc'        => 'Choose your background color for Header.',
        'std'         => '',
        'type'        => 'colorpicker',
        'section'     => 'style_setting',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'portfolio_color_bg',
        'label'       => 'Portfolio Section Background Color',
        'desc'        => 'Choose your background color for Portfolio section.',
        'std'         => '',
        'type'        => 'colorpicker',
        'section'     => 'style_setting',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'about_color_bg',
        'label'       => 'About Section Background Color',
        'desc'        => 'Choose your background color for About section.',
        'std'         => '',
        'type'        => 'colorpicker',
        'section'     => 'style_setting',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'services_color_bg',
        'label'       => 'Services Section Background Color',
        'desc'        => 'Choose your background color for Services section.',
        'std'         => '',
        'type'        => 'colorpicker',
        'section'     => 'style_setting',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'contact_color_bg',
        'label'       => 'Contact Section Background Color',
        'desc'        => 'Choose your background color for Contact section.',
        'std'         => '',
        'type'        => 'colorpicker',
        'section'     => 'style_setting',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'footer_color_bg',
        'label'       => 'Footer Background Color',
        'desc'        => 'Choose your background color for Footer.',
        'std'         => '',
        'type'        => 'colorpicker',
        'section'     => 'style_setting',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'content_color_bg',
        'label'       => 'Content Background Color',
        'desc'        => 'Choose your background color for the content.The default color is white.',
        'std'         => '',
        'type'        => 'colorpicker',
        'section'     => 'style_setting',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'css_box',
        'label'       => 'Custom Css Box',
        'desc'        => 'You can insert your custom css for styling here.',
        'std'         => '',
        'type'        => 'css',
        'section'     => 'style_setting',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',

        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'portfolio_home',
        'label'       => 'How many Portfolio item display in Homepage?',
        'desc'        => 'You can choose how many item that will display in homepage. The default value is 6 items.',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'portfolio_section_setting',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'portfolio_single',
        'label'       => 'How many Portfolio item display in Portfolio Page?',
        'desc'        => 'You can choose how many item that will display in portfolio page here. The default value is 6 items.',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'portfolio_section_setting',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'portfolio_list',
        'label'       => 'Choose your Portfolio Page',
        'desc'        => 'Select your portfolio page that using <strong>"Portfolio List"</strong> template.',
        'std'         => '',
        'type'        => 'page-select',
        'section'     => 'portfolio_section_setting',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'portfolio_list_title',
        'label'       => 'Portfolio Page Link Text',
        'desc'        => 'Insert your text for button link to the portfolio page. eg : View All Works',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'portfolio_section_setting',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'pp_big_title',
        'label'       => 'Porfolio Page Big Title',
        'desc'        => 'Insert your big title here',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'portfolio_section_setting',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      ),
      array(
        'id'          => 'pp_small_title',
        'label'       => 'Portfolio Page Small Title',
        'desc'        => 'Insert your small title here',
        'std'         => '',
        'type'        => 'text',
        'section'     => 'portfolio_section_setting',
        'rows'        => '',
        'post_type'   => '',
        'taxonomy'    => '',
        'min_max_step'=> '',
        'class'       => ''
      )
    )
  );
  
  /* allow settings to be filtered before saving */
  $custom_settings = apply_filters( 'option_tree_settings_args', $custom_settings );
  
  /* settings are not the same update the DB */
  if ( $saved_settings !== $custom_settings ) {
    update_option( 'option_tree_settings', $custom_settings ); 
  }
  
}