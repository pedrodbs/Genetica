function updateNode(source) {

    // compute the new tree layout
    window.nodes = window.tree.nodes(window.root).reverse();
    window.links = window.tree.links(window.nodes);

    // normalize for fixed-depth
    const dist = ((window.vertLayout ? window.height : window.width) - 2 * window.treeMargin - 2 * window.nodeRadius) /
        Math.max(window.maxDepth - 1, 1);
    window.nodes.forEach(function(d) {
        d.y = window.nodeRadius +
            window.treeMargin +
            d.depth * Math.min(window.vertLayout ? window.maxDepthDistance : 2 * window.maxDepthDistance, dist) +
            (isArgNode(d) ? - dist / window.argNodeDistFactor : 0);
    });

    // sets node separation proportional to the nodes' widths
    tree.separation(function(a, b) { return getWidth(a) + getWidth(b); });

    // update the nodes
    var node = window.svg.selectAll("g.node")
        .data(window.nodes, function(d) { return d.id || (d.id = ++window.numNodes); });

    // enter any new nodes at the parent's previous position
    var nodeEnter = node.enter().append("g")
        .attr("id", function(d) { return d.id; })
        .attr("class", function(d) { return isArgNode(d) ? "node arg" : "node"; })
        .attr("transform",
            function() {
                return window.vertLayout
                    ? `translate(${source.x0},${source.y0})`
                    : `translate(${source.y0},${source.x0})`;
            })
        .on("click", click);

    var backColor = window.backgRect.style("fill");
    nodeEnter.append("ellipse")
        .attr("rx", 1e-6)
        .attr("ry", 1e-6)
        .style("stroke", function(d) { return getColor(d); })
        .style("fill", backColor);

    nodeEnter.append("text")
        .text(function (d) { return d.n; })
        .attr("text-anchor",
            function(d) {
                return window.vertLayout
                    ? "middle"
                    : (window.nodeNameWithin ? "middle" : d.children || d.c ? "end" : "start");
            })
        .attr("dominant-baseline", "middle")
        .style("fill", window.labelColor)
        .style("fill-opacity", 1e-6);

    // if not displaying names within nodes, change their position 
    if (window.nodeNameWithin) {
        node.select("text").attr("x", 0);
        node.select("text").attr("y", 0);
    } else {
        if (window.vertLayout)
            node.select("text").attr("y",
                function(d) { return (d.children || d.c) ? -window.textDistance : window.textDistance; });
        else
            node.select("text").attr("x",
                function(d) { return (d.children || d.c) ? -window.textDistance : window.textDistance; });
    }

    // transition nodes to their new position
    var nodeUpdate = node.transition()
        .duration(window.transitionTime)
        .attr("class",
            function(d) {
                return `${isArgNode(d) ? "node arg" : "node"} ${
                    d.children || d.c.length === 0 ? "expanded" : "collapsed"}`;
            })
        .attr("transform",
            function(d) {
                return window.vertLayout ? `translate(${d.x},${d.y})` : `translate(${d.y},${d.x})`;
            });

    // update nodes' radius and colors
    nodeUpdate.select("ellipse")
        .attr("rx",
            function(d) { return isArgNode(d) ? window.argNodeRadius : Math.max(window.nodeRadius, getWidth(d)); })
        .attr("ry", function(d) { return isArgNode(d) ? window.argNodeRadius : window.nodeRadius; })
        .style("fill", backColor)
        .style("stroke", function(d) { return getColor(d); });

    // update nodes' text
    nodeUpdate.select("text")
        .style("fill-opacity", 1)
        .style("fill", window.labelColor);

    // Transition exiting nodes to the parent's new position.
    var nodeExit = node.exit().transition()
        .duration(window.transitionTime)
        .attr("transform",
            function() {
                return window.vertLayout ? `translate(${source.x},${source.y})` : `translate(${source.y},${source.x})`;
            })
        .remove();

    nodeExit.select("ellipse")
        .attr("rx", 1e-6)
        .attr("ry", 1e-6);

    nodeExit.select("text")
        .style("fill-opacity", 1e-6);

    // Update the links
    var link = window.svg.selectAll("path.link")
        .data(window.links, function(d) { return d.target.id; });

    // Enter any new links at the parent's previous position.
    link.enter().insert("path", "g")
        .attr("class", "link")
        .style("stroke", function(d) { return getColor(d.target); })
        .attr("d",
            function() {
                const o = { x: source.x0, y: source.y0 };
                return window.diagonal({ source: o, target: o });
            });

    // Transition links to their new position.
    link.transition()
        .duration(window.transitionTime)
        .style("stroke", function(d) { return getColor(d.target); })
        .attr("d", window.diagonal)
    ;

    // Transition exiting nodes to the parent's new position.
    link.exit().transition()
        .duration(window.transitionTime)
        .attr("d",
            function() {
                const o = { x: source.x, y: source.y };
                return window.diagonal({ source: o, target: o });
            })
        .remove();

    // Stash the old positions for transition.
    window.nodes.forEach(function(d) {
        d.x0 = d.x;
        d.y0 = d.y;
    });
}

function isArgNode(d) {
    // checks if node is an argument node (in ordered symbol trees), ie, if the name is a small number
    return !isNaN(d.n) && parseInt(d.n)<=3;
}

function updateLayoutVariables() {
    window.maxDepth = window.getMaxDepth(window.root, 0) + 1;

    window.maxValue = -1;
    if (window.root.children)
        for (let child of window.root.children)
            window.maxValue = Math.max(window.maxValue, window.getMaxValue(child));
}

function getMaxDepth(node, curDepth) {

    //checks null node
    if (window.isNull(node)) return -1;

    //checks expanded children
    var nodeMaxDepth = curDepth;
    if (node.children)
        for (let child of node.children)
            nodeMaxDepth = Math.max(nodeMaxDepth, window.getMaxDepth(child, curDepth + 1));

    //returns max depth from this node
    return nodeMaxDepth;
}

function getMaxValue(node) {

    //checks null node
    if (window.isNull(node)) return -1;

    //checks expanded children
    var nodeMaxValue = node.v;
    if (node.children)
        for (let child of node.children)
            nodeMaxValue = Math.max(nodeMaxValue, window.getMaxValue(child));

    //returns max depth from this node
    return nodeMaxValue;
}

function getWidth(node) {
    return (node.n.length * window.nodeRadius) / 3.6;
}

function getColor(node) {
    const val = Math.min(1, parseFloat(node.v) / window.maxValue);
    const idx = Math.round(val * (window.numColors - 1));
    const colorValue = window.nodeColors[idx];
    return `#${window.grayscale ? getGrayscale(colorValue) : colorValue}`;
}

function getGrayscale(colorValue) {
    const c = parseInt(colorValue, 16);
    const func = function(r, g, b) {
        g = 0.2126 * r + 0.7152 * g + 0.0722 * b;
        return [g, g, g];
    };
    return func(c >> 16, (c >> 8) & 255, c & 255).map(function(v) {
        v = Math.floor(v);
        v = Number(v > 0 ? (v < 255 ? v : 255) : 0).toString(16);
        return v.length === 1 ? `0${v}` : v;
    }).join("");
}

function click(node) {
    // toggle children on click.
    if (node.children)
        collapseNode(node, false);
    else
        expandNode(node, false);

    //updates max depth
    updateLayoutVariables();

    // updates clicked node
    updateNode(node);
}

function collapseNode(node, colChild) {
    if (node.children) {
        //console.info(`Collapsing ${node.children.length} children from node: ${node.n}`);
        node.children = null;

        //also collapses children if needed
        if (colChild)
            for (let child of node.c)
                collapseNode(child, colChild);
    }
}

function expandNode(node, expChild) {
    if (node.c) {

        //checks to expand children (meaning that expand did not come from node click) and
        // only expand node if value is above threshold
        if (expChild && (!window.isNull(node.v) && (parseFloat(node.v) < window.nodeValueThreshold)))
            return;

        //console.info(`Expanding ${node.c.length} children from node: ${node.n}`);

        node.children = [];
        for (let child of node.c) {
            // verifies child's value
            if (parseFloat(child.v) >= window.nodeValueThreshold) {
                // adds to children list
                node.children.push(child);

                //also expands children if needed
                if (expChild) expandNode(child, expChild);
            }
        }
    }
}

function expandAll() {
    //expand all nodes from root
    collapseNode(window.root, true);
    expandNode(window.root, true);

    //updates max depth
    updateLayoutVariables();

    updateNode(window.root);
}

function collapseAll() {
    //collapse all nodes from root
    collapseNode(window.root, true);
    expandNode(window.root, false);

    //updates max depth
    updateLayoutVariables();

    updateNode(window.root);
}

function updateTree() {

    if (window.isNull(window.root)) return;

    // a line function for the tree's links
    var line = d3.svg.line()
        .x(function(point) { return window.vertLayout ? point.lx : point.ly; })
        .y(function(point) { return window.vertLayout ? point.ly : point.lx; });

    //creates tree diagonal (for children link plotting)
    window.diagonal = window.straightLinks
        ? function(d) {
            const points = [
                { lx: d.source.x, ly: d.source.y },
                { lx: d.target.x, ly: d.target.y }
            ];
            return line(points);
        }
        : d3.svg.diagonal()
        .projection(function(d) {
            return window.vertLayout ? [d.x, d.y] : [d.y, d.x];
        });

    // initially centers root
    if (window.vertLayout) {
        window.root.x0 = window.width / 2;
        window.root.y0 = 10;
    } else {
        window.root.x0 = 10;
        window.root.y0 = window.height / 2;
    }

    //visually updates root node
    updateNode(window.root);
}