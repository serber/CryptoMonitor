$(document).ready(function () {
    var table = $("#drop-price-table").DataTable({
        serverSide: true,
        bProcessing: true,
        bAutoWidth: false,
        searching: false,
        dom: "ptr",
        lengthMenu: [20, 50, 100],
        language: {
            sProcessing: "Please wait, it will take a few seconds",
            emptyTable: "There are no items to display",
            decimal: ",",
            thousands: "."
        },
        columns: [
            { title: "Symbol", data: "SellSymbol", name: "SellSymbol", orderable: true, width: "20%" },
            { title: "Drop price", data: "Price", name: "Price", orderable: true, width: "30%" },
            { title: "Symbol price", data: "SymbolPrice", name: "SymbolPrice", orderable: true, width: "30%" },
            { title: "Multiplier", data: null, orderable: false, width: "25%" }
        ],

        order: [[0, "asc"]],

        columnDefs: [
            {
                render: function (data, type, row) {
                    if (data) {
                        return parseFloat(data).toFixed(10);
                    }

                    return "";
                },
                targets: [1, 2]
            },
            {
                render: function (data, type, row) {
                    return (parseFloat(row.Price) / parseFloat(row.SymbolPrice)).toFixed(4);
                },
                targets: [3]
            }
        ],

        rowCallback: function (row, data, index) {
            if (data.change > 0) {
                $(row).addClass("plus");
            }
            else if (data.change < 0) {
                $(row).addClass("minus");
            }
        },

        ajax: {
            url: "dropprice/data",
            type: "GET",
            contentType: "application/json",
            data: function (model) {
                var formData = {};

                var source = $("input[type='radio'][name='btnradio_symbolSource']:checked").val();
                var symbol = $("input[type='radio'][name='btnradio_symbol']:checked").val();

                formData.SymbolSource = source;
                formData.BuySymbol = symbol;

                formData.OrderBy =
                    model.order.map(col => (col.dir === "asc" ? "" : "^") + model.columns[col.column].name)
                        .join(",");

                formData.Take = model.length;
                formData.Skip = model.start;

                return $.param(formData);
            },
            dataSrc: function (json) {
                return json.data;
            },
            error: function (err) {
                alert(err.responseJSON.Message);
            }
        }
    });

    $("input[name='btnradio_symbolSource']").on("change", function () {
        table.ajax.reload();
    });

    $("input[name='btnradio_symbol']").on("change", function () {
        table.ajax.reload();
    });

    $("#search").on("change", function () {
        table.ajax.reload();
    });
});