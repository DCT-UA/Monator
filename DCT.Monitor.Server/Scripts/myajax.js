var MyAjax = {
    CreateAjaxObject: function() {
        return new MyAjaxObject();
    }
};

function LoadResource(url) {
    if (this.loadComplite) {
        this.loadComplite = false;
        this.ajax.open('GET', url, true);
        this.ajax.send(null);
        return true;
    }
    return false;
}

function MyAjax_CreateAjaxObject() {
    r = false;
    try {
        r = new XMLHttpRequest();
    }
    catch (trymicrosoft) {
        try {
            r = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (othermicrosoft) {
            try {
                r = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (failed) {
                r = false;             
            }
        }
    }
    return r;
}

function MyAjax_OnLoaded() {
    this.loadComplite = true;
    if (this.ajax.readyState == 4) {
        this.onLoadComplite(this.ajax.responseText);
    }
}

function MyAjaxObject() {
    this.ajax = MyAjax_CreateAjaxObject();
    this.loadComplite = true; 
    this.MyAjax_OnLoaded = MyAjax_OnLoaded;
    this.LoadResource = LoadResource;
    var v = this;
    this.ajax.onreadystatechange = function() { v.MyAjax_OnLoaded() };
    this.onLoadComplite = false;
}