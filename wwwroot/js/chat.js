"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/Hubs/Public").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (message) {
    var li = document.createElement("li");
    document.getElementById("messageList").appendChild(li);
    var time = new Date(message.sentUtc).toLocaleTimeString();
    li.textContent = `${message.userName} at ${time}: ${message.content}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (e) {
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessageAsync", message).catch(function (err) {
        return console.error(err.toString());
    });

    e.preventDefault();
});
