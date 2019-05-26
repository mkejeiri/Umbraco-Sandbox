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
        //private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/Home/";

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
            const string HOME_PAGE_DOC_TYPE_ALIAS = "home";

            //IPublishedContent homePage = CurrentPage.AncestorOrSelf(1).DescendantsOrSelf().Where(x => x.DocumentTypeAlias.Equals(HOME_PAGE_DOC_TYPE_ALIAS, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            IPublishedContent homePage = CurrentPage.AncestorOrSelf(HOME_PAGE_DOC_TYPE_ALIAS);
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
            return PartialView(PartialViewPath("_Featured"), model);
        }

        public ActionResult RenderServices()
        {
            return PartialView(PartialViewPath("_Services"));
        }

        public ActionResult RenderBlog()
        {
            const string HOME_PAGE_DOC_TYPE_ALIAS = "home";
            IPublishedContent page = CurrentPage.AncestorOrSelf(HOME_PAGE_DOC_TYPE_ALIAS);
            LatestBlogPost model = new LatestBlogPost()
            {
                Title = page.GetPropertyValue<string>("latestBlogPostsTitle"),
                Introduction = page.GetPropertyValue<string>("latestBlogPostsIntroduction")
            };

            return PartialView(PartialViewPath("_Blog"), model);
        }

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