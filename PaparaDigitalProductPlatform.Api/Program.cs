﻿using PaparaDigitalProductPlatform.Persistance;

namespace PaparaDigitalProductPlatform;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        DbInitializer.Initialize(host);
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}