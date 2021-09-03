$(document).ready(function () {
    var table = $("#drop-price-table").DataTable({
        serverSide: true,
        bProcessing: true,
        bAutoWidth: false,
        searching: false,
        dom: "ptr",
        lengthMenu: [200],
        language: {
            sProcessing: "Please wait, it will take a few seconds",
            emptyTable: "There are no items to display",
            decimal: ",",
            thousands: "."
        },
        columns: [
            { title: "", data: null, orderable: false, width: "5%" },
            { title: "Symbol", data: "SellSymbol", name: "SellSymbol", orderable: true, width: "15%" },
            { title: "Drop price", data: "Price", name: "Price", orderable: true, width: "30%" },
            { title: "Symbol price", data: "SymbolPrice", name: "SymbolPrice", orderable: true, width: "30%" },
            { title: "Multiplier", data: "Multiplier", name: "Multiplier", orderable: true, width: "20%" }
        ],

        order: [[4, "desc"]],

        columnDefs: [
            {
                render: function (data, type, row) {
                    var formatButton =
                        (sellSymbol, buySymbol, symbolSource) =>
                            `<button type='button' class='btn btn-outline-danger btn-sm btn-delete-drop-price' data-sell-symbol='${sellSymbol
                            }' data-buy-symbol='${buySymbol}' data-symbol-source='${symbolSource}'>-</button>`;

                    var button = formatButton(row.SellSymbol, row.BuySymbol, row.Source, row.Price);

                    return button;
                },
                targets: [0]
            },
            {
                render: function (data, type, row) {
                    if (data) {
                        return parseFloat(data).toFixed(8);
                    }

                    return "";
                },
                targets: [2, 3, 4]
            }
        ],
        
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

    $("#drop-price-table").on("click", ".btn-delete-drop-price", function (event) {
        var target = event.target;

        var sellSymbol = target.getAttribute("data-sell-symbol");
        var buySymbol = target.getAttribute("data-buy-symbol");
        var symbolSource = target.getAttribute("data-symbol-source");

        var data = {
            SellSymbol: sellSymbol,
            BuySymbol: buySymbol,
            SymbolSource: symbolSource
        };

        $.ajax
        ({
            url: "/dropprice",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            type: "delete",
            success: function (result) {
                table.ajax.reload();
            }
        });
    });
});