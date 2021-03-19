using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace RSSActivityAnalysis
{
    public static class SyndicationFeedExtensions
    {
        public static DateTimeOffset LastPublishOrUpdate(this SyndicationFeed feed)
            => Helpers.Max(feed.LastUpdatedTime, feed.Items.Max(i => Helpers.Max(i.PublishDate, i.LastUpdatedTime)));
    }
}
