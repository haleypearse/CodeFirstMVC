var uri = '/CodeFirstMVC/api/people';
const uriBase = '/CodeFirstMVC/api/people';
const uriVirtual = '/CodeFirstMVC';
const field = $("#filter-field");
const button = $("#filter-button");
const dateFormat = "LLL";
const request = new XMLHttpRequest();
const columnAnchorCollection = document.getElementsByClassName(".column-headers"); // $('.column-headers a');
var sort = { "column": "name", "asc": true };
var resjson = {};
var responseHeaders = "";
$(document).ready(function () {
    // --- FUNCTIONS ---
    function getResponseHeader() {
        responseHeaders = request.getAllResponseHeaders();
        // a Hacky way of getting the right header
        responseHeaders = responseHeaders.split('paging-headers: ')[1].split('\n')[0];
        return JSON.parse(responseHeaders);
    }
    function getUri() {
        var uri = uriBase + "?";
        for (var param in resjson) {
            //console.log(x + " " + resjson[x])
            if (resjson[param])
                uri += "&" + param + "=" + resjson[param];
            //console.log(param + ": " + resjson[param]);
        }
        console.log(uri);
        // uri += "Query=" + field.val();
        // uri += "&SortBy=" + sort["column"] + "&SortOrder=" + sort["asc"];
        return uri;
    }
    function addColumnSortLinks() {
        for (var a of columnAnchorCollection) {
            a.addEventListener("click", clickSortHandler);
        }
    }
    function clickSortHandler() {
        // Generate parameter names from anchor tags, strip white space 
        var paramName = this.innerHTML.replace(/\s/g, '');
        // If already applied, toggle ascending
        if (sort["column"] == paramName) {
            sort["asc"] = !sort["asc"];
        }
        else {
            // Apply new sort column
            sort["column"] = paramName;
            // restore default sort order
            sort["asc"] = true; //ascDefaults[paramName];
        }
        resjson["SortBy"] = sort["column"];
        resjson["SortOrder"] = sort["asc"];
        getTable(); //Refresh the table
    }
    function addPagination() {
        var pagination = "";
        for (var i = 1; i < 1 + resjson["totalPages"]; i++) {
            pagination += "<li><a class='pagination-link' href='#' data-page='" + i + "'>" + i + "</a></li>";
        }
        document.getElementById("pagination").innerHTML = pagination;
    }
    function getTable() {
        request.open('GET', getUri());
        request.send();
        request.onload = () => {
            if (request.status === 200) {
                resjson = getResponseHeader();
                //console.log(parsedHeader.currentPage);
                var table = "";
                for (const person of JSON.parse(request.response)) {
                    table += "<tr>"; //new row
                    table += "<td><a href='" + uriVirtual + "/people/edit/" + person["name"] + "'>" + person["name"] + "</a></td>";
                    table += "<td>" + person["timesMet"] + "</td>";
                    //table += "<td>" + moment(person["whenMet"]).format(dateFormat) + "</td>";
                    //table += "<td>" + moment(person["lastMet"]).format(dateFormat) + "</td>";
                    table += "<td>" + person["whenMet"] + "</td>";
                    table += "<td>" + person["lastMet"] + "</td>";
                    table += "<td><button data-person-name='" + person["name"] + "' class='btn-link js-delete'>Delete</button> <a href='" + uriVirtual + "/people/details/" + person["name"] + "'>Details</a></td>";
                    table += "</tr>";
                }
                document.getElementById("table-body").innerHTML = table;
                addPagination();
            }
        };
        request.onerror = () => console.log("error");
    }
    // --- ADD EVENT LISTENERS ---
    // Query db upon button press
    //console.log(button);
    button.on("click", () => getTable());
    // Query db upon each keystroke
    field.on('change keydown paste input', () => {
        var fieldval = field.val();
        resjson["Query"] = field.val();
        getTable();
    });
    // Query db upon pressing enter and prevent form submit
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault(); //Don't submit the form
            getTable();
        }
    });
    //DELETE person (and set Meetings.Person to null for those entries)
    $("#people").on("click", ".js-delete", function () {
        var button = $(this);
        if (confirm("Are you sure you want to delete this person?")) {
            //console.log("confirm delete");
            $.ajax({
                url: uriBase + "/" + $(button).attr("data-person-name"),
                method: "DELETE",
                success: function () {
                    console.log("success filter button");
                    button.parents("tr").remove();
                }
            });
        }
        ;
    });
    $("#pagination").on("click", ".pagination-link", function () {
        // this = <a class="pagination-link" href="#" data-page="1">1</a>
        // console.log(this.getAttribute("data-page")); // "1"
        resjson["pageNumber"] = this.getAttribute("data-page");
        getTable();
    });
    // --- RUN ON LOAD ---
    addColumnSortLinks();
    getTable();
});
//# sourceMappingURL=app.js.map