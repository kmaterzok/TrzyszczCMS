function getSelectionRange(aElementId)
{
    var txtarea = document.getElementById(aElementId);
    var Start = txtarea.selectionStart;
    var End = txtarea.selectionEnd;
    return { Start, End };
}

function setSelectionRange(aElementId, aStart, aEnd) {
    var elem = document.getElementById(aElementId);
    elem.focus();
    elem.setSelectionRange(aStart, aEnd);
}