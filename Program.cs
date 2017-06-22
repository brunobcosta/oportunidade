using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;


namespace oportunidade
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //Obter 10 ultimos tópicos
            var crawler = new Minuto.Crawler();
            var postsRecentes = crawler.ObterRecentes(10);

            var tokenizer = new Minuto.Tokenizer();
            tokenizer.AddStopWord("ser");
            tokenizer.AddStopWord("são");
            tokenizer.AddStopWord("está");
            tokenizer.AddStopWord("pode");
            tokenizer.AddStopWord("fazer");
            var todasPalavras = new List<string>();
            //Analizar posts
            foreach (var post in postsRecentes)
            {
                //Separar descrição por palavras
                var palavras = tokenizer.ExtractTokens(post.Descricao,3);
                todasPalavras.AddRange(palavras);


                Console.WriteLine($"{post.Titulo} - {palavras.Count}");
            }

            Console.WriteLine("=======================");
            foreach (var group in todasPalavras.GroupBy(x=>x).OrderByDescending(x=>x.Count()).Take(10))
            {
                Console.WriteLine($"{group.Key} - {group.Count()}");                
            }



        }
    }
}
