let connection = new signalR.HubConnectionBuilder()
    .withUrl("/signalServer")
    .withAutomaticReconnect()
    .build();

connection.start().then(function () {
    connection.invoke("ShipperJoinGroup", "Shipper");
}).catch(function (err) {
    return console.error(err);
})