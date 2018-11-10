// graph.js

function createPopularGuidesGraph(data) {

    var w = 400,
        h = 400,
        r = 200,
        color = d3.scale.category20c();

          // load the data
    data.forEach(function (d) {
        d.label = d.guideName;
        d.value = d.timesOrderd;
    });
    var vis = d3.select("#most-ordered-guides")
        .append("svg:svg")
        .data([data])
        .attr("width", w)
        .attr("height", h)
        .append("svg:g")
        .attr("transform", "translate(" + r + "," + r + ")")

    var arc = d3.svg.arc()
        .outerRadius(r);

    var pie = d3.layout.pie()
        .value(function (d) { return d.value; });

    var arcs = vis.selectAll("g.slice")
        .data(pie)
        .enter()
        .append("svg:g")
        .attr("class", "slice");


     arcs.append("svg:path")
        .attr("fill", function (d, i) { return color(i); })
        .attr("d", arc);

    arcs.append("svg:text")
        .attr("transform", function (d) {
            d.innerRadius = 0;
            d.outerRadius = r;
            return "translate(" + arc.centroid(d) + ")";
        })
        .attr('text-anchor', 'middle')
        .attr('font-size', '1em')
        .text(function (data) {
            return data.data.guideName;
        });
}

function createPopularTripsGraph(data) {

    var margin = { top: 20, right: 20, bottom: 70, left: 40 },
    width = 600 - margin.left - margin.right,
    height = 300 - margin.top - margin.bottom;


    // set the ranges
    var x = d3.scale.ordinal().rangeRoundBands([0, width], .05);

    var y = d3.scale.linear().range([height, 0]);

    // define the axis
    var xAxis = d3.svg.axis()
        .scale(x)
        .orient("bottom");

    var yAxis = d3.svg.axis()
        .scale(y)
        .orient("left")
        .ticks(10);

    // add the SVG element
    var svg = d3.select("#most-ordered-trips").append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
      .append("g")
        .attr("transform",
              "translate(" + margin.left + "," + margin.top + ")");


    // load the data
        data.forEach(function (d) {
            d.tripsName = d.tripName;
            d.numberOfOrders = d.timesOrderd;
        });

        // scale the range of the data
        x.domain(data.map(function (d) { return d.tripsName; }));
        y.domain([0, d3.max(data, function (d) { return d.numberOfOrders; })]);

        // add axis
        svg.append("g")
            .attr("class", "x axis")
            .attr("transform", "translate(0," + height + ")")
            .call(xAxis)
          .selectAll("text")
            .style("text-anchor", "end")
            .attr("dx", "-.8em")
            .attr("dy", "-.55em")
            .attr("transform", "rotate(-90)");

        svg.append("g")
            .attr("class", "y axis")
            .call(yAxis)
          .append("text")
            .attr("transform", "rotate(-90)")
            .attr("y", 5)
            .attr("dy", ".71em")
            .style("text-anchor", "end")
            .text("NumberOfOrders");


        // Add bar chart
        svg.selectAll("bar")
            .data(data)
          .enter().append("rect")
            .attr("class", "bar")
            .attr("x", function (d) { return x(d.tripsName); })
            .attr("width", x.rangeBand())
            .attr("y", function (d) { return y(d.numberOfOrders); })
            .attr("height", function (d) { return height - y(d.numberOfOrders); });

}