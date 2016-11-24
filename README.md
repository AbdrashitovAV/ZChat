# ZChat
Simple wpf chat. Client server interaction via signalr.
####Client
After loading one can specify his username, server address and port. Client written using WPF with MVVM patern. Message can be send both to a specific user and to everyone.
####Server
C# server written as a console application. After loading server name, network interface and port can be specified. Server loggs events using log4net.

##Project structure 

#### - WPFStuff
Contains some WPF converters and PropertyChangedImplementation class.
#### - ZChat
WPF Client inner logic. Contains Views and ViewModels for client.
#### - ZChat.Communication
Contains communication logic for client. Currently contains SignalR and test implementation of connection logic
#### - ZChat.Shared
Contains DTO objects for client-server communication
#### - ZChat.Server.SignalR
Console application for starting server via OWIN self host. Contains specification for log4net loggers.
#### - ZChat.Server.Logic
Class library with ChatHub and SignalRServerConnectionManager. ConnectionManager contains actual server logic with message processing and logging.
