function createPopUp(windowId) {
    $("html").prepend("<div class='pop-up'><div class='pop-up-content' id='" + windowId + "'></div></div>");
};

function closePopUp() {
    $(".pop-up").detach();
};