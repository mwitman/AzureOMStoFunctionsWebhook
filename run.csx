using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

    private const string endpointUrl = "DocDBUrl";
    private const string authorizationKey = "DocDBKey";
    private const string databaseId = "alertsdb";
    private const string collectionId = "alerts";
    private static DocumentClient client;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
  
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req}");

    string jsonstring = await req.Content.ReadAsStringAsync();
    dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstring);
    log.Info($"RequestPayload={jsonstring}");

    client = new DocumentClient(new Uri(endpointUrl), authorizationKey);

    using (client)
    {
         InsertDocument(data).Wait();
    }
    
    return req.CreateResponse(HttpStatusCode.OK, new {
        body = $"New GitHub comment: {data}"
    }); 
}

private static async Task InsertDocument(object json)
        {
            var collectionLink = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
            //Console.WriteLine("\n1.1 - Creating documents");
            Document created = await client.CreateDocumentAsync(collectionLink, json);        
        }
