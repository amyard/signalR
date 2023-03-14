"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dashboardHub").build();

$(function () {
    connection.start().then(function () {
        alert("connected to dashboard");

        InvokeProduct();
    }).catch(function (err) {
        return console.error(err.toString())
    })
})

connection.on("ReceivedProducts", function (products) {
    BindProductWithGrid(products);
})

function InvokeProduct()
{
    // "SendProducts" - method name from hub
    connection.invoke("SendProducts").catch(function(err) {
        return console.error(err.toString());
    })
}

function BindProductWithGrid(products) {
    $("#tblProduct tbody").empty();
    
    $.each(products, function (index, product) {
        var tr = $("<tr />");
        tr.append(`<td>${(index+1)}</td>`)
        tr.append(`<td>${product.name}</td>`)
        tr.append(`<td>${product.category}</td>`)
        tr.append(`<td>${product.price}</td>`)
        $("#tblProduct").append(tr);
    })
}