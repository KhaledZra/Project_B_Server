using Serilog;

namespace Project_B_Server_Test;

public static class CustomLogger
{ 
    public static Serilog.Core.Logger Log = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
}