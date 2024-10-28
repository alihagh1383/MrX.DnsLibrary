using System.Net;
using System.Text;
using DNS.Server;

namespace MrX.DnsLibrary;

public class DnsServer
{
    private DNS.Server.DnsServer _server;
    private readonly MasterFile _masterFile;
    private string _fatherDnsServer;
    private int _fatherPort;
    public event EventHandler<DNS.Server.DnsServer.RequestedEventArgs> Requested;
    public event EventHandler<DNS.Server.DnsServer.RespondedEventArgs> Responded;
    public DnsServer(string fatherDnsServer, int fatherPort = 53)
    {
        _fatherDnsServer = fatherDnsServer;
        _fatherPort = fatherPort;
        _masterFile = new MasterFile();
        _server = new DNS.Server.DnsServer(fatherDnsServer, fatherPort);
    }

    public void Start(string ip = null, int port = 53)
    {
        _server = new(_masterFile, _fatherDnsServer, _fatherPort);
        _server.Listening += delegate { Console.WriteLine("Dns Server Start  =>[" + IPAddress.Parse(ip ?? IPAddress.Any.ToString()).ToString() + "]:" + port); };
        _server.Requested += (_, args) => Requested?.Invoke(_, args);
        _server.Responded += (_, args) => Responded?.Invoke(_, args);
        _server.Errored += (_, args) => { Console.WriteLine(args.Exception.Message); };
        if (ip != null)
            _server.Listen(port, IPAddress.Parse(ip));
        else
        {
            _server.Listen(53, IPAddress.Any);
        }
    }

    public void Stop()
    {
        _server.Dispose();
    }

    public void AddA(string domain, string ip)
    {
        _masterFile.AddIPAddressResourceRecord(domain, ip);
    }
    public void AddCNAME(string domain, string cname)
    {
        _masterFile.AddCanonicalNameResourceRecord(domain, cname);
    }
}

/*

  #r "nuget:Dns"
  #r "MrX.DnsLibrary.dll"
    using MrX.DnsLibrary;
 DnsServer s = new("8.8.8.8");
 s.Start();


 */