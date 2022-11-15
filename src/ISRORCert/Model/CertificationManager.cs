using ISRORCert.Database;
using ISRORCert.Model.Serialization;

using Microsoft.Extensions.Logging;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;

namespace ISRORCert.Model
{
    internal class CertificationManager
    {
        private readonly ILogger _logger;
        private readonly IDbAdapter _adapter;
        private readonly ICertificationSerializer _certificationSerializer;

        private List<Content> _contentList = new();
        private List<Module> _moduleList = new();
        private List<Division> _divisionList = new();
        private List<Farm> _farmList = new();
        private List<FarmContent> _farmContentList = new();
        private List<Shard> _shardList = new();
        private List<ServerMachine> _serverMachineList = new();
        private List<ServerBody> _serverBodyList = new();
        private List<ServerCord> _serverCordList = new();

        private Dictionary<string, Module> _moduleByName = new();

        private Dictionary<byte, Content> _contentByID = new();
        private Dictionary<byte, Module> _moduleByID = new();
        private Dictionary<byte, Division> _divisonByID = new();
        private Dictionary<byte, Farm> _farmByID = new();
        private Dictionary<short, Shard> _shardByID = new();
        private Dictionary<int, ServerMachine> _serverMachineByID = new();
        private Dictionary<short, ServerBody> _serverBodyByID = new();
        private Dictionary<int, ServerCord> _serverCordyByID = new();


        public IReadOnlyList<Content> Content => _contentList;
        public IReadOnlyList<Module> Modules => _moduleList;
        public IReadOnlyList<Division> Divisions => _divisionList;
        public IReadOnlyList<Farm> Farms => _farmList;
        public IReadOnlyList<FarmContent> FarmContent => _farmContentList;
        public IReadOnlyList<Shard> Shards => _shardList;
        public IReadOnlyList<ServerMachine> ServerMachines => _serverMachineList;
        public IReadOnlyList<ServerBody> ServerBodies => _serverBodyList;
        public IReadOnlyList<ServerCord> ServerCords => _serverCordList;

        public ServerBody? Identity { get; private set; }

        public CertificationManager(ILogger<CertificationManager> logger, IDbAdapter adapter, ICertificationSerializer certificationSerializer)
        {
            _logger = logger;
            _adapter = adapter;
            _certificationSerializer = certificationSerializer;
        }

        public async Task<bool> RefreshAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Querying certification data...");

            var results = await Task.WhenAll(
                _adapter.GetDataTableAsync(_contentList, "_GetContentList", cancellationToken),
                _adapter.GetDataTableAsync(_moduleList, "_GetModuleList", cancellationToken),
                _adapter.GetDataTableAsync(_divisionList, "_GetDivisionList", cancellationToken),
                _adapter.GetDataTableAsync(_farmList, "_GetFarmList", cancellationToken),
                _adapter.GetDataTableAsync(_farmContentList, "_GetFarmContentList", cancellationToken),
                _adapter.GetDataTableAsync(_shardList, "_GetShardList", cancellationToken),
                _adapter.GetDataTableAsync(_serverMachineList, "_GetServerMachineList", cancellationToken),
                _adapter.GetDataTableAsync(_serverBodyList, "_GetServerBodyList", cancellationToken),
                _adapter.GetDataTableAsync(_serverCordList, "_GetServerCordList", cancellationToken));

            if(results.Any(p => p == false))
            {
                _logger.LogCritical("Failed to query certification data...");
                return false;
            }

            _contentByID = _contentList.ToDictionary(p => p.Id);
            _moduleByID = _moduleList.ToDictionary(p => p.Id);
            _moduleByName = _moduleList.ToDictionary(p => p.Name);
            _divisonByID = _divisionList.ToDictionary(p => p.Id);
            _farmByID = _farmList.ToDictionary(p => p.Id);
            _shardByID = _shardList.ToDictionary(p => p.ID);
            _serverMachineByID = _serverMachineList.ToDictionary(p => p.Id);
            _serverBodyByID = _serverBodyList.ToDictionary(p => p.ID);
            _serverCordyByID = _serverCordList.ToDictionary(p => p.ID);

            Link();

            if (!_moduleByName.TryGetValue("Certification", out var certificationModule))
            {
                _logger.LogCritical("Failed to find Certification module");
                return false;
            }

            var certificationBody = _serverBodyList.SingleOrDefault(p => p.ModuleID == certificationModule.Id);
            if (certificationBody == null)
            {
                _logger.LogCritical("Failed to find Certification serverBody");
                return false;
            }

            Identity = certificationBody;
            Identity.State = ServerBodyState.ServiceRunning;
            _logger.LogInformation("Certification successfully refreshed");

            Print(0, Identity);
            return true;
        }

        private void Print(int indent, ServerBody body)
        {
            var prefix = new string(' ', indent);
            Console.WriteLine($"{prefix}{body}");
            foreach (var item in _serverBodyList.Where(p => p.CertifierID == body.ID))
                Print(indent + 3, item);
        }

        private void Link()
        {
            foreach (var item in _serverBodyList)
            {
                // Division
                Division? division = null;
                if (item.DivisionID.HasValue && !_divisonByID.TryGetValue(item.DivisionID ?? 0, out division))
                    _logger.LogError($"Cannot find {nameof(Division)}#{item.DivisionID} for {nameof(ServerBody)}#{item.ID}");

                item.Division = division;

                // Farm
                Farm? farm = null;
                if (item.FarmID.HasValue && !_farmByID.TryGetValue(item.FarmID.Value, out farm))
                    _logger.LogError($"Cannot find {nameof(Farm)}#{item.FarmID} for {nameof(ServerBody)}#{item.ID}");

                item.Farm = farm;

                // Shard
                Shard? shard = null;
                if (item.ShardID.HasValue && !_shardByID.TryGetValue(item.ShardID.Value, out shard))
                    _logger.LogError($"Cannot find {nameof(Shard)}#{item.ShardID} for {nameof(ServerBody)}#{item.ID}");

                item.Shard = shard;

                // Shard
                ServerBody? certifier = null;
                if (item.CertifierID.HasValue && !_serverBodyByID.TryGetValue(item.CertifierID.Value, out certifier))
                    _logger.LogError($"Cannot find Certifier#{item.CertifierID} for {nameof(ServerBody)}#{item.ID}");

                item.Certifier = certifier;

                // ServerMachine
                if (!_serverMachineByID.TryGetValue(item.MachineID, out var machine))
                    _logger.LogError($"Cannot find {nameof(ServerMachine)}#{item.MachineID} for {nameof(ServerBody)}#{item.ID}");

                item.Machine = machine;

                // Module
                if (!_moduleByID.TryGetValue(item.ModuleID, out var module))
                    _logger.LogError($"Cannot find {nameof(Module)}#{item.ModuleID} for {nameof(ServerBody)}#{item.ID}");

                item.Module = module;
            }

            foreach (var item in _serverCordList)
            {
                // Outlet
                if (!_serverBodyByID.TryGetValue(item.OutletID, out var outlet))
                    _logger.LogError($"Cannot find Outlet#{item.OutletID} for {nameof(ServerCord)}#{item.ID}");

                item.Outlet = outlet;

                // Inlet
                if (!_serverBodyByID.TryGetValue(item.InletID, out var inlet))
                    _logger.LogError($"Cannot find Inlet#{item.InletID} for {nameof(ServerCord)}#{item.ID}");

                item.Inlet = inlet;
            }

            foreach (var item in _farmList)
            {
                // Division
                Division? division = null;
                if (!_divisonByID.TryGetValue(item.DivisionID, out division))
                    _logger.LogError($"Cannot find {nameof(Division)}#{item.DivisionID} for {nameof(Farm)}#{item.Id}");

                item.Division = division;
            }

            foreach (var item in _farmContentList)
            {
                // Farm
                if (!_farmByID.TryGetValue(item.FarmID, out var farm))
                    _logger.LogError($"Cannot find {nameof(Farm)}#{item.FarmID} for {nameof(FarmContent)}#{item.ID}");

                item.Farm = farm;

                // Content
                if (!_contentByID.TryGetValue(item.ContentID, out var content))
                    _logger.LogError($"Cannot find {nameof(Content)}#{item.ContentID} for {nameof(FarmContent)}#{item.ID}");

                item.Content = content;
            }

            foreach (var item in _shardList)
            {
                // Farm
                if (!_farmByID.TryGetValue(item.FarmID, out var farm))
                    _logger.LogError($"Cannot find {nameof(Farm)}#{item.FarmID} for {nameof(Shard)}#{item.ID}");

                item.Farm = farm;

                // Content
                if (!_contentByID.TryGetValue(item.ContentID, out var content))
                    _logger.LogError($"Cannot find {nameof(Content)}#{item.ContentID} for {nameof(Shard)}#{item.ID}");

                item.Content = content;
            }

            foreach (var item in _serverMachineList)
            {
                // Division
                Division? division = null;
                if (item.DivisionID.HasValue && !_divisonByID.TryGetValue(item.DivisionID ?? 0, out division))
                    _logger.LogError($"Cannot find {nameof(Division)}#{item.DivisionID} for {nameof(ServerMachine)}#{item.Id}");

                item.Division = division;

                if (!IPAddress.TryParse(item.PublicIP, out var publicAddress))
                {
                    if(_logger.IsEnabled(LogLevel.Error))
                        _logger.LogError($"Failed to parse {nameof(item.PublicIP)} for {nameof(ServerMachine)}#{item.Id}.");
                }
                item.PublicIPAddress = publicAddress;

                if (!IPAddress.TryParse(item.PrivateIP, out var privateAddress))
                {
                    if(_logger.IsEnabled(LogLevel.Error))
                        _logger.LogError($"Failed to parse {nameof(item.PrivateIP)} for {nameof(ServerMachine)}#{item.Id}.");
                }
                item.PrivateIPAddress = privateAddress;
            }

            if (!TryGetModule("GlobalManager", out var globalModule))
                _logger.LogCritical("Cannot find GlobalManager module");

            if (!TryGetModule("MachineManager", out var machineModule))
                _logger.LogCritical("Cannot find MachineManager module");

            if (!TryGetModule("FarmManager", out var farmModule))
                _logger.LogCritical("Cannot find FarmManager module");

            if (!TryGetModule("SR_ShardManager", out var shardModule))
                _logger.LogCritical("Cannot find ShardManager module");

            foreach (var item in _serverBodyList)
            {
                if (item.Module == globalModule)
                    item.Division!.ManagerBodyID = item.ID;

                if (item.Module == machineModule)
                    item.Machine!.ManagerBodyID = item.ID;

                if (item.Module == farmModule)
                    item.Farm!.ManagerBodyID = item.ID;

                if (item.Module == shardModule)
                    item.Shard!.ManageBodyID = item.ID;
            }
        }
        public bool TryGetCertifiableServerBody(string moduleName, string moduleAddress, ushort modulePort, [MaybeNullWhen(false)] out ServerBody serverBody)
        {
            serverBody = null;

            if (Identity is null)
                return false;

            if (!TryGetModule(moduleName, out var module))
                return false;

            foreach (var item in _serverBodyList)
            {
                if (item == Identity)
                    continue; // don't certify ourself

                if (item.CertifierID != Identity.ID)
                    continue; // we're not the certifier

                if (item.ModuleID != module.Id)
                    continue; // wrong module

                var machine = _serverMachineByID[item.MachineID];
                if (moduleAddress != machine.PublicIP && moduleAddress != machine.PrivateIP)
                    continue;

                if (modulePort != 0 && modulePort != item.ListenerPort)
                    continue;

                serverBody = item;
                return true;
            }
            return false;
        }

        public bool TryGetModule(string moduleName, [MaybeNullWhen(false)] out Module module) => _moduleByName.TryGetValue(moduleName, out module);
        public bool TryGetServerBody(short id, [MaybeNullWhen(false)] out ServerBody serverBody) => _serverBodyByID.TryGetValue(id, out serverBody);
        public bool TryGetServerCord(int id, [MaybeNullWhen(false)] out ServerCord serverCord) => _serverCordyByID.TryGetValue(id, out serverCord);
        public bool TryGetShard(short id, [MaybeNullWhen(false)] out Shard shard) => _shardByID.TryGetValue(id, out shard);

        public bool UpdateShardName(Shard shard, string newName)
        {
            if (shard.Name == newName)
                return false;

            // TODO: Async
            _adapter.Execute("_UpdateShardName",
                _adapter.GetInputParameter("@nID", shard.ID),
                _adapter.GetInputParameter("@szName", newName));

            // TODO: Check result

            _logger.LogInformation($"Changed {nameof(Shard)}#{shard} name: {shard.Name} -> {newName}");
            shard.Name = newName;
            return true;
        }
        public bool UpdateShardMaxUser(Shard shard, short newMaxUser)
        {
            if (shard.MaxUser == newMaxUser)
                return false;

            // TODO: Async
            _adapter.Execute("_UpdateShardMaxUser",
                _adapter.GetInputParameter("@nID", shard.ID),
                _adapter.GetInputParameter("@nMaxUser", newMaxUser));

            // TODO: Check result

            _logger.LogInformation($"Changed {nameof(Shard)}#{shard} max user: {shard.MaxUser} -> {newMaxUser}");
            shard.MaxUser = newMaxUser;
            return true;
        }

    }
}
