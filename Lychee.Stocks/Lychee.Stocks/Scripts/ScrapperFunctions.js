//we already added jquery so we should be able to use jquery function here

function getHtml(element, selector) {
    return $(element).find(selector).html(); //add more logic here like trim()
    //return element.innerText; 
}

function getValue(element, selector) {
    return $(element).find(selector).val(); //add more logic here like trim()
    //return element.value;
}

function getAttribute(element, selector, attributeName) {
    return $(element).find(selector).attr(attributeName); //add more logic here like trim()
    //return element.getAttribute(attributeName);
}

function getText(element, selector) {
    return $(element).find(selector).text();
}

function setObject(obj, item, element) {
    if (item.accessor === 'Html') {
        obj[item.key] = getHtml($(element), item.selector);
    }
    else if (item.accessor === 'Value') {
        obj[item.key] = getValue($(element), item.selector);
    }
    else if (item.accessor === 'Attribute') {
        obj[item.key] = getAttribute($(element), item.selector, item.attributeName);
    }
    else if (item.accessor === 'Text') {
        obj[item.key] = getText($(element), item.selector);
    }
}

//function dropDownSelectedText(element) {
//    return element.options[element.selectedIndex].text;

//}