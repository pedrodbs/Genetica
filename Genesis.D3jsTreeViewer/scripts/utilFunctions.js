function clone(obj) {
    var copy;

    // Handle the 3 simple types, and null or undefined
    if (null == obj || "object" != typeof obj) return obj;

    // Handle Date
    if (obj instanceof Date) {
        copy = new Date();
        copy.setTime(obj.getTime());
        return copy;
    }

    // Handle Array
    if (obj instanceof Array) {
        copy = [];
        for (let i = 0; i < obj.length; i++) {
            copy[i] = clone(obj[i]);
        }
        return copy;
    }

    // Handle Object
    if (obj instanceof Object) {
        copy = {};
        for (let attr in obj) {
            if (obj.hasOwnProperty(attr)) copy[attr] = clone(obj[attr]);
        }
        return copy;
    }

    throw new Error("Unable to copy obj! Its type isn't supported.");
}

function isNullOrEmptyOrWhiteSpaces(str) {
    return isNull(str) || isEmpty(str) || isBlank(str) || str.isEmpty();
}

function isNull(myVar) {
    return myVar == null;
}

function isEmpty(str) {
    return (!str || 0 === str.length);
}

function isBlank(str) {
    return (!str || /^\s*$/.test(str));
}

String.prototype.isEmpty = function() {
    return (this.length === 0 || !this.trim());
};

function getFileName(path) {
    return isNull(path) ? null : path.replace(/^.*[\\\/]/, "");
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    const regex = new RegExp(`[\\?&]${name}=([^&#]*)`);
    const results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function saveImage() {

    const prefix = {
        xmlns: "http://www.w3.org/2000/xmlns/",
        xlink: "http://www.w3.org/1999/xlink",
        svg: "http://www.w3.org/2000/svg"
    };

    const svg = document.getElementById("innerSvg");
    const svgImgFile = getFileName(window.fileName);

    console.info(`Saving ${svgImgFile}...`);

    // add empty svg element
    const emptySvg = window.document.createElementNS(prefix.svg, "svg");
    window.document.body.appendChild(emptySvg);
    const emptySvgDeclarationComputed = getComputedStyle(emptySvg);

    //opens url with svg content
    const svgInfo = getSvgInfo(svg, emptySvgDeclarationComputed);
    svgInfo.id = svgImgFile;
    download(svgInfo);

    //removes empty element
    window.document.body.removeChild(emptySvg);
}