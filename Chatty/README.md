If you chose to not remove HTTPS redirection you will need to target the HTTPS address because SignalR doesn’t allow redirections. In our case, we will be using the default local address https://localhost:5001.

dotnet dev-certs https --clean    
dotnet dev-certs https --trust
