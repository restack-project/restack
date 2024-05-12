using AutoMapper;
using Microsoft.Extensions.Options;
using ReStack.Common.Interfaces.Aggregates;
using ReStack.Common.Models;
using ReStack.Domain.Settings;
using System.Net;

namespace ReStack.Application.Aggregates;

// TODO abstract to a seperate service
public class SshKeyAggregate(
    IOptions<ApiSettings> _options
) : ISshKeyAggregate
{
    private readonly ApiSettings _settings = _options.Value;

    public Task<SshKeyModel> Generate()
    {
        var keyLocation = Path.Combine(_settings.KeysStorage, _settings.SshKey_Default);
        var pubKeyLocation = Path.Combine(_settings.KeysStorage, $"{_settings.SshKey_Default}.pub");

        if (File.Exists(keyLocation))
            File.Delete(keyLocation);
        else
            Directory.CreateDirectory(Path.GetDirectoryName(keyLocation));

        if (File.Exists(pubKeyLocation))
            File.Delete(pubKeyLocation);
        else
            Directory.CreateDirectory(Path.GetDirectoryName(pubKeyLocation));

        var keygen = new SshKeyGenerator.SshKeyGenerator(2048);
        var privateKey = keygen.ToPrivateKey();
        var publicSshKeyWithComment = keygen.ToRfcPublicKey($"{Environment.UserName}@{Dns.GetHostName()}");

        File.WriteAllText(keyLocation, privateKey);
        File.WriteAllText(pubKeyLocation, publicSshKeyWithComment);

        if (!OperatingSystem.IsWindows())
        {
            File.SetUnixFileMode(keyLocation, UnixFileMode.UserRead | UnixFileMode.UserWrite);
            File.SetUnixFileMode(pubKeyLocation, UnixFileMode.UserRead | UnixFileMode.UserWrite);
        }

        return Get();
    }

    public Task<SshKeyModel> Get(CancellationToken token = default)
    {
        var keyLocation = Path.Combine(_settings.KeysStorage, $"{_settings.SshKey_Default}.pub");
        if (!File.Exists(keyLocation))
            return Task.FromResult<SshKeyModel>(null);

        var keyInfo = new FileInfo(keyLocation);
        var key = File.ReadAllText(keyLocation);
        var model = new SshKeyModel() { Key = key, Generated = keyInfo.CreationTimeUtc };

        return Task.FromResult(model);
    }
}
