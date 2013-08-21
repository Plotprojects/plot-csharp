using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plot.Models
{
    // TODO: document the source

    public class JSONobject
    {
    }

    public class JSONrequest : JSONobject
    {
    }

    public class JSONresponse : JSONobject
    {
        public Boolean Success { get; set; }
        public string ErrorMessage { get; set; } // optional
        public string ErrorCode { get; set; } // optional

        internal void Check()
        {
            //  response.Success
            if (!Success)
            {
                string em;
                //  response.ErrorMessage
                if (!string.IsNullOrWhiteSpace(ErrorMessage))
                    em = ErrorMessage;
                else
                    em = "Error returned.";
                // response.ErrorCode
                if (!string.IsNullOrWhiteSpace(ErrorCode))
                    em += " Error code: " + ErrorCode;
                throw new Exception(em);
            }
        }
    }
    
    public class GetPlaceResponse : JSONresponse
    {
        public class Store
        {
            public class StoreLocation
            {
                public double Latitude { get; set; }
                public double Longitude { get; set; }
            }

            public string ID { get; set; }
            public string Name { get; set; }
            public StoreLocation Location { get; set; }
            public string Created { get; set; } // date
        }

        public Store Result { get; set; }
    }

    // https://admin.plotprojects.com/v1/superadmin/findStores

    public class FindStoresResponse : JSONresponse
    {
        public class StoreResult
        {
            public class Store
            {
                public class StoreLocation
                {
                    public double Latitude { get; set; }
                    public double Longitude { get; set; }
                }

                public string ID { get; set; }
                public string AccountId { get; set; }
                public string Name { get; set; }
                public string Address { get; set; }
                public string PostalCode { get; set; }
                public string City { get; set; }
                public string Country { get; set; }
                public StoreLocation Location { get; set; }
                public string Created { get; set; } // date
            }

            public int Total { get; set; } // double ???
            public Store[] Data { get; set; }
        }

        public StoreResult Result { get; set; } // optional
    }

    public class CreateStoreRequest : JSONrequest // CASE-SENSITIVE !!!
    {
        public class StoreLocation
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
        }

        public string accountId { get; set; }

        public string name { get; set; }
        public string address { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }
        public string country { get; set; }

        public StoreLocation location { get; set; }
    }

    public class CreateStoreResponse : JSONresponse
    {
        public string Result { get; set; } // optional ???
    }

    public class UpdateStoreRequest : JSONrequest // CASE-SENSITIVE !!!
    {
        public class StoreLocation
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
        }

        public string name { get; set; }

        public StoreLocation location { get; set; }
    }

    public class GetNotificationResponse : JSONresponse
    {
        public class Notification
        {
            public class Timespan
            {
                public string Start { get; set; } // date, optional
                public string End { get; set; } // date, optional
            }

            public string ID { get; set; }
            public string PlaceId { get; set; }

            public string State { get; set; } // "new" or "published" or "unpublished"
            public string Message { get; set; }
            public string Data { get; set; }
            public int MatchRange { get; set; } // range in meters at which this notification is triggered
            public string Created { get; set; } // date

            public Timespan[] Timespans { get; set; }
        }

        public Notification Result { get; set; } // optional
    }

    public class FindNotificationsResponse : JSONresponse
    {
        public class NotificationResult
        {
            public class Notification
            {
                public class Timespan
                {
                    public string Start { get; set; } // date, optional
                    public string End { get; set; } // date, optional
                }

                public string ID { get; set; }
                public string StoreId { get; set; }

                public string State { get; set; } // "new" or "published" or "unpublished"
                public string Message { get; set; }
                public string Data { get; set; }
                public int MatchRange { get; set; } // range in meters at which this notification is triggered
                public string Created { get; set; } // date

                public Timespan[] Timespans { get; set; }
            }

            public int Total { get; set; } // double ???
            public Notification[] Data { get; set; }
        }

        public NotificationResult Result { get; set; } // optional
    }

    public class CreateUpdateNotificationRequest : JSONrequest // CASE-SENSITIVE !!!
    {
        public class Timespan
        {
            public string start { get; set; } // date (ISO8601 formatted), optional
            public string end { get; set; } // date (ISO8601 formatted), optional
        }

        public string message { get; set; }

        public string data { get; set; }

        public Timespan[] timespans { get; set; }

        public int? matchRange { get; set; } // match range radius in meters (>= 200 and <= 10000)

        public Boolean? published { get; set; }
    }

    public class CreateNotificationRequest : CreateUpdateNotificationRequest
    {
        public string placeId { get; set; }
    }

    public class CreateNotificationResponse : JSONresponse
    {
        public string Result { get; set; } // optional
    }

    // CASE-SENSITIVE !!!
    public class UpdateNotificationRequest : CreateUpdateNotificationRequest
    {
        public string notificationId { get; set; } // just for admin!
    }

    public class UpdateNotificationResponse : JSONresponse
    {
    }

    public class PublishNotificationResponse : JSONresponse
    {
    }

    public class DeleteNotificationResponse : JSONresponse
    {
    }
    
    /* string lengths in bytes:
    Store:

    name: 254
    address: 254
    postal_code: 254
    city: 254


    Notification:

    message: 2048
    data: 2048
    */

    public class GetStatisticsResponse : JSONresponse
    {
        public class Statistic
        {
            public string ID { get; set; } // NotificationId
            public string Date { get; set; } // day
            public int TimesSent { get; set; }
            public int TimesViewed { get; set; }
        }

        public Statistic[] Result { get; set; } // optional
    }
}
