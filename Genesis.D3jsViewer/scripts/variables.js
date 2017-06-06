// ======================
// File input
// ======================
var fileNameParam = getParameterByName("file");
var fileName = !isNullOrEmptyOrWhiteSpaces(fileNameParam) ? fileNameParam : "data/sub-elem-tree.json";

// ======================
// Control / update variables
// ======================
var sizeChanged = true,        // whether the window size has changed
    nodeChanged = true,        // whether some node was expanded / collapsed
    colorsChanged = true;      // whether the colors have changed

// ======================
// D3 graph algorithm objects and svg elements
// ======================
var links, // the graph links
    nodes, // the graph nodes
    force, // d3 force
    link, // the d3 svg graph lines
    node; // the d3 svg graph circles

var topSvg, // d3 main svg
    svg, // d3 svg to contain all elements, including cluster tree
    backgRect; // d3 background rectangle


// ======================
// Layout variables
// ======================
var width = 800, height = 800; 

var linkDistance = 30, // the force link distance
      forceCharge = -120;   // the force charge

var showLabels = true, //false,             // whether to place node name inside circle
    straightLinks = false, // whether to draw straight links in tree
    nodeStrokeColor = "#000", // node stroke color from color picker
    nodeValueThreshold = 0.5, // the threshold for node expansion (updated by slider)
    nodeRadius = 15, // radius of node
    argNodeRadius = 7, // radius of ragument node of ordered symbol trees
    maxDepthDistance = 100, // the max distance between the several depths/levels of the tree
    treeMargin = 20, // the margin between the tree nodes and the border
    argNodeDistFactor = 10, //the distance factor between arg nodes and their parent symbol node in ordered symbol trees
    textDistance = 0; // distance of node text from center

var numNodes = 0, // number of expanded nodes (auto updated)
    maxDepth = 1, // current max depth of tree (auto updated)
    maxValue = 1; // current max node value in the tree (auto updated)


// ======================
// Style variables
// ======================
var expNodeColor = "white", // expanded node color (updated from back color)
    colNodeColor = "#eee", // collapsed node color (fixed)
    labelColor = "black", // color of labels (auto-updated)
    grayscale = false, // whether nodes colors appear in grayscale
    nodeColors, // color nodes palette (updated according to selected option)
    numColors = 20, // num of different colors in the palette
    rootStrokeDash = "2, 2"; // root node stroke dash pattern

var transitionTime = 600; //500; //150; //750;  // animation time (ms)

// ======================
// Pan zoom variables
// ======================
var zoomDragable = false, // whether the tree is zoomable & dragable
    panTimer = false,
    panSpeed = 200,
    panBoundary = 20; // within 20px from edges will pan when dragging

var minZoom = 0.5,
    maxZoom = 10,
    zoomable = false; //true;


// ======================
// Color palettes
// ======================

// select options 
var colorPaletteOptions = {
    "tol-sq": "Tol's Sequential",
    "tol-dv": "Tol's Diverging",
    "tol-rainbow": "Tol's Rainbow",
    "reds": "Reds",
    "greens": "Greens",
    "blues": "Blues",
    "yellows": "Yellows",
    "magentas": "Magentas",
    "cyans": "Cyans",
    "grayscale": "Linear Grayscale"
};

// palettes options 
var colorPalettes = {
    "tol-sq": function(n) { return window.palette("tol-sq", n) },
    "tol-dv": function(n) { return window.palette("tol-dv", n).reverse() },
    "tol-rainbow": function(n) { return window.palette("tol-rainbow", n) },
    "reds": function(n) { return generateLinearPalette(function(x) { return palette.linearRgbColor(x, 0, 0); }, n); },
    "greens": function(n) { return generateLinearPalette(function(x) { return palette.linearRgbColor(0, x, 0); }, n); },
    "blues": function(n) { return generateLinearPalette(function(x) { return palette.linearRgbColor(0, 0, x); }, n); },
    "yellows": function(n) {
        return generateLinearPalette(function(x) { return palette.linearRgbColor(x, x, 0); }, n);
    },
    "magentas": function(n) {
        return generateLinearPalette(function(x) { return palette.linearRgbColor(x, 0, x); }, n);
    },
    "cyans": function(n) { return generateLinearPalette(function(x) { return palette.linearRgbColor(0, x, x); }, n); },
    "grayscale": function(n) {
        return generateLinearPalette(function(x) { return palette.linearRgbColor(x, x, x); }, n);
    }
};

function generateLinearPalette(scheme, n) {
    const colors = [];
    for (let i = n - 1; i >= 0; i--)
        colors.push(scheme(i / (n - 1)));
    return colors;
}