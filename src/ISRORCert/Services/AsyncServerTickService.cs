using ISRORCert.Network;

using Microsoft.Extensions.Hosting;


namespace ISRORCert.Services
{
    internal class AsyncServerTickService : BackgroundService
    {
        // This not the most accurate way but it's fine because Certification is not under heavy duety.
        private readonly PeriodicTimer _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(1));

        private readonly AsyncServer _serverInterface;

        public AsyncServerTickService(AsyncServer serverInterface)
        {
            _serverInterface = serverInterface;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
                _serverInterface.Tick();
        }
    }
}
