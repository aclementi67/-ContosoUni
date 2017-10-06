$(document).ready(function () {  
        $.ajax({  
            type: "GET",  
            url: "/Course/IndexJson",  
            contentType: "application/json; charset=utf-8",  
            dataType: "json",  
            success: function (data) {  
                var tableRef = document.getElementById('myTable').getElementsByTagName('tbody')[0];
                $.each(data, function (i, item) {  
                    // Insert a row in the table at the last row
                    var newRow = tableRef.insertRow(tableRef.rows.length);

                    newRow.insertCell(0).appendChild(document.createTextNode(item.Title));
                    newRow.insertCell(1).appendChild(document.createTextNode(item.Credits));
                }); //End of foreach Loop   
                console.log(data);  
            }, //End of AJAX Success function  
  
            failure: function (data) {  
                alert(data.responseText);  
            }, //End of AJAX failure function  
            error: function (data) {  
                alert(data.responseText);  
            } //End of AJAX error function  
  
        });         
    });  
