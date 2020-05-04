function fileBrowse(self, labelID) {
    var fileName = $(self).val();
    var fileName = fileName.replace(/^.*\\/, "");
    //replace the "Choose a file" label
    $("#" + labelID).html(fileName);
}