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
                //{"New York Times "                                    ,"https://archive.nytimes.com/www.nytimes.com/services/xml/rss/index.html?mcubz=0"},
               // {"Huffington Post"                                    ,"https://www.huffpost.com/section/front-page/feed?x=1"},
                //{"Fox News"                                           ,"http://www.foxnews.com/about/rss/"},
                {"USA Today"                                          ,"http://rssfeeds.usatoday.com/UsatodaycomNation-TopStories"},
                {"LifeHacker"                                         ,"https://lifehacker.com/rss"},
                //{"Reuters"                                            ,"https://www.reuters.com/tools/rss"},
                {"Politico"                                           ,"http://www.politico.com/rss/politicopicks.xml"},
                {"Yahoo News"                                         ,"https://www.yahoo.com/news/rss"},
                //{"NPR"                                                ,"https://help.npr.org/customer/portal/articles/2094175-where-can-i-find-npr-rss-feeds-"},
                {"Los Angeles Times"                                  ,"https://www.latimes.com/local/rss2.0.xml"},
             };


            GetLastActivity(companies);
        }


        static double GetLastActivity(string url)
        {
            var now = DateTimeOffset.Now;
            using (var reader = XmlReader.Create(url))
            {
                var feed = SyndicationFeed.Load(reader);
                var lastUpdate = feed.Items.Max(i => i.PublishDate > i.LastUpdatedTime ? i.PublishDate : i.LastUpdatedTime);
                return (now - lastUpdate).TotalDays;
            }
        }

        static void GetLastActivity(IDictionary<string, string> rssUrlDictionary)
        {
            foreach (var (company, url) in rssUrlDictionary)
            {
                Console.WriteLine($"{company} last updated {GetLastActivity(url)} days ago");
            }
        }


        static IEnumerable<string> GetCompaniesWithNoActivityFor(IDictionary<string, string> rssUrlDictionary, int numberOfDays)
        {
            var now = DateTimeOffset.Now;
            foreach (var (company, url) in rssUrlDictionary)
            {
                using (var reader = XmlReader.Create(url))
                {
                    var feed = SyndicationFeed.Load(reader);
                    if ((now - feed.LastUpdatedTime).TotalDays > numberOfDays)
                        yield return company;
                }
            }
        }
    }
}
