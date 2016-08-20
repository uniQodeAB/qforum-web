using System;
using System.ComponentModel.DataAnnotations;

namespace QForum.Web.Models
{
    public class EventInfoModel
    {
        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string RestaurantName { get; set; }

        [Required]
        public string RestaurantMenuUrl { get; set; }
    }
}