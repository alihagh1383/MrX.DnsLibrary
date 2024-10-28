using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DNS;
using DNS.Client;
using DNS.Protocol.ResourceRecords;
using DNS.Protocol;
namespace MrX.DnsLibrary
{
    public class DNSClient
    {
        ClientRequest request;
        public DNSClient(string dnsserver)
        {
            request = new ClientRequest(dnsserver);

            // Bind to a Domain Name Server
            // Create request bound to 8.8.8.8
        }
        public IList<IPAddress> Lookup(string domain)
        {
            request.Questions.Add(new Question(Domain.FromString(domain)));
            request.RecursionDesired = true;
            IResponse response = request.Resolve().Result;
            // Get all the IPs for the foo.com domain
            return response.AnswerRecords
                  // .Where(r => r.Type == RecordType.AAAA)
                  .Cast<IPAddressResourceRecord>()
                  .Select(r => r.IPAddress)
                  .ToList();
        }
      

    }
}
