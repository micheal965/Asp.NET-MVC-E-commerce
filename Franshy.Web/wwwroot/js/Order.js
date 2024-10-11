var datatable;
$(document).ready(function () {
    loaddata();
});
function loaddata() {
    datatable = $('#Orders').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetData",
        },
        "columns": [
            { "data": "name" },
            { "data": "address" },
            { "data": "phone" },
            { "data": "city" },
            { "data": "email" },
            { "data": "orderstatus" },
            { "data": "totalprice" },
            {
                "data": "actions",
                "render": function (data, type, row) {
                    return data; // This will render the HTML in the column
                }
            }
        ]
    });
}