using System.Collections.Generic;
using System.Web;
using CleanBlog.Core.ViewModels;
using Umbraco.Core.Models.PublishedContent;

namespace CleanBlog.Core.Services
{
    public interface IArticleService
    {
        IPublishedContent GetArticleListPage(IPublishedContent siteRoot);
        IEnumerable<IPublishedContent> GetLatestArticles(IPublishedContent siteroot);
        ArticleResultSet GetLatestArticles(IPublishedContent currentContentItem, HttpRequestBase request);
    }
}
