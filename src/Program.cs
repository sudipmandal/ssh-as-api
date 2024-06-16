
using Microsoft.AspNetCore.Mvc;
using Renci.SshNet;

var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

app.MapPost("/cmd", async (Payload p) =>
{
    string result = string.Empty;
    try
    {
        string? host = Environment.GetEnvironmentVariable("host");
        string? user = Environment.GetEnvironmentVariable("sshuser");
        string? pwd = Environment.GetEnvironmentVariable("sshpwd");
        using (var client = new SshClient(host, user, pwd))
        {
            await client.ConnectAsync(new CancellationToken());

            using (SshCommand command = client.CreateCommand(p.cmd))
            {
                command.CommandTimeout = new TimeSpan(0,0,15);
                result = command.Execute();

                return Results.Ok(result);
            }
        }
    }
    catch (Exception ex)
    {
        result = ex.Message;
        return Results.Problem(result);
    }


});


app.Run();
record Payload(string cmd);   
