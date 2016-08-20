using System;
using System.Globalization;
using System.Text;

namespace QForum.Web.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToReadableDate(this DateTime dateTime)
        {
            var culture = new CultureInfo("sv-SE");
            var builder = new StringBuilder();

            // ToString("dddden dde MMMM", new CultureInfo("sv-SE"));

            builder.Append(dateTime.ToString("dddden", culture));
            builder.Append(" den ");
            builder.Append(dateTime.ToString("d:e", culture));
            builder.Append(" ");

            var month = dateTime.ToString("MMMM", culture);
            month = month[0].ToString().ToUpper() + month.Substring(1);

            builder.Append(month);
            builder.Append(" kl. ");
            builder.Append(dateTime.ToString("HH:mm", culture));

            return builder.ToString();
        }
    }
}