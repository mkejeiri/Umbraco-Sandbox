using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Archetype.Models;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using UmbracoCms.Models;

namespace UmbracoCms.Controllers
{
    public class BlogController : SurfaceController
    {
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/Blog/";
        private const string CURRENT_PAGE_NAME = "blog";
        public ActionResult RenderPostList()
        {

            IPublishedContent blogPage = CurrentPage.AncestorOrSelf(1).DescendantsOrSelf().Where(x => x.DocumentTypeAlias.Equals(CURRENT_PAGE_NAME, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            List<BlogPreview> model = new List<BlogPreview>();



            foreach (IPublishedContent page in blogPage.Children.OrderByDescending(x=> x.UpdateDate))
            {
               
                int imageId = page.GetPropertyValue<int>("articleImage");
                var mediaItem = Umbraco.Media(imageId);
                model.Add(new BlogPreview()
                    { Name = page.Name,
                        Introduction = page.GetPropertyValue<string>("articleIntro"),
                        ImageUrl = mediaItem.Url,
                        LinkUrl = page.Url });
            }
            return PartialView($"{PARTIAL_VIEW_FOLDER}_PostList.cshtml", model);
        }
    }
}