window.getElementWidth = (elementId) => {
    let elems = document.getElementsByClassName(elementId);
    for (var i = 0; i < elems.length; i++) {
        return elems[i].offsetWidth;
    }

    //const element = document.getElementById(elementId);
    //if (element) {
    //    return element.offsetWidth;
    //} else {
    //    return 0;
    //}
};

window.getElementHeight = (elementId) => {
    let elems = document.getElementsByClassName(elementId);
    for (var i = 0; i < elems.length; i++) {
        return elems[i].offsetHeight;
    }
    //const element = document.getElementById(elementId);

    //if (element) {
    //    return element.offsetHeight;
    //} else {
    //    return 0;
    //}
};