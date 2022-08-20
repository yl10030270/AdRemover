using HtmlAgilityPack;

namespace AdRemover.Core
{
    public class AdRemover
    {
        public void RemoveFromWebPage(string uri)
        {
            var web = new HtmlWeb();
            var doc = web.Load(uri);
            // foreach doc.url node
                // if blocklist.exist
                    // remove/replace
        }

    }
}
