
const GET = "GET";
const POST = "POST";

// #region Aias
function OzAjax(url, method, sendData, cllBckFnSuccess, cllBckFnError, cllBckFnProcess, multipart) {
    this.url = url;
    this.method = method || GET;
    this.sendData = sendData;
    this.cllBckFnSuccess = cllBckFnSuccess;
    this.cllBckFnProcess = cllBckFnProcess;
    this.cllBckFnError = cllBckFnError;
    this.multipart = multipart;
    this.Running = false;
}
OzAjax.prototype.BasitGonder = function () {
    this.Running = true;
    $.ajax({
        url: this.url,
        method: this.method,
        data: this.sendData,
        dataType: "text",
        success: this.cllBckFnSuccess,
        error: this.cllBckFnError || this.Error
    });
    this.Running = false;
};
OzAjax.prototype.MultiGonder = function () {
    var that = this;
    this.Running = true;
    $.ajax({
        url: this.url,
        method: "POST",
        data: this.sendData,
        dataType: "text", 
        enctype: "multipart/form-data",
        timeout: 180000,
        contentType: false,
        processData: false,
        cache: false,
        async: true,
        success: this.cllBckFnSuccess,
        error: this.Error,
        xhr: function () {
            var myxhr = $.ajaxSettings.xhr();
            if (myxhr.upload) {
                myxhr.upload.addEventListener("progress", that.cllBckFnProcess || that.Process);// function (event) { AyakMultiFormProgress(event, processCallBack); });
            }
            return myxhr;
        }
    });
    this.Running = false;
};
OzAjax.prototype.Error = function (hata) {
    new OzModal().Bilgi("Bağlantı hatası.. ", 2000);
};
OzAjax.prototype.Process = function (event) {
    var percent = 0;
    var position = event.loaded || event.position;
    var total = event.total;
    if (event.lengthComputable) {
        percent = Math.ceil(position / total * 100);
    }
    this.cllBckFnProcess(percent);
};

$().addClass("");
// #endregion

// #region Ozmodal
function OzModal() {
    this.ozmodal = $("#ozModal");
    this.ozicerik = $("#ozIcerik");
    this.ozBaslik = $("#ozModalBaslik");
    this.bilgiUsed = 0;
}
OzModal.prototype.Bilgi = function (bilgi, sure, baslik) {

    this.ozicerik.html(bilgi);
    this.ozBaslik.html(baslik || "Bilgi");
    this.ozmodal.modal("show");
    setTimeout(this.Kapat, sure || 1200);
    this.bilgiUsed++;
};
OzModal.prototype.Sayfa = function (icerik, baslik) {
    this.ozBaslik.html(baslik);
    this.ozicerik.html(icerik);
    this.ozmodal.modal("show");
};
OzModal.prototype.Kapat = function () {
    $("#ozModal").modal("hide");
    if(this.bilgiUsed>0)
    this.bilgiUsed--;
    
    if (this.bilgiUsed < 1)
        $("#ozModal").modal("hide");
};
// #endregion

// #region OzOnay
function OzOnay() {
    this.soruTxt = "";
    this.evetTxt = "";
    this.hayirTxt = "";
    this.soruCls = "";
    this.evetCls = "";
    this.hayirCls = "";
    this.sorumodal = new OzModal();
}
OzOnay.prototype.Sor = function (callEvet, callHayir) {
    $("#ozOnaySoruDiv").removeClass().addClass(this.soruCls).html(this.soruTxt);
    $("#ozOnayEvetBtn").removeClass().addClass(this.evetCls).html(this.evetTxt);
    $("#ozOnayHayirBtn").removeClass().addClass(this.hayirCls).html(this.hayirTxt);
    var sorumodal = this.sorumodal;
    sorumodal.Sayfa($("#ozOnayDiv").html());
    
    document.getElementById("ozOnayEvetBtn").addEventListener("click", evetInvoked);
    document.getElementById("ozOnayHayirBtn").addEventListener("click", hayirInvoked);

    function evetInvoked() {
        callEvet();
    }
    function hayirInvoked() {
        if (callHayir) callHayir();
        sorumodal.Kapat();
    }
}
// #endregion




    

