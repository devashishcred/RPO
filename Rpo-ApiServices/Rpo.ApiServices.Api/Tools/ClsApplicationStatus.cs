using SODA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Tools
{
    public class ClsApplicationStatus
    {
        public void GetApplicationStatus()
        {
            var client = new SodaClient("https://data.cityofnewyork.us/", "7tPQriTSRiloiKdWIJoODgReN");

            // Get a reference to the resource itself
            // The result (a Resouce object) is a generic type
            // The type parameter represents the underlying rows of the resource
            // and can be any JSON-serializable class
            var dataset = client.GetResource<ResourceMetadata>("rvhx-8trz");

            // Resource objects read their own data
            var rows = dataset.GetRows(limit: 5000);

            Console.WriteLine("Got {0} results. Dumping first results:", rows.Count());

            //foreach (var keyValue in rows.First())
            //{
            //    Console.WriteLine(keyValue);
            //}
        }
    }
}