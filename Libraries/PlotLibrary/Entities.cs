using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plot.Entities
{
    /// <summary>
    /// Exception raised on entity validation error
    /// </summary>
    public class EntityException : Exception
    {
        /// <summary>
        /// Name of the property raising the error
        /// </summary>
        public string PropertyName { get; private set; }

        public EntityException(string propertyName, string message)
            : base(message)
        {
            PropertyName = propertyName;
        }
    }

    /// <summary>
    /// Base class of all Plot entities
    /// </summary>
    public class Entity
    {
        protected Boolean forAdmin = true; // for backwards compatibility to admin interface

        /// <summary>
        /// Call this method to validate the entities properties
        /// </summary>
        public void Check()
        {
            DoCheck();
        }

        public void Check(Boolean forAdmin)
        {
            this.forAdmin = forAdmin;
            DoCheck();
        }

        /// <summary>
        /// Can be overridden in derived classes to add extra validation methods
        /// </summary>
        protected virtual void DoCheck()
        {
        }
    }

    // create
    /// <summary>
    /// Use this entity to create a new store in API v1
    /// </summary>
    public class CreateStoreV1 : Entity
    {
        /// <summary>
        /// Name of the store
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Address of the store
        /// </summary>
        public string Address { get; set; } // only admin
        /// <summary>
        /// Postal code of the store
        /// </summary>
        public string PostalCode { get; set; } // only admin
        /// <summary>
        /// City of the store
        /// </summary>
        public string City { get; set; } // only admin
        /// <summary>
        /// Two letter lowercase ISO3166 country code (e.g. nl, de, be)
        /// </summary>
        public string Country { get; set; } // only admin
        /// <summary>
        /// Latitude of coordinate of the stores location
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Longitude of coordinate of the stores location
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Additional checks for store creation
        /// </summary>
        protected override void DoCheck()
        {
            base.DoCheck();
            if (string.IsNullOrWhiteSpace(Name))
                throw new EntityException("Store.Name", "Store.Name may not be empty");
            if (Name.Length > 254)
                throw new EntityException("Store.Name", "Length of Store.Name may not exceed 254");
            if (string.IsNullOrWhiteSpace(Address))
                throw new EntityException("Store.Address", "Store.Address may not be empty");
            if (Address.Length > 254)
                throw new EntityException("Store.Address", "Length of Store.Address may not exceed 254");
            if (string.IsNullOrWhiteSpace(PostalCode))
                throw new EntityException("Store.PostalCode", "Store.PostalCode may not be empty");
            if (PostalCode.Length > 254)
                throw new EntityException("Store.PostalCode", "Length of Store.PostalCode may not exceed 254");
            if (string.IsNullOrWhiteSpace(City))
                throw new EntityException("Store.City", "Store.City may not be empty");
            if (City.Length > 254)
                throw new EntityException("Store.City", "Length of Store.City may not exceed 254");
            if (string.IsNullOrWhiteSpace(Country))
                throw new EntityException("Store.Country", "Store.Country may not be empty");
            if (Country.Length != 2)
                throw new EntityException("Store.Country", "Length of Store.Country must be 2 (two letter lowercase ISO-3166 country code; e.g. nl, de, be)");
            if (Latitude == 0)
                throw new EntityException("Store.Latitude", "Store.Latitude may not be 0");
            if (Longitude == 0)
                throw new EntityException("Store.Longitude", "Store.Longitude may not be 0");
        }

#if FALSE
        /// <summary>
        /// Helper property to get or set the location using a DbGeography object
        /// </summary>
        public System.Data.Spatial.DbGeography Location
        {
            get
            {
                return System.Data.Spatial.DbGeography.PointFromText(string.Format(System.Globalization.CultureInfo.InvariantCulture, "POINT({0} {1})", Longitude, Latitude), 4326);
            }
            set
            {
                if (value == null)
                {
                    Latitude = 0;
                    Longitude = 0;
                }
                else
                {
                    Latitude = value.Latitude ?? 0;
                    Longitude = value.Longitude ?? 0;
                }
            }
        }
#endif
    }

    /// <summary>
    /// Use this generic entity to create a new store
    /// </summary>
    public class CreateStore : CreateStoreV1
    {
    }

    // select
    /// <summary>
    /// Entity containing all v1 store data
    /// </summary>
    public class SelectStoreV1 : CreateStoreV1
    {
        /// <summary>
        /// Unique identifier of the store
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// UTC date/time indicating when the store was created
        /// </summary>
        public DateTime Created { get; set; }
    }

    /// <summary>
    /// Generic entity containing all store data
    /// </summary>
    public class SelectStore : SelectStoreV1
    {
    }

    // create/update
    /// <summary>
    /// Entity containing data to create or update a notification in API v1
    /// </summary>
    public class CreateUpdateNotificationV1 : Entity
    {
        public const int MatchRangeDefault = 200;

        public class Timespan
        {
            /// <summary>
            /// Optional start date of the notification
            /// </summary>
            public DateTime? Start { get; set; } // optional
            /// <summary>
            /// Optional end date of the notification
            /// </summary>
            public DateTime? End { get; set; } // optional

            /// <summary>
            /// Checks for timespan. Can be overridden in derived classes
            /// </summary>
            protected internal virtual void DoCheck()
            {
                // nothing to do
            }
        }

        /// <summary>
        /// Message to display to the user on notification
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Data supplied to the (end) user program when a user clicks/opens the notification. Example for SC: {"couponId" : 371}
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 0..* timespans consisting of a start date, end date or both
        /// </summary>
        public Timespan[] Timespans { get; set; }
        /// <summary>
        /// Range in meters at which this notification is triggered. Currently 200 m is advised (default)
        /// </summary>
        public int? MatchRange { get; set; } // range in meters at which this notification is triggered
        /// <summary>
        /// Indictes if notification is published. Default is false
        /// </summary>
        public Boolean? Published { get; set; }

        private const int MinMatchRange = 200;
        private const int MaxMatchRange = 10000;

        /// <summary>
        /// Additional checks for notification creation/updation
        /// </summary>
        protected override void DoCheck()
        {
            base.DoCheck();
            if (string.IsNullOrWhiteSpace(Message) && forAdmin)
                throw new EntityException("Notification.Message", "Notification.Message may not be empty");
            if (!string.IsNullOrWhiteSpace(Message) && Message.Length > 2048)
                throw new EntityException("Notification.Message", "Length of Notification.Message may not exceed 2048");
            if (string.IsNullOrWhiteSpace(Data) && forAdmin)
                throw new EntityException("Notification.Data", "Notification.Data may not be empty");
            if (!string.IsNullOrWhiteSpace(Data) && Data.Length > 2048)
                throw new EntityException("Notification.Data", "Length of Notification.Data may not exceed 2048");
            if (MatchRange == 0)
                throw new EntityException("Notification.MatchRange", "Notification.MatchRange may not be 0");
            if (MatchRange < MinMatchRange || MatchRange > MaxMatchRange)
                throw new EntityException("Notification.MatchRange", string.Format("Notification.MatchRange ({0}) out of bounds [{1}..{2}]", MatchRange, MinMatchRange, MaxMatchRange));
            if (Timespans != null)
                foreach (Timespan ts in Timespans)
                    ts.DoCheck();
        }
    }

    // create
    /// <summary>
    /// Entity containing all data to create a notification in API v1
    /// </summary>
    public class CreateNotificationV1 : CreateUpdateNotificationV1
    {
        /// <summary>
        /// Unique ID of the store for which the notification is created
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// Additional checks for notification creation
        /// </summary>
        protected override void DoCheck()
        {
            base.DoCheck();
            if (string.IsNullOrWhiteSpace(StoreId))
                throw new EntityException("Notification.StoreId", "Notification.StoreId may not be empty");
            if (StoreId.Length != 32)
                throw new EntityException("Notification.StoreId", "Length of Notification.StoreId must be 32");
        }
    }

    /// <summary>
    /// Generic entity containing all data to create a notification
    /// </summary>
    public class CreateNotification : CreateNotificationV1
    {
    }

    // update
    /// <summary>
    /// Entity containing all data to update a notification in API v1
    /// </summary>
    public class UpdateNotificationV1 : CreateUpdateNotificationV1
    {
        /// <summary>
        /// Unique ID of the notification that is to be updated
        /// </summary>
        public string NotificationId { get; set; }

        /// <summary>
        /// Additional checks for notification updation
        /// </summary>
        protected override void DoCheck()
        {
            base.DoCheck();
            if (string.IsNullOrWhiteSpace(NotificationId))
                throw new EntityException("Notification.NotificationId", "Notification.NotificationId may not be empty");
            if (NotificationId.Length != 32)
                throw new EntityException("Notification.NotificationId", "Length of Notification.NotificationId must be 32");
        }
    }

    /// <summary>
    /// Generic entity containing all data to update a notification
    /// </summary>
    public class UpdateNotification : UpdateNotificationV1
    {
    }

    /// <summary>
    /// Indicates the state of a notification, New is 'created', Published is 'available to user' and Unpublished is 'deleted'
    /// </summary>
    public enum State { New, Published, Unpublished };

    //select
    /// <summary>
    /// Entity containing all v1 notification data
    /// </summary>
    public class SelectNotificationV1 : CreateNotificationV1
    {
        /// <summary>
        /// Unique identifier of this notification
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Indicates the state of a notification, New is 'created', Published is 'available to user' and Unpublished is 'deleted'
        /// </summary>
        public State State { get; set; } // "new" or "published" or "unpublished"
        /// <summary>
        /// UTC date/time indicating when the notification was created
        /// </summary>
        public DateTime Created { get; set; }
    }

    /// <summary>
    /// Generic entity containing all notification data
    /// </summary>
    public class SelectNotification : SelectNotificationV1
    {
    }

    /// <summary>
    /// Entity containing all v1 statistics data
    /// </summary>
    public class StatisticV1
    {
        /// <summary>
        /// Unique identifier of the notification
        /// </summary>
        public string NotificationId { get; set; }
        /// <summary>
        /// The date of the day this statistic is measured
        /// </summary>
        public DateTime Day { get; set; }
        /// <summary>
        /// Number of times the notification is sent
        /// </summary>
        public int TimesSent { get; set; }
        /// <summary>
        /// Number of times the notification is viewed
        /// </summary>
        public int TimesViewed { get; set; }
    }

    /// <summary>
    /// Generic entity containing all statistics data
    /// </summary>
    public class Statistic : StatisticV1
    {
    }
}
