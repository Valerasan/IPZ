"use strict";




$(document).ready(() => {

    var connection = new signalR.HubConnectionBuilder().withUrl("/notification").build();

    connection.on("RefreshProducts", function () {
        console.log("Hello world!");
    });

    connection.start();
});