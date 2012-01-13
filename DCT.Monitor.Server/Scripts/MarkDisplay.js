function init() {
    this.stars = $("div", this.element.get(0));
    var r = this;
    this.stars.each(function(i) {
        $(this).bind("click", i, function(e) {
            r.SetMark(e.data);
        }).bind("mouseover", i, function(e) {
            r.ChangeMark(e.data);
        });
    }).bind("mouseout", function() {
        r.ResetMark();
    });
}

function ltrim(str) {
    var ptrn = /\s*((\S+\s*)*)/;
    return str.replace(ptrn, "$1");
}

function GetClass(j, i) {
    return (j < i) ? this.options.markedClass : this.options.unmarkedClass;
}

function SetMark(i) {
    if (this.OnSetMark) {
        this.OnSetMark(this, i + 1);
    }
}

function ChangeMark(i) {
    var r = this;
    this.stars.each(function(j) {
        $(this).attr("class", r.GetClass(j, i + 1));
    });
}

function ResetMark() {
    var r = this;
    this.stars.each(function(i) {
        $(this).attr("class", r.GetClass(i, r.mark));
    });
}

function MarkDisplay(element, options) {
    this.options = options;
    this.init = init;
    this.ResetMark = ResetMark;
    this.SetMark = SetMark;
    this.ChangeMark = ChangeMark;
    this.GetClass = GetClass;
    this.element = $(element);
    this.OnSetMark = false;

    this.mark = parseInt(this.element.attr('mark'));
    this.data = this.element.attr('url');
    this.active = this.data != "";
    
    if(this.active) this.init();
}