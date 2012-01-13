var isCreating = true;

$().ready(function() {
	var commentEditor;
	var quotePlaceHolder;

	function ctor() {
		CKEDITOR.replace('CommentTextBox', { language: Resource.Language, toolbar: 'Short' });
		quotePlaceHolder = $("<div id='quoteplaceholder'></div>");
		commentEditor = CKEDITOR.instances.CommentTextBox;
		commentEditor.on('dataReady', function(e) {
			$("#cke_CommentTextBox").find("iframe").before(quotePlaceHolder);
		});
		$("body").click(comments_Click);
	}
	/*$('#CommentTextBox').tinymce({
	// Location of TinyMCE script
	script_url: '/Scripts/tinymce/tiny_mce.js',

	// General options
	theme: "advanced",
	plugins: "style,advlink,emotions,fullscreen,uploader",
	width: "100%",

	// Theme options
	theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,fontselect,fontsizeselect,styleprops,|,link,unlink,|,image,emotions,|,code,fullscreen",
	theme_advanced_buttons2: "",
	theme_advanced_buttons3: "",
	theme_advanced_buttons4: "",
	theme_advanced_toolbar_location: "top",
	theme_advanced_toolbar_align: "left",
	theme_advanced_statusbar_location: "bottom",
	theme_advanced_resizing: true,
		
	// Drop lists for link/image/media/template dialogs
	/*
	template_external_list_url : "lists/template_list.js",
	external_link_list_url : "lists/link_list.js",
	external_image_list_url : "lists/image_list.js",
	media_external_list_url : "lists/media_list.js",
	*/
	/*

	// Replace values for the template plugin
	template_replace_values: {
	username: "Some User",
	staffid: "991234"
	}
	});*/

	$('#commentsForm > input[name=mode]').val('ajax');

	$('#commentsForm').submit(function(e) {
		return false;
	}).submit(function(e) {
		var url = $(e.target).attr('action');
		var text = commentEditor.getData();
		/*if ($.trim($("<div>" + text + "</div>").text()) == "") {

			return;
		}*/
		var quote = $("<div></div>").append(quotePlaceHolder.html());
		quote.find(".quote").append("<div class='footer'></div>");

		text = quote.html() + text;
		var tmpl = $("#template");
		tmpl.find("[link=text]").html(text);

		$.ajax({
			data: "text=" + text + "&mode=ajax",
			type: "POST",
			url: url,
			success: postSuccess,
			error: postError
		});
		return false;
	});

	function quoteComment(commentContainer) {
		var name = commentContainer.find(".name").html();
		var html = commentContainer.find("[link=text]").html();
		var d = $("<div class='quote'></div>");
		d.html(html);

		var quote = d.find(".quote");
		if (quote.length == 0) {
			d.html("<blockquote><span class='quotename'>" + name + Resource.Writen + "</span>" + html + "</blockquote>");
			quotePlaceHolder.empty().append(d);
			return;
		}
		quote.remove();
		// only text without quote
		var text = d.html();
		d.empty().append(quote.html());
		d.find(".footer").remove();
		d.find("blockquote:last").append("<blockquote><span class='quotename'>" + name + Resource.Writen + "</span>" + text + "</blockquote>");

		quotePlaceHolder.empty().append(d);
	}

	function comments_Click(e) {
		var target = $(e.target);
		if (target.is('a') && target.parent().is('.quotebutton')) {
			var commentContainer = target.parents(".comment");
			quoteComment(commentContainer);
		}
	}

	function postSuccess(data, textStatus) {
		eval("data=" + data + ";");
		var tmpl = $($("#template").html());
		$("#comments").append(tmpl);
		tmpl.find(".date").text(data.date);
		commentEditor.setData('');
	}

	ctor();
});



function deletePostSuccess() {
	$("#comments").get(0).removeChild($("[link=delete]").get(0));
}

function postError(data, textStatus, errorThrown) {
	window.alert('error');
}


function BindCommentEvents() {
	$(".btnDelete").bind("click", function(event) {
		cancelHandler();
		var editor = tinyMCE.get('CommentTextBox');
		var base = $(event.target).parents(".comment").get(0);
		$(base).attr("link", "delete");
		if (window.confirm(Resource.DeleteCommentQuery)) {
			$.ajax({
				data:{comment: $("[link=id]", base).text()},
				type: "POST",
				url: ActionHelper.DeleteComment,
				success: deletePostSuccess,
				error: postError
			});
		}
	});
}