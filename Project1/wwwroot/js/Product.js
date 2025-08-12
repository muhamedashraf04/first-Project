
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#tblData').DataTable({
        "ajax": { url: '/Admin/Product/getall' },
        "columns": [{ data: 'title', className: "text-start", width: "25%" },
            { data: 'isbn', className: "text-start", width: "15%" },
            { data: 'author', className: "text-start", width: "15%" },
            { data: 'listPrice', className: "text-start", width: "10%" },
            { data: 'category.name', className: "text-start", width: "15%" },
            {
                data: 'id',
                "render": function (data)
                {
                    return `<div class="w-75 btn-group" role="group">
                            <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2" ><i class="bi bi-pencil"></i> Edit</a>
                            <a onclick=Delete('/admin/product/delete/${data}') class="btn btn-danger mx-2"><i class="bi bi-trash3"></i> Delete</a>
                    </div>`
                },
                "width": "20%"
            }

        ]
    });
}

function Delete(url) {
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
                url:url,
                type: 'DELETE',
                success: function (data) {
                    toastr.success(data.message);
                    $('#tblData').DataTable().ajax.reload();}
            })
        }
    });
}


