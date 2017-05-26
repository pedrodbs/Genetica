function loadFile(files) {
    // checks file
    if (files.length > 0) {
        const file = files[0];
        console.info(`Reading Json tree data from file ${file.name}...`);
        window.fileName = file.name;

        // reads file as json
        const reader = new FileReader();
        reader.onload = function (event) {
            // reads data and loads tree
            const treeObj = JSON.parse(event.target.result);
            window.readData(treeObj);
            return true;
        }

        reader.readAsText(file);
    }
    return false;
}

function loadFromUrl() {
    console.info(`Reading Json tree data from file ${window.fileName}...`);
    d3.json(window.fileName,
        function(error, treeObj) {
            if (error) {
                console.error(error);
                return false;
            }

            // reads data and loads tree
            window.readData(treeObj);

            return true;
        });
}

function readData(treeObj) {
    if (window.isNull(treeObj)) {
        
        window.update();
        return false;
    } else {

        // changes title
        document.title = `Tree Visualizer - ${window.fileName}`;

        //reads root element
        window.root = treeObj;

        //initiates tree and all elements' dimensions according to window
        window.update();

        //expands only root node (not children)
        window.collapseAll();

        return true;
    }
}

function initUI() {

    //creates d3 tree layout
    window.tree = d3.layout.tree()
        .size([window.width, window.height]);

    //adds main svg
    window.topSvg = d3.select("#container").append("svg")
        .attr("id", "topSvg")
        .call(zoomListener);

    // adds tree svg
    window.svg = window.topSvg.append("svg:svg")
        .attr("id", "innerSvg");

    // adds a rectangle as a background
    window.backgRect = window.svg.append("rect")
        .attr("id", "backgRect")
        .attr("width", "100%")
        .attr("height", "100%")
        .style("fill", "none");
    document.getElementById("color-picker").value = "FFF";
    document.getElementById("pick-color-btn").style.color = "black";

    // adds color options to select
    const select = document.getElementById("color-scheme-select");
    for (let key in window.colorPaletteOptions) {
        select.options[select.options.length] = new Option(window.colorPaletteOptions[key], key);
    }

    // disables file upload button if not supported
    if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
        document.getElementById("load-file-btn").disabled = true;
    }

    //reads data and initializes graph
    loadFromUrl();
}

window.onresize = function () {
    update();
};

function update() {
    // console.info("Updating elements");
    
    //updates variables based on UI 
    updateVariables();
    
    //updates UI elements dimensions
    updatePageElements();
    
    //updates tree
    window.updateTree();
}

function onValueThresholdChanged(value) {
    window.nodeValueThreshold = Number(value);
    window.update();
}

function updateVariables() {
    
    //reads all options from the html elements
    window.vertLayout = document.getElementById("vert-layout-chkbox").checked;
    window.nodeNameWithin = document.getElementById("name-within-chkbox").checked;
    window.grayscale = document.getElementById("grayscale-chkbox").checked;
    window.straightLinks = document.getElementById("straight-chkbox").checked;
    window.zoomDragable = document.getElementById("zoom-chkbox").checked;
    window.nodeStrokeColor = `#${document.getElementById("color-picker").value}`;
    window.labelColor = document.getElementById("pick-color-btn").style.color;
    
    window.nodeRadius = window.nodeNameWithin ? 15 : 4.5,     //radius of nodes
    window.textDistance = window.nodeNameWithin ? 0 : 18;     //the distance of the node's text to its center
}

function updatePageElements() {

    // calculates max dimensions
    const windowDiscount = 20;
    const optionsWidth = document.getElementById("expand-button").offsetWidth + 20;
    window.width = window.innerWidth - optionsWidth - windowDiscount;
    window.height = window.innerHeight - windowDiscount;
            
    //modifies divs dimensions
    document.getElementById("options-column").style.width = optionsWidth + "px";
    document.getElementById("container").style.width = window.width + "px";
    document.getElementById("container").style.height = window.height + "px";

    window.backgRect.style("fill", `#${document.getElementById("color-picker").value}`);

    //updates threshold slider text and value
    document.getElementById("value-threshold-slider-value").innerHTML = window.nodeValueThreshold.toFixed(3);
    document.getElementById("value-threshold-slider").value = window.nodeValueThreshold;
    
    //updates d3 elements
    window.topSvg.attr("width", window.width).attr("height", window.height);
    window.svg.attr("width", window.width).attr("height", window.height);
    window.tree.size(window.vertLayout ? [window.width, window.height] : [window.height, window.width]);

    // reads selected palette and create colors
    const select = document.getElementById("color-scheme-select");
    const selectedPalette = select.options[select.selectedIndex].value;
    window.nodeColors = window.colorPalettes[selectedPalette](window.numColors);
}