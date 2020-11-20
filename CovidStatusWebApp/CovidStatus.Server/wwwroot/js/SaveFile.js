function FileSaveAs(strData, strFileName, strMimeType) {
    var newdata = "data:" + strMimeType + ";base64," + escape(strData);
    var a = window.document.createElement('a');
    //build download link:
    a.href = newdata;
    a.download = strFileName;
    // Append anchor to body.
    document.body.appendChild(a);
    a.click();
    // Remove anchor from body
    document.body.removeChild(a);
};