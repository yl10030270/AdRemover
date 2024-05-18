using System.Net.Sockets;
using System.Text;
using HtmlAgilityPack;

namespace AdRemover.Core
{
    public interface IUrlCleaner
    {
        string? RemoveFromWebPage(Uri uri);
    }

    public class UrlCleaner : IUrlCleaner
    {
        private readonly HashSet<string> _blockList;

        public UrlCleaner(HashSet<string> blockList)
        {
            _blockList = blockList;
        }

        public string? RemoveFromWebPage(Uri uri)
        {
            var web     = new HtmlWeb
            {
                OverrideEncoding = Encoding.UTF8
            };
            var doc     = web.Load(uri);
            var scriptsNodes = doc.DocumentNode.SelectNodes("//script");
            foreach (var scriptNode in scriptsNodes.Where(node => node.Attributes["src"] is not null))
            {
                var src = scriptNode.Attributes["src"].Value;
                if (src.StartsWith("//"))
                {
                    src = uri.Scheme + ":" + src;
                }
                if (!Uri.TryCreate(src, UriKind.RelativeOrAbsolute, out var result)) continue;

                if (result.IsAbsoluteUri)
                {
                    if (_blockList.Contains(result.Host))
                        scriptNode.Remove();
                }
                //else
                //    scriptNode.Attributes["src"].Value = new Uri(new Uri(uri.GetLeftPart(UriPartial.Authority)), result).ToString();
            }

            var linkNodes = doc.DocumentNode.SelectNodes("//link");
            foreach (var linkNode in linkNodes)
            {
                var href = linkNode.Attributes["href"]?.Value;
                if(!Uri.TryCreate(href, UriKind.RelativeOrAbsolute, out var result)) continue;
                if (result.IsAbsoluteUri)
                {
                    if(_blockList.Contains(result.Host))
                        linkNode.Remove();
                }
                //else
                //    linkNode.Attributes["href"].Value = new Uri(new Uri(uri.AbsoluteUri.Replace(uri.AbsolutePath, string.Empty)), result).ToString();

            }

            AddBaseUrl(doc, uri.GetLeftPart(UriPartial.Authority));

            return doc.DocumentNode.OuterHtml;
        }

        public static void AddBaseUrl(HtmlDocument doc, string baseUrl)
        {
            HtmlNode headNode = doc.DocumentNode.SelectSingleNode("//head");
            if (headNode != null)
            {
                HtmlNode baseNode = HtmlNode.CreateNode($"<base href='{baseUrl}'>");
                headNode.PrependChild(baseNode);
            }
        }

    }
}
