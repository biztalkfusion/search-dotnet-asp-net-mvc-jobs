using BingGeocoder;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;
using NYCJobsWeb.Attributes;
using NYCJobsWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NYCJobsWeb.Controllers
{
    [SearchAuthorize]
    public class HomeController : Controller
    {
        public const string PrefixUrl = "https://kuebixedi.blob.core.windows.net/incomingedi/EDIData/";
        private JobsSearch _jobsSearch = new JobsSearch();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult JobDetails()
        {
            return View();
        }

        public ActionResult Search(string q = "", string MessageTypeFacet = "", string SendingPartnerTypeFacet = "", string ReceivePartnerTypeFacet = "", string DocDateTypeFacet = "",
            int currentPage = 0)
        {
            // If blank search, assume they want to search everything
            if (string.IsNullOrWhiteSpace(q))
                q = "*";

            string maxDistanceLat = string.Empty;
            string maxDistanceLon = string.Empty;
            if (SendingPartnerTypeFacet == "0")
                SendingPartnerTypeFacet = "";
            if (ReceivePartnerTypeFacet == "0")
                ReceivePartnerTypeFacet = "";
            if (DocDateTypeFacet == "0")
                DocDateTypeFacet = "";



            var response = _jobsSearch.Search(q, MessageTypeFacet, SendingPartnerTypeFacet, ReceivePartnerTypeFacet, DocDateTypeFacet,currentPage);
            if(response==null)
                return RedirectToAction("Index", "Home");
            return new JsonResult
            {
                // ***************************************************************************************************************************
                // If you get an error here, make sure to check that you updated the SearchServiceName and SearchServiceApiKey in Web.config
                // ***************************************************************************************************************************

                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new NYCJob() { Results = response.Results, Facets = response.Facets, Count = Convert.ToInt32(response.Count) }
            };
        }

        [HttpGet]
        public ActionResult Suggest(string term, bool fuzzy = true)
        {
            // Call suggest query and return results
            var response = _jobsSearch.Suggest(term, fuzzy);
            List<string> suggestions = new List<string>();
            foreach (var result in response.Results)
            {
                suggestions.Add(result.Text);
            }

            // Get unique items
            List<string> uniqueItems = suggestions.Distinct().ToList();

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = uniqueItems
            };

        }

        public ActionResult LookUp(string id)
        {
            // Take a key ID and do a lookup to get the job details
            if (id != null)
            {
                var response = _jobsSearch.LookUp(id);
                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new NYCJobLookup() { Result = response }
                };
            }
            else
            {
                return null;
            }

        }

        public JsonResult GetBlobContent(string fileName, string folderName)
        {
            var jsonResult = "";
            var fullfilter = PrefixUrl + folderName + "/Inbound/" + fileName;
            var searchString = "metadata_storage_path:\"" + fullfilter + "\"";
            ISearchIndexClient docdbindexClient = CreateSearchIndexClient();
            var parameters = new SearchParameters()
            {
                Select = new[] { "content" },
                SearchMode = SearchMode.All,
                QueryType = QueryType.Full
            };
            var documentDBResult = docdbindexClient.Documents.Search(searchString, parameters);
            if (documentDBResult.Results != null && documentDBResult.Results.Count > 0)
            {
                jsonResult = JsonConvert.SerializeObject(documentDBResult.Results.FirstOrDefault().Document.FirstOrDefault().Value);
            }
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        private static SearchIndexClient CreateSearchIndexClient()
        {
            string searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
            string queryApiKey = ConfigurationManager.AppSettings["SearchServiceblobApiKey"];            
            SearchIndexClient indexClient = new SearchIndexClient(searchServiceName, "azureblob-index", new SearchCredentials(queryApiKey));
            return indexClient;
        }
    }
}
