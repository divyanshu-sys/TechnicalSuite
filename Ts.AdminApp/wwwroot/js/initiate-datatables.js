// Initiate datatables in roles, tables, users page
(function () {
    'use strict';
})();

function initAjaxTable(tableId, sorting, targets, url, columns, dynamicDataFunc, deferLoading = false) {
    return $(tableId).DataTable({
        pageLength: 50,
        dom: 'Bfrtip',
        buttons: [
            'pageLength',
            {
                extend: 'copy',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'csv',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'excel',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'pdf',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: ':visible'
                }
            },
            'colvis'
        ],
        scrollX: true,
        scrollY: "350",
        processing: true,
        columnDefs: [{
            orderable: false,
            targets: targets,
        }],
        sorting: sorting,
        serverSide: true,
        deferLoading: deferLoading ? 0 : null,
        ajax: {
            url: url,
            type: 'post',
            data: function (d) {
                if (typeof dynamicDataFunc === 'function') {
                    $.extend(d, dynamicDataFunc());
                } else if (typeof dynamicDataFunc === 'object') {
                    $.extend(d, dynamicDataFunc);
                }
            },
            error: function (xhr, error, thrown) {
                handleErrorApiResponse(xhr);
            }
        },
        columns: columns,
        language: {
            zeroRecords: "Sorry - no records found",
            info: "_START_ - _END_ of _TOTAL_ records",
            infoEmpty: "No records available"
        }
    });
}

function initTable(tableId, sorting, targets) {
    return $(tableId).DataTable({
        pageLength: 50,
        dom: 'Bfrtip',
        buttons: [
            'pageLength',
            {
                extend: 'copy',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'csv',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'excel',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'pdf',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: ':visible'
                }
            },
            'colvis'
        ],
        scrollX: true,
        scrollY: "350",
        processing: true,
        columnDefs: [{
            orderable: false,
            targets: targets,
        }],
        sorting: sorting,
        language: {
            zeroRecords: "Sorry - no records found",
            info: "_START_ - _END_ of _TOTAL_ records",
            infoEmpty: "No records available"
        }
    });
}