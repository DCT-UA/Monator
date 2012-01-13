/*
Stylish Select 0.3 - $ plugin to replace a select drop down box with a stylable unordered list
http://scottdarby.com/

Copyright (c) 2009 Scott Darby

Requires: $ 1.3

Licensed under the GPL license:
http://www.gnu.org/licenses/gpl.html
*/
(function($) {
	//add class of js to html tag
	//create cross-browser indexOf
	Array.prototype.indexOf = function(obj, start) {
		for (var i = (start || 0); i < this.length; i++) {
			if (this[i] == obj) {
				return i;
			}
		}
	}

	//utility methods
	$.fn.extend({
		getSetSSValue: function(value) {
			if (value) {
				//set value and trigger change event
				$(this).val(value).change();
				return this;
			} else {
				return selText = $(this).find(':selected').text();
			}
		},
		resetSS: function() {
			$this = $(this);
			$this.next().remove();
			//unbind all events and redraw
			$this.unbind().sSelect();
		}
	});

	$.fn.multiSelect = function(options) {
		return this.each(function() {
			var defaults = {
				defaultText: 'Please select',
				SelectAll: false,
				animationSpeed: 0, //set speed of dropdown
				ddMaxHeight: '' //set css max-height value of dropdown
			};

			//initial variables
			var opts = $.extend(defaults, options),
				$input = $(this),
				$containerDivText = $('<div class="selectedTxt"></div>'),
				$containerDiv = $('<div class="newMultiListSelected" tabindex="0"></div>'),
				$containerDivOptions = $('<div class="newMultiOptions"></div>'),
				$containerDivBorder = $('<div class="border"></div>'),
				$containerScroll = $('<div class="content-slider"></div>'),
				$containerContent = $('<div class="content-scroll"></div>'),
				$newUl = $('<ul class="newList"></ul>'),
				newListItems = '',
				currentIndex = -1,
				$selectAllUl = $("<ul><li></li></ul>");
			var $selectAllLi = $selectAllUl.children("li");
			//build new list
			$containerDiv.insertAfter($input);
			$containerDivText.prependTo($containerDiv);
			$containerScroll.appendTo($containerDivBorder);
			if (!!opts.SelectAll) {
				$selectAllLi.text(opts.SelectAll);
				$selectAllUl.appendTo($containerContent);
			}
			$newUl.appendTo($containerContent);
			$containerContent.appendTo($containerDivBorder);
			$containerDivBorder.appendTo($containerDivOptions);
			$containerDivOptions.appendTo($containerDiv);

			$input.hide();

			//test for optgroup

			var newListItems = '';
			$input.children().each(function() {
				var option = $(this).text();
				//alert(option);
				//alert($(this).attr('selected'))

				if ($(this).attr('selected') == true) {
					newListItems += '<li class="hiLite">' + option + '</li>';
				}
				else {
					newListItems += '<li>' + option + '</li>';
				}
			});
			//add new list items to ul
			$newUl.html(newListItems);
			newListItems = '';
			//cache list items object
			var $newLi = $newUl.children();

			//get heights of new elements for use later
			var containerDivOptions = $newUl.height();

			$containerDivText.text(opts.defaultText);

			navigateList();

			$containerDivText.click(function() {
				if ($containerDivOptions.is(':visible')) {
					$containerDivOptions.hide();
					$containerDiv.css('position', 'relative');
					return false;
				}

				$containerDiv.focus();

				//show list
				$containerDivOptions.slideDown(opts.animationSpeed);
				$containerDiv.css('position', 'static');

			});

			//add classes on hover
			$containerDivText.hover(
				function(e) {
					var $hoveredTxt = $(e.target);
					$hoveredTxt.parent().addClass('newListSelHover');
				},
				function(e) {
					var $hoveredTxt = $(e.target);
					$hoveredTxt.parent().removeClass('newListSelHover');
				}
			);

			$newLi.hover(
				function(e) {
					var $hoveredLi = $(e.target);
					$hoveredLi.addClass('newListHover');
				},
				function(e) {
					var $hoveredLi = $(e.target);
					$hoveredLi.removeClass('newListHover');
				}
			);

			$selectAllLi.click(function(e) {
				var $clickedLi = $(e.target);
				//update counter

				if ($clickedLi.hasClass('hiLite')) {
					$(this).removeClass('hiLite');
					$newLi.removeClass('hiLite');
					$input.children().removeAttr("selected");
				} else {
					$(this).addClass('hiLite');
					$newLi.addClass('hiLite');
					$input.children().attr('selected', 'selected');
				}

				//remove all hilites, then add hilite to selected item
				navigateList();
			});

			$newLi.click(function(e) {
				var $clickedLi = $(e.target);
				//update counter
				currentIndex = $newLi.index($clickedLi);

				if ($input.children().eq(currentIndex).attr('selected')) {
					$(this).removeClass('hiLite');
					$input.children().eq(currentIndex).removeAttr("selected");
					$selectAllLi.removeClass('hiLite');
				} else {
					$(this).addClass('hiLite');
					$input.children().eq(currentIndex).attr('selected', 'selected');
				}

				//remove all hilites, then add hilite to selected item
				navigateList();
			});

			$(document).click(function(e) {
				if ($(e.target).parents(".newMultiListSelected").length != 0) return;
				if ($containerDivOptions.is(':visible')) {
					$containerDivOptions.hide();
					$containerDiv.css('position', 'relative');
					return false;
				}
			});

			function navigateList() {
				//$newLi.removeClass('hiLite').eq(currentIndex).addClass('hiLite');
				//alert($input.children().find(':selected'))
				var multipleValues = [];
				var i = 0;
				$input.children().each(function() {
					var option = $(this).text();
					if ($(this).attr('selected')) {
						multipleValues[i++] = option;
					}
				});
				$containerDivText.text(multipleValues.join(", "));
				$containerDivText.attr("title", multipleValues);
			};

			$(".selectAll").click(function(e) {
				var target = $(e.target);

				if (target.hasClass('hiLite')) target.removeClass('hiLite');
				else target.addClass('hiLite');

				var checked = target.hasClass('hiLite') ? 'selected' : '';

				$input.children().each(function() {
					$(this).attr('selected', checked);
				});
				$newLi.each(function() {
					$(this).text();
					$(this).addClass('hiLite');
				});
				navigateList();
			});

			$containerScroll.slider({
				animate: true,
				orientation: 'vertical',
				change: handleSlider,
				slide: handleSlider,
				start: handleSlider,
				value: 100
			});

			function handleSlider(e, ui) {
				var maxScroll = $containerContent.attr("scrollHeight") - $containerContent.height();
				$containerContent.attr({ scrollTop: parseInt((100 - ui.value) * (maxScroll / 100)) });
			}

			//reset left property and hide
			$containerDivOptions.css('left', '-10px').hide();
		});
	};
})(jQuery);
