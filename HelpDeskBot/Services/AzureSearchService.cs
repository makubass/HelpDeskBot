namespace HelpDeskBot.Services
{
    using Model;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Configuration;

    [Serializable]
    public class AzureSearchService
    {
        private readonly string QueryString = $"https://{WebConfigurationManager.AppSettings["AzureSearchAccount"]}.search.windows.net/indexes/{WebConfigurationManager.AppSettings["AzureSearchIndex"]}/docs?api-key={WebConfigurationManager.AppSettings["AzureSearchKey"]}&api-version=2016-09-01&";

        public async Task<SearchResult> SearchByCategory(string category)
        {
            using (var httpClient = new HttpClient())
            {
                string nameQuery
                    = $"{QueryString}$filter=category eq '{category}'";
                string response
                    = await httpClient.GetStringAsync(nameQuery);
                return JsonConvert.DeserializeObject<SearchResult>(response);
            }
        }
    }
}