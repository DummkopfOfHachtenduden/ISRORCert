using ISRORCert;
using ISRORCert.Database;
using ISRORCert.Logic;
using ISRORCert.Logic.Handler;
using ISRORCert.Model;
using ISRORCert.Model.Serialization;
using ISRORCert.Network;
using ISRORCert.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = new HostBuilder()
   .ConfigureAppConfiguration((hostingContext, config) =>
   {
       config.AddJsonFile("appsettings.json", optional: true);
       config.AddEnvironmentVariables();

       if (args != null)
       {
           config.AddCommandLine(args);
       }
   })
   .ConfigureServices((hostContext, services) =>
   {
       services.AddOptions();
       services.Configure<CertificationConfig>(hostContext.Configuration.GetSection("CertificationConfig"));

       services.AddSingleton<IDbAdapter, SqlDbAdapter>();

       services.AddSingleton<ICertificationSerializer, CertificationSerializerOld>(); // VSRO188
       services.AddSingleton<ICertificationSerializer, CertificationSerializerNew>(); // ISROR2015+

       services.AddSingleton<AsyncServer>();
       services.AddSingleton<IAsyncInterface, CertificationInterface>();
       services.AddSingleton<CertificationManager>();

       services.AddSingleton<PacketHandlerManager>();
       services.AddSingleton<IPacketHandler, PacketHandlerSetupCord>();
       services.AddSingleton<IPacketHandler, PacketHandlerCertificate>();
       services.AddSingleton<IPacketHandler, PacketHandlerNotify>();
       services.AddSingleton<IPacketHandler, PacketHandlerRelay>();
       services.AddSingleton<IPacketHandler, PacketHandlerChangeShardData>();

       services.AddHostedService<CertificationService>();
       services.AddHostedService<AsyncServerTickService>();
   })
   .ConfigureLogging((hostingContext, logging) =>
   {
       logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
       logging.AddConsole();
   });

await builder.RunConsoleAsync();


