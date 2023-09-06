<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>DataTables Example</title>

    <!-- Include jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <!-- Include DataTables CSS and JavaScript -->
    <link rel="stylesheet" href="https://cdn.datatables.net/1.11.5/css/jquery.dataTables.min.css">
    <script src="https://cdn.datatables.net/1.11.5/js/jquery.dataTables.min.js"></script>
</head>
<body>
    <table id="example" class="display" style="width:100%">
        <thead>
            <tr>
                <th>Name</th>
                <th>Email</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>John Doe</td>
                <td>john@example.com</td>
                <td></td> <!-- This column will hold the button -->
            </tr>
            <!-- Add more rows here -->
        </tbody>
    </table>

    <script>
        $(document).ready(function() {
            var table = $('#example').DataTable({
                "columns": [
                    { "data": "Name" },
                    { "data": "Email" },
                    {
                        "data": null,
                        "defaultContent": "<button>Delete</button>",
                    }
                ]
            });

            // Add a click event handler for the buttons
            $('#example tbody').on('click', 'button', function () {
                var data = table.row($(this).parents('tr')).data();
                // Handle the button click (e.g., perform a delete operation)
                alert('Delete clicked for ' + data.Name);
            });
        });
    </script>
</body>
</html>
