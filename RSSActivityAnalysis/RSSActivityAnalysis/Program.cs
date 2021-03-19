using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;

namespace RSSActivityAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            var companies = new Dictionary<string, string>
             {
                {"The Apology Line"                                   ,"https://rss.art19.com/apology-line"},
                {"The Daily by The New York Times"                    ,"http://rss.art19.com/the-daily"},
                {"The Bible in a Year"                                ,"https://feeds.fireside.fm/bibleinayear/rss"},
                {"Crime Junkie"                                       ,"https://feeds.megaphone.fm/ADL9840290619"},
                {"The Experiment"                                     ,"http://feeds.wnyc.org/experiment_podcast"},
                {"The Dan Bongino Show"                               ,"https://feeds.megaphone.fm/WWO3519750118"},
                {"Unrivaled: Long Island Serial Killer"               ,"https://rss.acast.com/unraveled"},
                {"Morbid: A True Crime Podcast"                       ,"https://audioboom.com/channels/4997220.rss"},
                {"Dateline NBC"                                       ,"https://podcastfeeds.nbcnews.com/dateline-nbc"},
                {"The Lincoln Project"                                ,"https://lincolnproject.libsyn.com/rss"},
                {"CNN"                                                ,"http://rss.cnn.com/rss/cnn_topstories.rss"},
                {"New York Times "                                    ,"https://archive.nytimes.com/www.nytimes.com/services/xml/rss/"},
                {"Huffington Post"                                    ,"https://www.huffpost.com/section/front-page/feed?x=1"},
                {"Fox News"                                           ,"http://www.foxnews.com/about/rss/"},
                {"USA Today"                                          ,"http://rssfeeds.usatoday.com/UsatodaycomNation-TopStories"},
                {"LifeHacker"                                         ,"https://lifehacker.com/rss"},
                {"Reuters"                                            ,"https://www.reuters.com/tools/rss"},
                {"Politico"                                           ,"http://www.politico.com/rss/politicopicks.xml"},
                {"Yahoo News"                                         ,"https://www.yahoo.com/news/rss"},
                {"Los Angeles Times"                                  ,"https://www.latimes.com/local/rss2.0.xml"},
             };


            GetLastActivity(companies);
            Console.WriteLine();
            foreach (var c in GetCompaniesWithNoActivityFor(companies, 1))
                Console.WriteLine($"{c} hasn't had an update in the range");
        }

        static void GetLastActivity(IDictionary<string, string> rssUrlDictionary)
        {
            var now = DateTimeOffset.Now;
            foreach (var (company, url) in rssUrlDictionary)
            {
                try
                {
                    var days = 0.0;
                    using (var reader = XmlReader.Create(url, new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore }))
                    {
                        var last = SyndicationFeed.Load(reader).LastPublishOrUpdate();
                        days = (now - last).TotalDays;
                    }
                    Console.WriteLine($"{company} last updated {days} days ago");
                }
                catch (Exception ex)
                {
                    Console.Write($"{company}'s RSS feed read error: {ex.Message}");
                }               
            }
        }


        static IEnumerable<string> GetCompaniesWithNoActivityFor(IDictionary<string, string> rssUrlDictionary, int numberOfDays)
        {
            var now = DateTimeOffset.Now;
            foreach (var (company, url) in rssUrlDictionary)
            {
                DateTimeOffset? lastActivity = default;
                try
                {
                    using (var reader = XmlReader.Create(url, new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore }))                    
                         lastActivity = SyndicationFeed.Load(reader).LastPublishOrUpdate();                    
                }
                catch (Exception ex)
                {

                }

                if (lastActivity.HasValue && (now - lastActivity.Value).TotalDays > numberOfDays)
                    yield return company;
            }
        }
    }
}
