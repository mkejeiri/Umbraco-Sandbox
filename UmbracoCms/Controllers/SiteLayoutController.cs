﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using UmbracoCms.Models;
using Umbraco.Web;

namespace UmbracoCms.Controllers
{

    public class SiteLayoutController : SurfaceController
    {
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/SiteLayout/";

        //public ActionResult RenderHeader()
        //{
        //    return PartialView($"{PARTIAL_VIEW_FOLDER}_Header.cshtml");
        //}

        public ActionResult RenderFooter()
        {
            return PartialView($"{PARTIAL_VIEW_FOLDER}_Footer.cshtml");
        }

        public ActionResult RenderTitleControls()
        {
            return PartialView($"{PARTIAL_VIEW_FOLDER}_TitleControls.cshtml");
        }


        /// <summary>
        /// Renders the top navigation partial in the header partial
        /// </summary>
        /// <returns>Partial view with a model</returns>

        public ActionResult RenderHeader()
        {
            MemoryCache.Default.Remove("mainNav");
            List<NavigationListItem> nav = GetObjectFromCache<List<NavigationListItem>>("mainNav", 5, GetNavigationModelFromDatabase);
            //List < NavigationListItem > nav = GetNavigationModelFromDatabase();
            return PartialView($"{PARTIAL_VIEW_FOLDER}_Header.cshtml", nav);
        }

        //public ActionResult RenderTopNavigation()
        //{
        //    List<NavigationListItem> nav = GetNavigationModelFromDatabase();
        //    return PartialView($"{PARTIAL_VIEW_FOLDER}_TopNavigation.cshtml", nav);
        //}

        /// <summary>
        /// Finds the home page and gets the navigation structure based on it and it's children
        /// </summary>
        /// <returns>A List of NavigationListItems, representing the structure of the site.</returns>
        private List<NavigationListItem> GetNavigationModelFromDatabase()
        {
            //const int HOME_PAGE_POSITION_IN_PATH = 1;
            //int homePageId = int.Parse(CurrentPage.Path.Split(',')[HOME_PAGE_POSITION_IN_PATH]);
            //IPublishedContent homePage = Umbraco.Content(homePageId);
            const string HOME_PAGE_DOC_TYPE_ALIAS = "home";

            IPublishedContent homePage = CurrentPage.AncestorOrSelf(1).DescendantsOrSelf().Where(x => x.DocumentTypeAlias.Equals(HOME_PAGE_DOC_TYPE_ALIAS, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            List<NavigationListItem> nav = new List<NavigationListItem>();
            nav.Add(new NavigationListItem(new NavigationLink(homePage.Url, homePage.Name)));
            nav.AddRange(GetChildNavigationList(homePage));
            return nav;
        }

        /// <summary>
        /// Loops through the child pages of a given page and their children to get the structure of the site.
        /// </summary>
        /// <param name="page">The parent page which you want the child structure for</param>
        /// <returns>A List of NavigationListItems, representing the structure of the pages below a page.</returns>
        private List<NavigationListItem> GetChildNavigationList(IPublishedContent page)
        {
            List<NavigationListItem> listItems = null;
            var childPages = page.Children.Where("Visible")
                .Where(x=> !x.HasValue("excludeFromTopNavigation") ||
                (x.HasValue("excludeFromTopNavigation") && !x.GetPropertyValue<bool>("excludeFromTopNavigation")));
            if (childPages != null && childPages.Any() && childPages.Count() > 0)
                if (childPages != null && childPages.Count() > 0)
                {
                    listItems = new List<NavigationListItem>();
                    foreach (var childPage in childPages)
                    {
                        NavigationListItem listItem = new NavigationListItem(new NavigationLink(childPage.Url, childPage.Name));
                        listItem.Items = GetChildNavigationList(childPage);
                        listItems.Add(listItem);
                    }
                }
            return listItems;
        }

        /// <summary>
        /// A generic function for getting and setting objects to the memory cache.
        /// </summary>
        /// <typeparam name="T">The type of the object to be returned.</typeparam>
        /// <param name="cacheItemName">The name to be used when storing this object in the cache.</param>
        /// <param name="cacheTimeInMinutes">How long to cache this object for.</param>
        /// <param name="objectSettingFunction">A parameterless function to call if the object isn't in the cache and you need to set it.</param>
        /// <returns>An object of the type you asked for</returns>
        private static T GetObjectFromCache<T>(string cacheItemName, int cacheTimeInMinutes, Func<T> objectSettingFunction)
        {
            ObjectCache cache = MemoryCache.Default;
            var cachedObject = (T)cache[cacheItemName];
            if (cachedObject == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheTimeInMinutes);
                cachedObject = objectSettingFunction();
                cache.Set(cacheItemName, cachedObject, policy);
            }
            return cachedObject;
        }
    }
}