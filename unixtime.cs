/*
namespace DreamboxControl
{

// Convert Unix timestamp to local time

        private string unixtimeToString(int unixtime)
        {
            if (unixtime > 0)
            {
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(unixtime).ToLocalTime();
                return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
            }
            else
            {
                return "";
            }
        }
}
*/