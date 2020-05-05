"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var moment = require("../Scripts/moment.min.js");
var bootbox = require("../Scripts/bootbox.min.js");
var uri = '/CodeFirstMVC/api/people';
$(document).ready(function () {
    var now = "@ViewBag.now";
    //console.log(now);
    //var field = $("#filter-field");
    var url = uri + "?searchString=" + $("#filter-field").val();
    var config = {
        "ajax": {
            //"url": uri + "?searchString=" + $("#filter-field").val(),
            "url": getUrl(),
            "dataSrc": ""
        },
        "columns": [
            {
                data: "",
                render: function (data, type, person) {
                    return "<a href='/people/edit/" + person.name + "'>" + person.name + "</a>";
                }
            },
            {
                data: "timesMet"
            },
            {
                data: "whenMet",
                render: function (date) {
                    if (date) {
                        return moment(date).format("L LT");
                        //return moment.duration(moment(now).format() - moment(date).format()).humanize();
                    }
                }
            },
            {
                data: "lastMet",
                render: function (date) {
                    if (date) {
                        return moment(date).format("L LT");
                    }
                }
            },
            {
                data: "name",
                render: function (data) {
                    return "<button data-person-name='" + data + "' class='btn-link js-delete'>Delete</button> <a href='/people/details/" + data + "'>Details</a> ";
                }
            }
        ]
    };
    var table = $("#people").DataTable(config);
    $("#filter-button").on("click", function () {
        //console.log("click button");
        url = uri + "?searchString=" + $("#filter-field").val();
        console.log(url);
        table.ajax.reload(console.log("table reloaded"));
        //var button = $(this);
        //var field = $("#filter-field");
        //; $.ajax({
        //    url: "/api/people/?searchString=" + $(field).val,
        //    method: "GET",
        //    success: function () {
        //        // console.log("delete success.");
        //        // button.parents("tr").remove();
        //    }
        //});
    });
    $("#filter-field").on("click", function () {
        console.log("field");
    });
    $("#people").on("click", ".js-delete", function () {
        var button = $(this);
        bootbox.confirm("Are you sure you want to delete this person?", function (result) {
            if (result) {
                $.ajax({
                    url: "/api/people/" + $(button).attr("data-person-name"),
                    method: "DELETE",
                    success: function () {
                        //console.log("success filter button");
                        // button.parents("tr").remove();
                        // TODO: remove from the table
                        //      table.render();
                    }
                });
            }
        });
        //if (confirm("Sure you want to delete?")) {
        //    //console.log("delete success.");
        //}
    });
});
function getUrl() {
    return uri + "?searchString=" + $("#filter-field").val();
}
//# sourceMappingURL=app.js.map