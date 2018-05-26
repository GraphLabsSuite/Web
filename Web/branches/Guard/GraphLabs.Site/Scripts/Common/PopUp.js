function createPopUp(windowId) {
    var closeCrossHTMLString = "<div id='pop-up-close'><img src=\"Images/PopUp/cross.png\" alt=\"Закрыть\" width=\"20px\" height=\"20px\" /></div>";
    $("html").prepend("<div class='pop-up'><div class='pop-up-content' id='" + windowId + "'>" + closeCrossHTMLString + "</div></div>");
};

$("#pop-up-close").live("click", function () {
    closePopUp();
});

function closePopUp() {
    $(".pop-up").detach();
};

