
var connection = new signalR.HubConnectionBuilder().withUrl("/notification").build();

$(document).ready(function () {

    connection.start();
});