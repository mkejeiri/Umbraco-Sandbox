//see https://codeshare.co.uk/blog/how-to-search-by-document-type-and-property-in-umbraco/
namespace UmbracoCms.Models
{
    public class SearchGroup
    {
        public string[] FieldsToSearchIn { get; set; }
        public string[] SearchTerms { get; set; }

        public SearchGroup(string[] fieldsToSearchIn, string[] searchTerms)
        {
            FieldsToSearchIn = fieldsToSearchIn;
            SearchTerms = searchTerms;
        }
    }
}