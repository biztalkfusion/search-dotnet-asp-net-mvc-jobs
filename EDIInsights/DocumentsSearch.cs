using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace BizTalkFusion.Solutions.Integration
{
    public class DocumentsSearch
    {
        private static SearchServiceClient _searchClient;
        private static ISearchIndexClient _indexClient;
        private static ISearchIndexClient _errorIndexClient;       
        private static string IndexName = "documentdb-index";
        private static string ErrorIndexName = "edibadmessages-index";
        public static string errorMessage;

        static DocumentsSearch()
        {
            try
            {
                string searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
                string apiKey = ConfigurationManager.AppSettings["SearchServiceApiKey"];

                // Create an HTTP reference to the catalog index
                _searchClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
                _indexClient = _searchClient.Indexes.GetClient(IndexName);
                _errorIndexClient = _searchClient.Indexes.GetClient(ErrorIndexName); 
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
            }
        }

        public DocumentSearchResult Search(string searchText, string MessageTypeFacet, string SendingPartnerTypeFacet, string ReceivePartnerTypeFacet,  string DocDateTypeFacet,
             int currentPage, string folderName)
        {
            try
            {
                SearchParameters sp = new SearchParameters()
                {
                    SearchMode = SearchMode.Any,
                    Top = 10,
                    Skip = currentPage - 1,
                    // Limit results
                    Select = new List<string>() { "id", "SendPartnerName", "ReceivePartnerName", "DocDate", "Path", "MessageType" },
                    // Add count
                    IncludeTotalResultCount = true,
                    Facets = new List<string>() { "SendPartnerName", "ReceivePartnerName", "DocDate", "MessageType", "FolderName", "Path" },
                };

                    // Add filtering
                string filter = null;
                if (MessageTypeFacet != "")
                    filter = "MessageType eq '" + MessageTypeFacet + "'";
                if (SendingPartnerTypeFacet != "")
                {
                    if (filter != null)
                        filter += " and ";
                    filter += "SendPartnerName eq '" + SendingPartnerTypeFacet + "'";

                }

                if (ReceivePartnerTypeFacet != "")
                {
                    if (filter != null)
                        filter += " and ";
                    filter += "ReceivePartnerName eq '" + ReceivePartnerTypeFacet + "'";

                }

                if (DocDateTypeFacet != "")
                {
                    if (filter != null)
                        filter += " and ";
                    filter += "DocDate eq '" + DocDateTypeFacet + "'";
                    
                }
                if (folderName != "")
                {
                    if (folderName != null && !string.IsNullOrEmpty(filter))
                        filter += " and ";
                    filter += "FolderName eq '" + folderName + "'";
                } 
                sp.Filter = filter;
            
                return _indexClient.Documents.Search(searchText, sp);
            }

            catch (Exception ex)
             {
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message.ToString());
            }
            return null;
        }

        public DocumentSearchResult ErrorMessageSearch(int currentPage, string folderName)
        {
            try
            {
                SearchParameters sp = new SearchParameters()
                {
                    SearchMode = SearchMode.Any,
                    Top = 10,
                    Skip = currentPage - 1,
                    // Limit results
                    Select = new List<String>() { "FolderName", "Path","ExceptionMessage", "DocDate" },
                    // Add count
                    IncludeTotalResultCount = true,
                    Facets = new List<String>() { "FolderName" },
                };

                // Add filtering
                string filter = null;
                var searchText = "";
                if (folderName != "")
                {                    
                    filter = "FolderName eq '" + folderName + "'";
                }               

                sp.Filter = filter;

                return _errorIndexClient.Documents.Search(searchText, sp);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message.ToString());
            }
            return null;
        }
    }
}