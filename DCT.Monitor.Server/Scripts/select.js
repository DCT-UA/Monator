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

	$.fn.sSelect = function(options) {

		return this.each(function() {

			var defaults = {
				defaultText: 'Please select',
				animationSpeed: 0, //set speed of dropdown
				ddMaxHeight: '' //set css max-height value of dropdown
			};

			//initial variables
			var opts = $.extend(defaults, options),
				$input = $(this),
				$containerDivText = $('<div class="selectedTxt"></div>'),
				$containerDiv = $('<div class="newListSelected" tabindex="0"></div>'),
				$containerDivOptions = $('<div class="newOptions"></div>'),
				$containerDivBorder = $('<div class="border"></div>'),
				$containerScroll = $('<div class="content-slider"></div>'),
				$containerContent = $('<div class="content-scroll"></div>'),


				$newUl = $('<ul class="newList"></ul>'),
				itemIndex = -1,
				currentIndex = -1,
				keys = [],
				prevKey = false,
				newListItems = '',
				prevented = false;

			//build new list
			$containerDiv.insertAfter($input);
			$containerDivText.prependTo($containerDiv);
			$containerScroll.appendTo($containerDivBorder);
			$newUl.appendTo($containerContent);
			$containerContent.appendTo($containerDivBorder);
			$containerDivBorder.appendTo($containerDivOptions);
			$containerDivOptions.appendTo($containerDiv);

			$input.hide();

			//test for optgroup
			if ($input.children('optgroup').length == 0) {
				$input.children().each(function(i) {
					var option = $(this).text();
					//add first letter of each word to array
					keys.push(option.charAt(0).toLowerCase());
					if ($(this).attr('selected') == true) {
						opts.defaultText = option;
						currentIndex = i;
					}
					newListItems += '<li>' + option + '</li>';
				});
				//add new list items to ul
				$newUl.html(newListItems);
				newListItems = '';
				//cache list items object
				var $newLi = $newUl.children();
			}

			//get heights of new elements for use later
			var containerDivOptions = $newUl.height(),
				containerHeight = $containerDiv.height(),
				newLiLength = $newLi.length;

			//check if a value is selected
			if (currentIndex != -1) {
				navigateList(currentIndex, true);
			} else {
				//set placeholder text
				$containerDivText.text(opts.defaultText);
			}

			//positioning
			function positionFix() {
				$containerDiv.css('position', 'relative');
			}

			function positionHideFix() {
				$containerDiv.css('position', 'static');
			}

			$containerDivText.click(function() {
				if ($containerDivOptions.is(':visible')) {
					$containerDivOptions.hide();
					positionFix()
					return false;
				}

				$containerDiv.focus();

				//show list
				$containerDivOptions.slideDown(opts.animationSpeed);
				positionHideFix();
				//scroll list to selected item
				$containerDivOptions.scrollTop($input.liOffsetTop);

			});

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

			$newLi.click(function(e) {
				var $clickedLi = $(e.target);
				//update counter
				currentIndex = $newLi.index($clickedLi);
				//remove all hilites, then add hilite to selected item
				prevented = true;
				navigateList(currentIndex);
				$containerDivOptions.hide();
				$containerDiv.css('position', 'static'); //ie
			});

			function navigateList(currentIndex, init) {

				//get offsets
				var containerOffsetTop = $containerDiv.offset().top,
					liOffsetTop = $newLi.eq(currentIndex).offset().top,
					ulScrollTop = $containerDivOptions.scrollTop();

				//get distance of current li from top of list				
				$input.liOffsetTop = ((liOffsetTop - containerOffsetTop) - containerHeight) + ulScrollTop;

				//scroll list to focus on current item
				$containerDivOptions.scrollTop($input.liOffsetTop);

				$newLi.removeClass('hiLite')
					.eq(currentIndex)
					.addClass('hiLite');
				var text = $newLi.eq(currentIndex).text();
				//page load
				if (init == true) {
					$input.val(text);
					$containerDivText.text(text);
					return false;
				}
				$input.val(text).change();
				$containerDivText.text(text);

			};

			$input.change(function(event) {
				$targetInput = $(event.target);
				//stop change function from firing 
				if (prevented == true) {
					prevented = false;
					return false;
				}
				$currentOpt = $targetInput.find(':selected');
				currentIndex = $targetInput.find('option').index($currentOpt);
				navigateList(currentIndex, true);
			}
			);

			//if (opts.ddMaxHeight != '') $containerContent.height(opts.ddMaxHeight);

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

			//hide list on blur

			/*$containerDiv.blur(function(event){
			$(this).removeClass('newListSelFocus');
			$containerDivOptions.hide();
			positionHideFix();
			});*/

			$(document).click(function(e) {
				if ($(e.target).parents(".newListSelected").length != 0) return;
				if ($containerDivOptions.is(':visible')) {
					$containerDivOptions.hide();
					$containerDiv.css('position', 'relative');
					return false;
				}
			});

			//add classes on hover
			$containerDivText.hover(function(e) {
				var $hoveredTxt = $(e.target);
				$hoveredTxt.parent().addClass('newListSelHover');
			},
			  function(e) {
			  	var $hoveredTxt = $(e.target);
			  	$hoveredTxt.parent().removeClass('newListSelHover');
			  }
			);

			//reset left property and hide
			$containerDivOptions.css('left', '-10px').hide();
		});

	};

})(jQuery);