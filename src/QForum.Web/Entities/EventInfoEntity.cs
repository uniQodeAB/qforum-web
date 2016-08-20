using System;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Table;

namespace QForum.Web.Entities
{
    public class EventInfoEntity : TableEntity
    {
        public EventInfoEntity()
        {
            
        }

        public EventInfoEntity(DateTime eventDate) 
            : base(eventDate.ToString(CultureInfo), eventDate.ToString(CultureInfo))
        {

        }

        private static readonly CultureInfo CultureInfo = new CultureInfo("sv-SE");

        public DateTime EventDate { get; set; }

        public string RestaurantName { get; set; }

        public string RestaurantMenuUrl { get; set; }
    }
}