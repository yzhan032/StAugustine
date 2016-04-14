<ul class="commentlist clearfix"><?php wp_list_comments('avatar_size=70&callback=alamak_comments'); ?></ul>
<div class="pagination-comment clearfix"><?php paginate_comments_links(); ?> </div>
<?php comment_form(array('title_reply'=>'Leave your comment here', 'comment_notes_before'=>'', 'comment_notes_after'=>'')); ?> 

