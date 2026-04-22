using System.Xml;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using DUANCUAHANGAPPLE.Models;

namespace DUANCUAHANGAPPLE.Services
{
    class RssService
    {
        public List<New> GetNews(string url)
        {
            //gõ link nhưng chưa enter.
            using var reader = XmlReader.Create(url);
            // bây giờ mới nhấn vào enter để link đến trang web.
            var feed = SyndicationFeed.Load(reader);
            var newsList = new List<New>();
            foreach(var item in feed.Items)
            {
                newsList.Add(new New
                {
                    Tieude = item.Title.Text ?? "",
                    Link = item.Links.FirstOrDefault()?.Uri.ToString() ?? "",
                    Motangan = Regex.Replace(item.Summary?.Text ?? "", "<.*?>", ""),
                    Ngaytao = item.PublishDate.DateTime,
                    ImageUrl = item.Links.FirstOrDefault(l => l.RelationshipType == "enclosure")?.Uri.ToString() ?? "",
                    Source = "VnExpress"
                });
            }
            return newsList;
        }
    }
}