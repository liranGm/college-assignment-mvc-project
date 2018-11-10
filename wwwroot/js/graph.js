// graph.js

function createPopularGuidesGraph(data) {

    var w = 300,                        //width
        h = 300,                            //height
        r = 100,                            //radius
        color = d3.scale.category20c();     //builtin range of colors

          // load the data
    data.forEach(function (d) {
        d.label = d.guideName;
        d.value = d.timesOrderd;
    });

    var vis = d3.select("body")
        .append("svg:svg")              //create the SVG element inside the <body>
        .data([data])                   //associate our data with the document
        .attr("width", w)           //set the width and height of our visualization (these will be attributes of the <svg> tag
        .attr("height", h)
        .append("svg:g")                //make a group to hold our pie chart
        .attr("transform", "translate(" + r + "," + r + ")")    //move the center of the pie chart from 0, 0 to radius, radius

    var arc = d3.svg.arc()              //this will create <path> elements for us using arc data
        .outerRadius(r);

    var pie = d3.layout.pie()           //this will create arc data for us given a list of values
        .value(function (d) { return d.value; });    //we must tell it out to access the value of each element in our data array

    var arcs = vis.selectAll("g.slice")     //this selects all <g> elements with class slice (there aren't any yet)
        .data(pie)                          //associate the generated pie data (an array of arcs, each having startAngle, endAngle and value properties) 
        .enter()                            //this will create <g> elements for every "extra" data element that should be associated with a selection. The result is creating a <g> for every object in the data array
        .append("svg:g")                //create a group to hold each slice (we will have a <path> and a <text> element associated with each slice)
        .attr("class", "slice");    //allow us to style things in the slices (like text)

    arcs.append("svg:path")
        .attr("fill", function (d, i) { return color(i); }) //set the color for each slice to be chosen from the color function defined above
        .attr("d", arc);                                    //this creates the actual SVG path using the associated data (pie) with the arc drawing function

    arcs.append("svg:text")                                     //add a label to each slice
        .attr("transform", function (d) {                    //set the label's origin to the center of the arc
            //we have to make sure to set these before calling arc.centroid
            d.innerRadius = 0;
            d.outerRadius = r;
            return "translate(" + arc.centroid(d) + ")";        //this gives us a pair of coordinates like [50, 50]
        })
}

//    var margin = { top: 20, right: 20, bottom: 70, left: 40 },
//        width = 600 - margin.left - margin.right,
//        height = 300 - margin.top - margin.bottom;


//    // set the ranges
//    var x = d3.scale.ordinal().rangeRoundBands([0, width], .05);

//    var y = d3.scale.linear().range([height, 0]);

//    // define the axis
//    var xAxis = d3.svg.axis()
//        .scale(x)
//        .orient("bottom");


//    var yAxis = d3.svg.axis()
//        .scale(y)
//        .orient("left")
//        .ticks(10);


//    // add the SVG element
//    var svg = d3.select("#most-ordered-guides").append("svg")
//        .attr("width", width + margin.left + margin.right)
//        .attr("height", height + margin.top + margin.bottom)
//        .append("g")
//        .attr("transform",
//            "translate(" + margin.left + "," + margin.top + ")");


//    // load the data
//    data.forEach(function (d) {
//        d.guidesName = d.guideName;
//        d.numberOfOrders = d.timesOrderd;
//    });

//    // scale the range of the data
//    x.domain(data.map(function (d) { return d.guidesName; }));
//    y.domain([0, d3.max(data, function (d) { return d.numberOfOrders; })]);

//    // add axis
//    svg.append("g")
//        .attr("class", "x axis")
//        .attr("transform", "translate(0," + height + ")")
//        .call(xAxis)
//        .selectAll("text")
//        .style("text-anchor", "end")
//        .attr("dx", "-.8em")
//        .attr("dy", "-.55em")
//        .attr("transform", "rotate(-90)");

//    svg.append("g")
//        .attr("class", "y axis")
//        .call(yAxis)
//        .append("text")
//        .attr("transform", "rotate(-90)")
//        .attr("y", 5)
//        .attr("dy", ".71em")
//        .style("text-anchor", "end")
//        .text("NumberOfOrders");


//    // Add bar chart
//    svg.selectAll("bar")
//        .data(data)
//        .enter().append("rect")
//        .attr("class", "bar")
//        .attr("x", function (d) { return x(d.guidesName); })
//        .attr("width", x.rangeBand())
//        .attr("y", function (d) { return y(d.numberOfOrders); })
//        .attr("height", function (d) { return height - y(d.numberOfOrders); });
//}

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