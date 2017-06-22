using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml;
using HtmlAgilityPack;

namespace Minuto
{
    internal class Crawler
    {
        public Crawler()
        {
        }

        internal IEnumerable<dynamic> ObterRecentes(int quantidade = 10)
        {

            var client = new HttpClient();
            var response = client.GetAsync("http://www.minutoseguros.com.br/blog/feed/").Result;
            var xmlString = response.Content.ReadAsStringAsync().Result;

            var xml = new XmlDocument();
            xml.LoadXml(xmlString);

            foreach (XmlElement item in xml.GetElementsByTagName("item"))
            {

                var desc = ObterTexto(item.GetElementsByTagName("link")[0].InnerText);

                yield return new {
                    Titulo=item.GetElementsByTagName("title")[0].InnerText,
                    Descricao=desc
                };
            }
        }

        private string ObterTexto(string postUrl){
            
            var web = new HtmlWeb();
            var doc = web.Load(postUrl);

            var htmlNodeCollection = doc.DocumentNode.SelectNodes("//div[@class='_entry_']/p");
            return string.Join(" ", htmlNodeCollection.Select(x => x.InnerText ?? ""));
        }
    }
}

