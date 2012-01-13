/*********** options *************
* content - id of content div block
* right - slider position. if true - slider at the right side
* width
* height
*/

function getElementPosition(elem) {
    var l = 0;
    var t = 0;
    while (elem) {
        l += elem.offsetLeft;
        t += elem.offsetTop;
        elem = elem.offsetParent;
    }
    return { "left": l, "top": t };
}

function InitMarkup() {
    if (!this.options.delay) this.options.delay = 100;
    
    var sliderid = this.options.content + "Placeholder"
    var m = '<div style="position:absolute; z-index:' + this.options.zorder + '" id="' + sliderid + '">'
                + '<table style="width:auto;">'
                    + '<tr><td class="r1" style="vertical-align:top">x</td><td class="r2"></td></tr>'
                + '</table></div>';
    
    var sliderContainer = $('#' + this.options.placeholder);
    var dataContainer = $("#" + this.options.content);
    var data = dataContainer.html();
    var activator = sliderContainer.html();
    var pos = getElementPosition(sliderContainer.get(0));
       
    dataContainer.html('');    
    sliderContainer.html(m);
    
    var slider = $("#" + this.options.content + "Placeholder");
    this.slider = slider.get(0);
    
    var r1 = $(".r1", slider);
    var r2 = $(".r2", slider);
    
    r1.html(activator);
    r2.html(data);
    
    this.width = this.slider.offsetWidth;
    this.visibleWidth = r1.get(0).offsetWidth;
    this.dx = (this.width - this.visibleWidth) / this.options.discret;
    this.width -= this.visibleWidth;
    
    this.xPos = pos.left;
    if (!this.options.right) pos.left += this.width - this.visibleWidth;
   
    this.slider.style.top = pos.top + "px";
    this.slider.style.left = pos.left + "px";
    this.slider.style.clip = "rect(0," + this.visibleWidth + "px,auto,0)"

    sliderContainer.get(0).style.height = r1.get(0).offsetHeight + "px";
    sliderContainer.get(0).style.width = r1.get(0).offsetWidth + "px";
    
    var r = this;
    $(slider).bind("mouseover", r, function(e) {
        e.data.Slide(true);
    }).bind("mouseout", r, function(e) {
        e.data.Slide(false);
    }); 
}

function Slide(bflag) {
    if (bflag == this.b && this.timer != -1) return;
    this.b = bflag;
    var r = this;
    if (r.options.discret == 0) {
        r.FastSlide();
    }
    else {
        r.Stop();
        r.timer = setInterval(function() { r.SlideHandler(); }, r.options.delay);
    }
}

function SlideHandler() {
    var trueLeft = this.slider.offsetLeft;
    var vis;
    if (this.options.right) {
        if (this.b) {
            trueLeft -= this.dx;
            if (trueLeft < this.xPos - this.width) {
                trueLeft = this.xPos - this.width;
                this.Stop();
            }
        }
        else {
            trueLeft += this.dx;
            if (trueLeft > this.xPos) {
                trueLeft = this.xPos;
                this.Stop();
            }
        }
        vis = this.xPos - trueLeft + this.visibleWidth;
    } else {
        if (this.b) {
        }
        else { }
    }
    this.slider.style.left = trueLeft + "px";
    if (this.options.right) {
        this.slider.style.clip = "rect(0," + vis + "px,auto,0)";// + this.options.width + 
    }
}

function FastSlide() {
    var left = this.xPos;
    var vis;
    if (this.b) {
        if (this.options.right) left -= this.width;
    }
    if (this.options.right) {
        vis = this.xPos - left + this.visibleWidth;
    }
    this.slider.style.left = left + "px";
    if (this.options.right) {
        this.slider.style.clip = "rect(0," + vis + "px,auto,0)"; // + this.options.width + 
    }
}

function Stop() {
    if (this.timer != -1) {
        clearInterval(this.timer);
        this.timer = -1;
    }
}

function SliderPanel(options) {
    this.timer = -1;
    this.constructor = SliderPanel;
    this.options = options;
    this.InitMarkup = InitMarkup;
    this.SlideHandler = SlideHandler;
    this.Slide = Slide;
    this.Stop = Stop;
    this.FastSlide = FastSlide;
    this.InitMarkup();   
}

