using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using UmbracoCms.Models;
using Archetype.Models;

namespace UmbracoCms.Controllers
{

    public class HomeController : SurfaceController
    {
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/Home/";

        public ActionResult RenderIntro()
        {
            return PartialView($"{PARTIAL_VIEW_FOLDER}_Intro.cshtml");
        }
        public ActionResult RenderFeatured()
        {
            const string HOME_PAGE_DOC_TYPE_ALIAS = "home";

            IPublishedContent homePage = CurrentPage.AncestorOrSelf(1).DescendantsOrSelf().Where(x => x.DocumentTypeAlias.Equals(HOME_PAGE_DOC_TYPE_ALIAS, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            List<FeaturedItem> model = new List<FeaturedItem>();

            ArchetypeModel featuredItems = homePage.GetPropertyValue<ArchetypeModel>("featuredItems");

            foreach (ArchetypeFieldsetModel fieldSet in featuredItems)
            {
                //name, category, image, page
                string name = fieldSet.GetValue<string>("name");
                string category = fieldSet.GetValue<string>("category");
                int imageId = fieldSet.GetValue<int>("image");
                int pageId = fieldSet.GetValue<int>("page");

                var mediaItem = Umbraco.Media(imageId);
                string imageUrl = mediaItem.Url;
                IPublishedContent linkedToPage = Umbraco.TypedContent(pageId);
                string linkUrl = linkedToPage.Url;
                model.Add(new FeaturedItem() { Name = name, Category = category, ImageUrl = imageUrl, LinkUrl = linkUrl });
            }
            return PartialView($"{PARTIAL_VIEW_FOLDER}_Featured.cshtml", model);
        }

        public ActionResult RenderServices()
        {
            return PartialView($"{PARTIAL_VIEW_FOLDER}_Services.cshtml");
        }

        public ActionResult RenderBlog()
        {
            return PartialView($"{PARTIAL_VIEW_FOLDER}_Blog.cshtml");
        }

        public ActionResult RenderClient()
        {
            return PartialView($"{PARTIAL_VIEW_FOLDER}_Client.cshtml");
        }
    }
}