using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.DataLayer.Utility.Table;

namespace UmbracoCms.Models
{
    public class TestimonialsModel
    {
        public string Title { get; set; }
        public string Introduction { get; set; }
        public List<TestimonialModel> Testimonials { get; set; }
        public bool HasTestimonials => Testimonials != null && Testimonials.Count > 0;

        public string ColumnClass
        {
            get
            {
                switch (Testimonials.Count)
                {
                    case 2:
                        return "col-md-6";
                    case 3:
                        return "col-md-4";
                    case 4:
                        return "col-md-3";
                    default:
                        return "col-md-12";
                }
            }
        }
    }
}