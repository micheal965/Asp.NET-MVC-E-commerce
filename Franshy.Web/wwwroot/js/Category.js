var datatable;
$(document).ready(function () {
    loaddata();
});
function loaddata() {
    datatable = $('#categories').DataTable({
        "ajax": {
            "url": "/Admin/Category/GetData",
        },
        "columns": [
            { "data": "name" },
            {
                "data": "actions",
                "render": function (data, type, row) {
                    return data;
                }
            }
        ]
    });
}

function DeleteItem(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.success) {
                        datatable.ajax.reload();
                        Swal.fire({
                            title: data.message,
                            icon: "success"
                        });
                    } else {
                        Swal.fire({
                            title: data.message,
                            icon: "error"
                        });
                    }
                }
            });

        }
    });
}