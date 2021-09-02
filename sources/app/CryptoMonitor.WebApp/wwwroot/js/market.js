$(document).ready(function () {
    var table = $("#market-table").DataTable({
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
            { title: "", data: null, orderable: false, width: "5%" },
            { title: "Symbol", data: "SellSymbol", name: "SellSymbol", orderable: true, width: "15%" },
            { title: "Price", data: "Price", name: "Price", orderable: true, width: "30%" },
            { title: "OpenPrice", data: "OpenPrice", name: "OpenPrice", orderable: false, width: "25%" },
            { title: "Change", data: "Change", name: "Change", orderable: true, width: "25%" }
        ],

        order: [[1, "asc"]],

        columnDefs: [
            {
                render: function (data, type, row) {
                    var formatButton =
                        (buttonStyle, buttonAction, buttonContent, sellSymbol, buySymbol, symbolSource, currentPrice) =>
                            `<button type='button' data-bs-toggle="modal" data-bs-target="#add-drop-price-modal" class='btn ${buttonStyle} btn-sm btn-${buttonAction}-drop-price' data-sell-symbol='${sellSymbol
                            }' data-buy-symbol='${buySymbol}' data-symbol-source='${symbolSource}' data-current-price='${currentPrice}'>${buttonContent
                            }</button>`;

                    var button = formatButton("btn-outline-success", "add", "+", row.SellSymbol, row.BuySymbol, row.Source, row.Price);

                    return button;
                },
                targets: [0]
            },
            {
                render: function (data, type, row) {
                    if (data) {
                        return parseFloat(data).toFixed(10);
                    }

                    return "";
                },
                targets: [2, 3]
            },
            {
                render: function (data, type, row) {
                    if (data) {
                        var parsed = parseFloat(data).toFixed(3);

                        if (parsed > 0) {
                            return `+${parsed}%`;
                        }

                        return `${parsed}%`;
                    }

                    return "";
                },
                targets: [4]
            }
        ],

        rowCallback: function (row, data, index) {
            if (data.Change > 0) {
                $(row).addClass("plus");
            }
            else if (data.Change < 0) {
                $(row).addClass("minus");
            }
        },

        ajax: {
            url: "market/data",
            type: "GET",
            contentType: "application/json",
            data: function (model) {
                var formData = {};

                var source = $("input[type='radio'][name='btnradio_symbolSource']:checked").val();
                var symbol = $("input[type='radio'][name='btnradio_symbol']:checked").val();

                formData.SymbolSource = source;
                formData.BuySymbol = symbol;
                formData.Query = $("#search").val();

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

    const addDropPriceModal = document.getElementById("add-drop-price-modal");
    addDropPriceModal.addEventListener("show.bs.modal",
        function (event) {
            var target = event.relatedTarget;

            var sellSymbol = target.getAttribute("data-sell-symbol");
            var buySymbol = target.getAttribute("data-buy-symbol");
            var symbolSource = target.getAttribute("data-symbol-source");
            var currentPrice = target.getAttribute("data-current-price");
            var symbolSourceDisplay = null;

            switch (symbolSource) {
                case "1":
                    symbolSourceDisplay = "Huobi";
                    break;
                case "2":
                    symbolSourceDisplay = "Binance";
                    break;
            }

            var sellSymbolInput = addDropPriceModal.querySelector("#sellSymbol");
            var buySymbolInput = addDropPriceModal.querySelector("#buySymbol");
            var symbolSourceDisplayInput = addDropPriceModal.querySelector("#symbolSourceDisplay");
            var symbolSourceInput = addDropPriceModal.querySelector("#symbolSource");
            var currentPriceInput = addDropPriceModal.querySelector("#currentPrice");
            var dropPriceInput = addDropPriceModal.querySelector("#dropPrice");

            sellSymbolInput.value = sellSymbol;
            buySymbolInput.value = buySymbol;
            symbolSourceInput.value = symbolSource;
            symbolSourceDisplayInput.value = symbolSourceDisplay;
            currentPriceInput.value = parseFloat(currentPrice).toFixed(10);

            dropPriceInput.value = "";
        });

    $("#add-drop-price-modal-form").on("submit", function (e) {
        e.preventDefault();

        var data = {
            SellSymbol: $("#add-drop-price-modal-form #sellSymbol").val(),
            BuySymbol: $("#add-drop-price-modal-form #buySymbol").val(),
            SymbolSource: $("#add-drop-price-modal-form #symbolSource").val(),
            price: parseFloat($("#add-drop-price-modal-form #dropPrice").val())
        };

        $.ajax({
            url: "/dropprice",
            type: "PUT",
            contentType: "application/json",
            data: JSON.stringify(data),
            success: function () {
                bootstrap.Modal.getInstance(addDropPriceModal).hide();
            }
        });
    });
});