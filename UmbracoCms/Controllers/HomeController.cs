using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using UmbracoCms.Models;
using Archetype.Models;
using UmbracoCmsLibrary.Helpers;
using UmbracoCmsLibrary.Models;

namespace UmbracoCms.Controllers
{

    public class HomeController : SurfaceController
    {
        //private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/Home/";
        private HomeHelper _helper => new HomeHelper(CurrentPage, new UmbracoHelper(UmbracoContext.Current));
        private string PartialViewPath(string name)
        {
            return $"~/Views/Partials/Home/{name}.cshtml";
        }
        private const int MAX_TESTIMONIAL = 4;

        public ActionResult RenderIntro()
        {
            return PartialView(PartialViewPath("_Intro"));
        }
        public ActionResult RenderFeatured()
        {
            var model = _helper.GetFeaturedItemsModel();
            return PartialView(PartialViewPath("_Featured"), model);
        }

        //private List<FeaturedItem> GetFeaturedItemsModel()
        //{
        //    var model = new List<FeaturedItem>();
        //    //IPublishedContent homePage = CurrentPage.AncestorOrSelf(1).DescendantsOrSelf().Where(x => x.DocumentTypeAlias.Equals(HOME_PAGE_DOC_TYPE_ALIAS, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        //    var homePage = CurrentPage.AncestorOrSelf("home");
        //    var featuredItems = homePage.GetPropertyValue<ArchetypeModel>("featuredItems");

        //    foreach (var fieldSet in featuredItems)
        //    {
        //        //name, category, image, page
        //        var name = fieldSet.GetValue<string>("name");
        //        var category = fieldSet.GetValue<string>("category");
        //        var imageId = fieldSet.GetValue<int>("image");
        //        var pageId = fieldSet.GetValue<int>("page");

        //        var mediaItem = Umbraco.Media(imageId);
        //        string imageUrl = mediaItem.Url;
        //        var linkedToPage = Umbraco.TypedContent(pageId);
        //        var linkUrl = linkedToPage.Url;
        //        model.Add(new FeaturedItem {Name = name, Category = category, ImageUrl = imageUrl, LinkUrl = linkUrl});
        //    }

        //    return model;
        //}

        public ActionResult RenderServices()
        {
            return PartialView(PartialViewPath("_Services"));
        }

        public ActionResult RenderBlog()
        {
          
            var model = _helper.GetLatestBlogPostModel();

            return PartialView(PartialViewPath("_Blog"), model);
        }

        //public LatestBlogPost GetLatestBlogPostModel()
        //{
        //    IPublishedContent page = CurrentPage.AncestorOrSelf("home");
        //    LatestBlogPost model = new LatestBlogPost()
        //    {
        //        Title = page.GetPropertyValue<string>("latestBlogPostsTitle"),
        //        Introduction = page.GetPropertyValue<string>("latestBlogPostsIntroduction")
        //    };
        //    return model;
        //}

        public ActionResult RenderTestimonials()
        {
            const string HOME_PAGE_DOC_TYPE_ALIAS = "home";
            IPublishedContent page = CurrentPage.AncestorOrSelf(HOME_PAGE_DOC_TYPE_ALIAS);
            ArchetypeModel testimonialList = page.GetPropertyValue<ArchetypeModel>("testimonialList");

            List<TestimonialModel> testimonials = new List<TestimonialModel>();
            if (testimonials != null || testimonials.Count > 0)
            {
                foreach (ArchetypeFieldsetModel fieldSet in testimonialList.Take(MAX_TESTIMONIAL))
                {
                    testimonials.Add(new TestimonialModel()
                    {
                        Name = fieldSet.GetValue<string>("name"),
                        Quote = fieldSet.GetValue<string>("quote")
                    });
                }
            }


            TestimonialsModel model = new TestimonialsModel()
            {
                Title = page.GetPropertyValue<string>("testimonialsTitle"),
                Introduction = page.GetPropertyValue<string>("testimonialsIntroduction"),
                Testimonials = testimonials
            };
            return PartialView(PartialViewPath("_Testimonials"), model);
        }
    }
}