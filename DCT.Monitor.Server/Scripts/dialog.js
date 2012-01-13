tinyMCEPopup.requireLangPack("uploader_dlg");

var UploaderDialog = {
	selectedPicture: false,
	uploadedPicture: false,
	uploadingUrl: '',
	removingUrl: '',
	listingUrl: '',
	plagin_url: '',
	no_view: '',
	chose_image: '',
	index: tabpos, // defined in Files.aspx    

	init: function(ed) {
		UploaderDialog.uploadingUrl = ed.getParam('uploader_uploadingurl');
		UploaderDialog.listingUrl = ed.getParam('uploader_listingurl');
		UploaderDialog.removingUrl = ed.getParam('uploader_removingurl');
		UploaderDialog.plagin_url = tinyMCEPopup.getWindowArg('plagin_url');
		UploaderDialog.no_view = ed.getLang('uploader_dlg.no_view');
		UploaderDialog.chose_image = ed.getLang('uploader_dlg.chose_image');

		$("#fileUpload > form").attr("action", UploaderDialog.uploadingUrl);
		$("#removeFileForm").attr("action", UploaderDialog.removingUrl);

		$("div[url]").bind("click", function(e) {
			if (UploaderDialog.selectedPicture) {
				UploaderDialog.selectedPicture.removeClass('selected');
			}

			if ($(e.target).attr('url') != undefined) UploaderDialog.selectedPicture = $(e.target);
			else UploaderDialog.selectedPicture = $(e.target).parents("div[url]");

			UploaderDialog.selectedPicture.addClass('selected');
			UploaderDialog.selectFile();
			$("form > input[type=hidden]").val($("[link=name]", UploaderDialog.selectedPicture).text());
		});
		$("#fileTabs").tabs({
			select: function(event, ui) {
				UploaderDialog.index = ui.index;
				if (ui.index == 0) UploaderDialog.selectFile();
				else if (ui.index == 1) UploaderDialog.newFile();
				else UploaderDialog.otherFile();
				return true;
			},
			selected: UploaderDialog.index
		});

		new AjaxUpload("#browse", {
			action: UploaderDialog.uploadingUrl,
			data: { mode: 'temp' },
			onComplete: function(file, response) {
				UploaderDialog.uploadedPicture = response;
				UploaderDialog.newFile();
			}
		});

		$("#fileOther > [type=text]").change(function(e) {
			UploaderDialog.otherFile();
		});
	},

	selectFile: function() {
		if (!UploaderDialog.selectedPicture) return;
		$("#display").attr("src", UploaderDialog.selectedPicture.attr('url'));
	},

	newFile: function() {
		$("#display").attr("src", UploaderDialog.uploadedPicture);
		$("#display").attr("alt", UploaderDialog.no_view);
	},

	otherFile: function() {
		var target = $("#fileOther > [type=text]").get(0);
		$("#display").attr("src", $(target).val());
		$("#display").attr("alt", UploaderDialog.no_view);
	},

	insert: function() {
		var src = $("#display").attr('src');
		var markup = 'img';
		$("[link=properties] > [property]").each(function() {
			var v = $(this).val();
			if (v != "") markup += " " + $(this).attr('property') + '="' + v + '"';
		});
		var style = "";
		$("[link=style] > [property]").each(function() {
			var v = $(this).val();
			if (v != "") style += " " + $(this).attr('property') + ':' + v + ';';
		});
		if (style != "") markup += ' style="' + style + '"';
		if (UploaderDialog.index == 1) {
			src = $.ajax({
				url: UploaderDialog.uploadingUrl,
				type: 'post',
				async: false
			}).responseText;
			alert(src);
			if (src == "error") {
				alert(src);
				return;
			}
		}
		if ((UploaderDialog.index == 0 && !UploaderDialog.selectedPicture)
			|| (UploaderDialog.index == 1 && !UploaderDialog.uploadedPicture)
			|| (UploaderDialog.index == 2 && $("#fileOther > [type=text]").get(0).val() == "")) {
			window.alert(UploaderDialog.chose_image);
			return;
		}

		markup = '<' + markup + ' src="' + src + '" />';
		tinyMCE.execCommand('mceInsertContent', false, markup);
		tinyMCEPopup.close();
	}
};

tinyMCEPopup.onInit.add(UploaderDialog.init, UploaderDialog);
