using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DUANCUAHANGAPPLE.Data;
using DUANCUAHANGAPPLE.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace DUANCUAHANGAPPLE.Services
{
    class RssBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        
        public RssBackgroundService(IServiceScopeFactory scopeFactory){
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var rssService = new RssService();
            while(!stoppingToken.IsCancellationRequested)
            {
                try 
                {
                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var rssUrl = "https://vnexpress.net/rss/so-hoa.rss";
                    var news = rssService.GetNews(rssUrl);
                    foreach(var item in news){
                        var content = (item.Tieude+" ").ToLower(); 
                        if (
                            content.Contains("điện thoại") ||
                            content.Contains("iphone") ||
                            content.Contains("samsung") ||
                            content.Contains("công nghệ") ||
                            content.Contains("laptop") ||
                            content.Contains("ai") ||
                            content.Contains("chip")
                        ){
                            if(!dbContext.News.Any(n=>n.Link == item.Link))
                            {
                                dbContext.News.Add(item);
                            }
                        }
                    }
                    await dbContext.SaveChangesAsync();
                    // giới hạn bài viết.
                    var totalNews = dbContext.News.Count();

                    if (totalNews > 20)
                    {
                        var oldNews = dbContext.News
                            .OrderBy(n => n.Ngaytao)
                            .Take(totalNews - 20)    
                            .ToList();

                        dbContext.News.RemoveRange(oldNews);
                        await dbContext.SaveChangesAsync();
                    }
                    
                }
                catch (Exception)
                {
                    
                }
                await Task.Delay(TimeSpan.FromMinutes(20), stoppingToken);
            }
        }
    }
} 