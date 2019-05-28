using System.Collections.Generic;
using Archetype.Models;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using UmbracoCmsLibrary.Models;

namespace UmbracoCmsLibrary.Helpers
{
    public  class HomeHelper
    {
        private   IPublishedContent _currentPage;
        private  UmbracoHelper _uHelper;
        private string _homeDocTypeAlias;
        private string _featuredItemsAlias;
        private string _imageAlias;
        private string _pageAlias;
        private string _categoryAlias;
        private string _nameAlias;
        private string _latestBlogPostsTitle;
        private string _latestBlogPostsIntroduction;
        public  HomeHelper(IPublishedContent currentPage, UmbracoHelper uHelper
            , string homeDocTypeAlias= "home",
            string featuredItemsAlias= "featuredItems",
            string imageAlias ="image",
            string pageAlias = "page",
            string categoryAlias = "category",
            string nameAlias = "name", 
            string latestBlogPostsTitle = "latestBlogPostsTitle",
            string latestBlogPostsIntroduction = "latestBlogPostsIntroduction")
        {
            _currentPage = currentPage;
            _uHelper = uHelper;
            _homeDocTypeAlias = homeDocTypeAlias;
            _featuredItemsAlias = featuredItemsAlias;
            _imageAlias = imageAlias;
            _pageAlias = pageAlias;
            _categoryAlias = categoryAlias;
            _nameAlias = nameAlias;
            _latestBlogPostsTitle = latestBlogPostsTitle;
            _latestBlogPostsIntroduction = latestBlogPostsIntroduction;
        }

        public List<FeaturedItem> GetFeaturedItemsModel()
        {
            //IPublishedContent homePage = CurrentPage.AncestorOrSelf(1).DescendantsOrSelf().Where(x => x.DocumentTypeAlias.Equals(HOME_PAGE_DOC_TYPE_ALIAS, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            IPublishedContent homePage = _currentPage.AncestorOrSelf(_homeDocTypeAlias);
            List<FeaturedItem> model = new List<FeaturedItem>();

            ArchetypeModel featuredItems = homePage.GetPropertyValue<ArchetypeModel>(_featuredItemsAlias);

            foreach (ArchetypeFieldsetModel fieldSet in featuredItems)
            {
                //name, categoryAlias, image, page
                string name = fieldSet.GetValue<string>(_nameAlias);
                string category = fieldSet.GetValue<string>(_categoryAlias);
                int imageId = fieldSet.GetValue<int>(_imageAlias);
                int pageId = fieldSet.GetValue<int>(_pageAlias);

                var mediaItem = _uHelper.Media(imageId);
                string imageUrl = mediaItem.Url;
                IPublishedContent linkedToPage = _uHelper.TypedContent(pageId);
                string linkUrl = linkedToPage.Url;
                model.Add(new FeaturedItem() { Name = name, Category = category, ImageUrl = imageUrl, LinkUrl = linkUrl });
            }
            return model;
        }

        public LatestBlogPost GetLatestBlogPostModel()
        {
            IPublishedContent page = _currentPage.AncestorOrSelf(_homeDocTypeAlias);
            LatestBlogPost model = new LatestBlogPost()
            {
                Title = page.GetPropertyValue<string>(_latestBlogPostsTitle),
                Introduction = page.GetPropertyValue<string>(_latestBlogPostsIntroduction)
            };
            return model;
        }

    }
}
