using Microsoft.Azure.Search.Models;
using System.Collections.Generic;

namespace BizTalkFusion.Solutions.Integration.Models
{
    public class EDIInsights
    {
        public FacetResults Facets { get; set; }
        public IList<SearchResult> Results { get; set; }
        public int? Count { get; set; }
    }   
}