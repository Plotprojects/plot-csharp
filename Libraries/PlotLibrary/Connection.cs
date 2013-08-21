using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

using Plot.Models;
using Plot.Entities;

using PlotLibrary.Extensions;

namespace Plot
{
    /// <summary>
    /// Base class of the Plot connection object(s)
    /// </summary>
    public class ConnectionBase : IDisposable
    {
        protected readonly string accountId;
        private string privateKey;
        protected readonly Boolean isAdminConnection;
        private string version;

        /// <summary>
        /// Instantiate a connection object to access the Plot back-end server
        /// </summary>
        /// <param name="accountId">Unique ID of the account to use to access the Plot back-end server</param>
        /// <param name="privateKey">The (private) key supplied by Plot to access the account on the Plot back-end server</param>
        /// <param name="isAdminConnection">Indicates which interface to use</param>
        /// <param name="version">A string indicating which version of the API should be used. Currently only v1 is implemented</param>
        protected ConnectionBase(string accountId, string privateKey, Boolean isAdminConnection, string version)
        {
            this.accountId = accountId;
            this.privateKey = privateKey;
            this.isAdminConnection = isAdminConnection;
            this.version = version;
        }

        protected class Parameters : Dictionary<string, object>
        {
        }

        private readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();

        public void Dispose()
        {
            // not currently necessary
        }

        protected void CheckNotAdmin()
        {
            if (isAdminConnection)
                throw new Exception("Call not supported on admin connection");
        }

        protected void CheckAdmin()
        {
            if (!isAdminConnection)
                throw new Exception("Call only supported on admin connection");
        }

        protected HttpWebRequest GetWebRequestAndSendRequest(string method, string function, JSONrequest request, Parameters parameters = null, string urlTrailer = null)
        {
            HttpWebRequest ret = GetWebRequest(method, function, parameters, urlTrailer);

            string json = Serializer.Serialize(request);

            ret.ContentType = "application/json;charset=UTF-8";

            // Send the data:
            using (Stream s = ret.GetRequestStream())
            {
                using (StreamWriter requestWriter = new StreamWriter(s))
                {
                    requestWriter.Write(json);
                }
            }

            return ret;
        }

        protected HttpWebRequest GetWebRequest(string method, string function)
        {
            return GetWebRequest(method, function, null, null);
        }

        protected HttpWebRequest GetTrailedWebRequest(string method, string function, string urlTrailer)
        {
            return GetWebRequest(method, function, null, urlTrailer);
        }

        protected HttpWebRequest GetWebRequest(string method, string function, Parameters parameters, string urlTrailer = null)
        {
            string url = GetURL(function, parameters, urlTrailer);
            HttpWebRequest ret = System.Net.WebRequest.Create(url) as HttpWebRequest;
            ret.Method = method;
            if (string.IsNullOrWhiteSpace(privateKey))
                throw new Exception("Private API key not supplied");
            if (isAdminConnection)
                ret.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(":" + privateKey))); // no user-name!
            else
                ret.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(accountId + ":" + privateKey)));
            return ret;
        }

        protected string GetURL(string function)
        {
            if (isAdminConnection)
                return string.Format("https://admin.plotprojects.com/{0}/superadmin/{1}", HttpUtility.UrlEncode(version), HttpUtility.UrlEncode(function));
            else
                return string.Format("https://admin.plotprojects.com/api/{0}/{1}", HttpUtility.UrlEncode(version), HttpUtility.UrlEncode(function));
        }

        private Parameters AddAccountId(Parameters parameters)
        {
            Parameters ret = new Parameters();
            if (parameters != null)
                foreach (KeyValuePair<string, object> vt in parameters)
                    ret.Add(vt.Key, vt.Value);
            if (string.IsNullOrWhiteSpace(accountId))
                throw new Exception("accountId not supplied");
            ret.Add("accountId", accountId);
            return ret;
        }

        protected string GetURL(string function, Parameters parameters, string urlTrailer)
        {
            if (isAdminConnection)
                parameters = AddAccountId(parameters);
            return GetURL(function, parameters, urlTrailer, false);
        }

        private string GetURL(string function, Parameters parameters, string urlTrailer, Boolean addAccountId)
        {
            if (addAccountId)
                parameters = AddAccountId(parameters);

            string ret = GetURL(function);

            if (!string.IsNullOrWhiteSpace(urlTrailer))
                ret += "/" + HttpUtility.UrlEncode(urlTrailer);

            if (parameters == null || parameters.Count == 0)
                return ret;
            string p = string.Empty;
            foreach (KeyValuePair<string, object> vt in parameters)
            {
                object o = vt.Value;
                if (o != null)
                {
                    if (!string.IsNullOrWhiteSpace(p))
                        p += "&";
                    p += HttpUtility.UrlEncode(vt.Key) + "=" + HttpUtility.UrlEncode(vt.Value.ToString());
                }
            }
            if (!string.IsNullOrWhiteSpace(p))
                ret += "?" + p;
            return ret;
        }

        protected T GetJSONresponse<T>(HttpWebRequest webRequest)
        {
            string res = GetJSONresponse(webRequest);
            T ret = Serializer.Deserialize<T>(res);
            return ret;
        }

        private string GetJSONresponse(HttpWebRequest webRequest)
        {
            string res;

            using (HttpWebResponse resp = webRequest.GetResponse() as HttpWebResponse)
            {
                string enc = resp.ContentEncoding;

                if (!string.IsNullOrWhiteSpace(enc))
                {
                    enc = enc.ToLower();
                    if (enc != "gzip")
                        throw new Exception(string.Format("Unsupported content encoding '{0}'", enc));
                }
                else
                    enc = null;

                using (Stream resStream = resp.GetResponseStream()) // returns 400 if case-sentitivity problem!!!
                {
                    Stream c = null;

                    // check encoding (gzip)
                    if (enc == "gzip")
                        c = new System.IO.Compression.GZipStream(resStream, System.IO.Compression.CompressionMode.Decompress);
                    try
                    {
                        Stream s = c ?? resStream;

                        // check charset (UTF-8)
                        Encoding cs = Encoding.UTF8;
                        string scs = resp.CharacterSet;
                        if (!string.IsNullOrWhiteSpace(scs))
                            cs = Encoding.GetEncoding(scs);
                        using (StreamReader reader = new StreamReader(s, cs))
                        {
                            res = reader.ReadToEnd();
                        }
                    }
                    finally
                    {
                        if (c != null)
                            c.Dispose();
                    }
                }
            }
            return res;
        }
    }

    /// <summary>
    /// Connection object to access the v1 Plot back-end server
    /// </summary>
    public class ConnectionV1 : ConnectionBase
    {
        private const string Version = "v1";

        private const int MaxNone = -1;
        private const int MaxOnePage = 0;

        /// <summary>
        /// Instantiate a v1 connection object to access the v1 Plot back-end server using a "v1" version string
        /// </summary>
        /// <param name="accountId">Unique ID of the account to use to access the Plot back-end server</param>
        /// <param name="privateKey">The (private) key supplied by Plot to access the account on the Plot back-end server</param>
        /// <param name="isAdminConnection">Indicates which interface to use</param>
        protected ConnectionV1(string accountId, string privateKey, Boolean isAdminConnection)
            : base(accountId, privateKey, isAdminConnection, Version)
        {
        }

        /// <summary>
        /// Fetchs a page of stores. Page-size is default (currently 25)
        /// </summary>
        /// <param name="start">Skip start stores; 0 is default</param>
        /// <returns>A list of (generic) store entities</returns>
        public IList<SelectStore> GetStores(int start = 0)
        {
            return FindStores(start, MaxOnePage);
        }

        /// <summary>
        /// Fetch all stores from the Plot back-end server, optionally indication a maximum allowed number of records to fetch
        /// </summary>
        /// <param name="max">Maximum number of stores to retreive. May be 'null' for ALL</param>
        /// <returns>A list of (generic) store entities</returns>
        public IList<SelectStore> GetAllStores(int? max = null)
        {
            return FindStores(0, max ?? MaxNone);
        }

        /// <summary>
        /// After a (any) fetch of stores this property contains the total number of stores
        /// </summary>
        public int TotalStores { get; private set; }

        private IList<SelectStore> FindStores(int start, int max) // max -> -1 = ALL (no max), 0 = one page (default size)
        {
            List<FindStoresResponse.StoreResult.Store> ret = new List<FindStoresResponse.StoreResult.Store>();

            while (true)
            {
                // get one page (from start)
                FindStoresResponse.StoreResult res = FindStores(start);

                // save (grand) total found (not thread safe)
                TotalStores = res.Total;

                if (res.Data.Count() == 0) // strange error?
                    break;

                // add them all
                ret.AddRange(res.Data);

                if (max == MaxOnePage)
                    break;

                if (max != MaxNone) // a 'real' max (> 0)
                    if (ret.Count() >= max)
                    {
                        // remove if too many
                        while (ret.Count() > max)
                            ret.Remove(ret.Last());
                        break;
                    }

                if (ret.Count() == res.Total) // no more available
                    break;

                // next:
                start += res.Data.Count();
            }
            return ret.Select(s => new SelectStore()
            {
                ID = s.ID,
                Name = s.Name,
                Address = s.Address,
                PostalCode = s.PostalCode,
                City = s.City,
                Country = s.Country,
                Latitude = s.Location.Latitude,
                Longitude = s.Location.Longitude,
                Created = s.Created.AsDateTime(true).Value
            }).ToList();
        }

        private FindStoresResponse.StoreResult FindStores(int start)
        {
            const string FunctionAdmin = "findPlaces"; // Admin
            const string Function = "place";

            const string Start = "start";

            Parameters parameters = new Parameters() { { Start, start } };
            HttpWebRequest webRequest = GetWebRequest("GET", isAdminConnection ? FunctionAdmin : Function, parameters);

            FindStoresResponse response = GetJSONresponse<FindStoresResponse>(webRequest);

            if (response == null)
                throw new Exception("No response.");

            // check the result codes:
            response.Check();

            FindStoresResponse.StoreResult ret = response.Result ?? new FindStoresResponse.StoreResult() { Total = 0 };

            if (ret.Data == null)
                ret.Data = new FindStoresResponse.StoreResult.Store[] { };

            return ret;
        }

        /// <summary>
        /// Fetch a place from the Plot back-end server
        /// </summary>
        /// <param name="placeId">Id of the place to fetch</param>
        /// <returns>Entity containing the data of the place (in fact it is a store entity)</returns>
        public SelectStore GetPlace(string placeId)
        {
            const string Function = "place";

            CheckNotAdmin();

            HttpWebRequest webRequest = GetWebRequest("GET", Function, null, placeId);

            GetPlaceResponse response = GetJSONresponse<GetPlaceResponse>(webRequest);

            if (response == null)
                throw new Exception("No response.");

            // check the result codes:
            response.Check();

            SelectStore ret = null;
            if (response.Result != null)
                ret = new SelectStore()
                {
                    ID = response.Result.ID,
                    Name = response.Result.Name,
                    Latitude = response.Result.Location.Latitude,
                    Longitude = response.Result.Location.Longitude,
                    Created = response.Result.Created.AsDateTime(true).Value
                };

            return ret;
        }

        /// <summary>
        /// Update a place on the Plot back-end server
        /// </summary>
        /// <param name="placeId">Id of the place (store) to update</param>
        /// <param name="name">New (optional) name of the place</param>
        /// <param name="location">New (optional) location of the place</param>
        public void UpdatePlace(string placeId, string name, UpdateStoreRequest.StoreLocation location = null)
        {
            const string Function = "place";

            CheckNotAdmin();

            UpdateStoreRequest request = new UpdateStoreRequest() { name = name, location = location };

            HttpWebRequest webRequest = GetWebRequestAndSendRequest("PUT", Function, request, null, placeId);

            JSONresponse response = GetJSONresponse<JSONresponse>(webRequest); // this seems to be a 'GetPlaceResponse'!

            if (response == null)
                throw new Exception("No response.");

            // check the result codes:
            response.Check();
        }

        /// <summary>
        /// Delete place from Plot back-end server
        /// </summary>
        /// <param name="placeId">Id of the place to delete</param>
        public void DeletePlace(string placeId)
        {
            const string Function = "place";

            CheckNotAdmin();

            HttpWebRequest webRequest = GetWebRequest("DELETE", Function, null, placeId);

            JSONresponse response = GetJSONresponse<JSONresponse>(webRequest);

            if (response == null)
                throw new Exception("No response.");

            // check the result codes:
            response.Check();
        }

        /// <summary>
        /// Create a new store on the Plot back-end server
        /// </summary>
        /// <param name="store">Entity containing the v1 data of the store to create</param>
        /// <returns>ID (string) of the created v1 store</returns>
        public string CreateStore(CreateStoreV1 store)
        {
            const string FunctionAdmin = "createPlace";
            const string Function = "place";

            store.Check();

            CreateStoreRequest request = new CreateStoreRequest()
            {
                accountId = this.accountId,

                name = store.Name,
                address = store.Address,
                postalCode = store.PostalCode,
                city = store.City,
                country = store.Country.ToLower(),

                location = new CreateStoreRequest.StoreLocation()
                {
                    latitude = store.Latitude,
                    longitude = store.Longitude
                }
            };

            HttpWebRequest webRequest = GetWebRequestAndSendRequest("POST", isAdminConnection ? FunctionAdmin : Function, request);

            CreateStoreResponse response = GetJSONresponse<CreateStoreResponse>(webRequest);

            // check the result codes:
            response.Check();

            string ret = response.Result;

            if (string.IsNullOrWhiteSpace(ret))
                throw new Exception("No ID returned.");

            return ret;
        }

        /// <summary>
        /// Fetchs a page of notifications. Page-size is default (currently 25)
        /// </summary>
        /// <param name="storeId">Unique ID of the store from which the notifications should be retreived</param>
        /// <param name="start">Skip start notifications; 0 is default</param>
        /// <returns></returns>
        public IList<SelectNotification> GetNotifications(string storeId, int start = 0)
        {
            return FindNotifications(storeId, start, MaxOnePage);
        }

        /// <summary>
        /// Fetch all notifications from the Plot back-end server, optionally indication a maximum allowed number of records to fetch
        /// </summary>
        /// <param name="storeId">Unique ID of the store from which the notifications should be retreived</param>
        /// <param name="max">Maximum number of notifications to retreive. May be 'null' for ALL</param>
        /// <returns>A list of (generic) notification entities</returns>
        public IList<SelectNotification> GetAllNotifications(string storeId, int? max = null)
        {
            return FindNotifications(storeId, 0, max ?? MaxNone);
        }

        /// <summary>
        /// After a (any) fetch of notifications (for a store) this property contains the total number of notifications (for that store)
        /// </summary>
        public int TotalNotifications { get; private set; }

        private IList<SelectNotification> FindNotifications(string storeId, int start, int max) // max -> -1 = ALL (no max), 0 = one page (default size)
        {
            List<FindNotificationsResponse.NotificationResult.Notification> ret = new List<FindNotificationsResponse.NotificationResult.Notification>();

            while (true)
            {
                // get one page (from start)
                FindNotificationsResponse.NotificationResult res = FindNotifications(storeId, start);

                // save (grand) total (for this store) found (not thread safe)
                TotalNotifications = res.Total;

                if (res.Data.Count() == 0) // strange error?
                    break;

                // add them all
                ret.AddRange(res.Data);

                if (max == MaxOnePage)
                    break;

                if (max != MaxNone) // a 'real' max (> 0)
                    if (ret.Count() >= max)
                    {
                        // remove if too many
                        while (ret.Count() > max)
                            ret.Remove(ret.Last());
                        break;
                    }

                if (ret.Count() == res.Total) // no more available
                    break;

                // next:
                start += res.Data.Count();
            }
            return ret.Select(s => new SelectNotification()
            {
                ID = s.ID,
                StoreId = s.StoreId,
                State = s.State.AsEnum<State>(),
                Message = s.Message,
                Data = s.Data,
                MatchRange = s.MatchRange,
                Created = s.Created.AsDateTime(true).Value,
                Timespans = s.Timespans == null ? new Plot.Entities.SelectNotification.Timespan[] { } : s.Timespans.Select(t => new Plot.Entities.SelectNotification.Timespan() { Start = t.Start.AsDateTime(), End = t.End.AsDateTime() }).ToArray()
            }).ToList();
        }

        private FindNotificationsResponse.NotificationResult FindNotifications(string placeId, int start)
        {
            const string FunctionAdmin = "findNotifications";
            const string Function = "notification";
            const string Start = "start";
            const string PlaceId = "placeId";

            Parameters parameters = new Parameters() { { Start, start }, { PlaceId, placeId } };
            HttpWebRequest webRequest = GetWebRequest("GET", isAdminConnection ? FunctionAdmin : Function, parameters);

            FindNotificationsResponse response = GetJSONresponse<FindNotificationsResponse>(webRequest); // returns 500 if storeId unknown at Plot back-end server

            if (response == null)
                throw new Exception("No response.");

            // check the result codes:
            response.Check();

            FindNotificationsResponse.NotificationResult ret = response.Result ?? new FindNotificationsResponse.NotificationResult() { Total = 0 };

            if (ret.Data == null)
                ret.Data = new FindNotificationsResponse.NotificationResult.Notification[] { };

            return ret;
        }

        /// <summary>
        /// Create a new notification on the Plot back-end server
        /// </summary>
        /// <param name="notification">Entity containing the v1 data of the notification to create</param>
        /// <returns>ID (string) of the created v1 notification</returns>
        public string CreateNotification(CreateNotificationV1 notification)
        {
            const string FunctionAdmin = "createNotification";
            const string Function = "notification";

            notification.Check();

            CreateNotificationRequest request = new CreateNotificationRequest()
            {
                placeId = notification.StoreId,
                message = notification.Message,
                timespans = (notification.Timespans == null || notification.Timespans.Count() == 0) ? new CreateNotificationRequest.Timespan[] { } : notification.Timespans.Select(t => new CreateNotificationRequest.Timespan() { start = t.Start.AsString(), end = t.End.AsString() }).ToArray(),
                data = notification.Data,
                matchRange = notification.MatchRange,
                published = notification.Published
            };

            HttpWebRequest webRequest = GetWebRequestAndSendRequest("POST", isAdminConnection ? FunctionAdmin : Function, request);

            CreateNotificationResponse response = GetJSONresponse<CreateNotificationResponse>(webRequest);

            // check the result codes:
            response.Check();

            string ret = response.Result;

            if (string.IsNullOrWhiteSpace(ret))
                throw new Exception("No ID returned.");

            return ret;
        }

        /// <summary>
        /// Fetch a single notification from the Plot back-end server
        /// </summary>
        /// <param name="notificationId">Id og the notification to fetch</param>
        /// <returns>Entity containing the notification data</returns>
        public SelectNotification GetNotification(string notificationId)
        {
            CheckNotAdmin();

            const string Function = "notification";

            HttpWebRequest webRequest = GetWebRequest("GET", Function, null, notificationId);

            GetNotificationResponse response = GetJSONresponse<GetNotificationResponse>(webRequest);

            if (response == null)
                throw new Exception("No response.");

            // check the result codes:
            response.Check();

            SelectNotification ret = null;
            if (response.Result != null)
                ret = new SelectNotification()
                {
                    ID = response.Result.ID,
                    StoreId = response.Result.PlaceId,
                    State = response.Result.State.AsEnum<State>(),
                    Message = response.Result.Message,
                    Data = response.Result.Data,
                    MatchRange = response.Result.MatchRange,
                    Created = response.Result.Created.AsDateTime(true).Value,
                    Timespans = response.Result.Timespans == null ? new Plot.Entities.SelectNotification.Timespan[] { } : response.Result.Timespans.Select(t => new Plot.Entities.SelectNotification.Timespan() { Start = t.Start.AsDateTime(), End = t.End.AsDateTime() }).ToArray()
                };

            return ret;
        }

        /// <summary>
        /// Update a v1 notification
        /// </summary>
        /// <param name="notification">Entity containing the v1 data of the notification to update</param>
        public void UpdateNotification(UpdateNotificationV1 notification)
        {
            const string FunctionAdmin = "updateNotification";
            const string Function = "notification";

            if (notification.Published.HasValue && isAdminConnection)
                throw new Exception("Published cannot be updated through admin connection. Use " + (notification.Published.Value ? "PublishNotification" : "DeleteNotification"));

            notification.Check(isAdminConnection); // update with only ID and Puplished should be possible!

            UpdateNotificationRequest request = new UpdateNotificationRequest()
            {
                notificationId = notification.NotificationId,
                message = notification.Message,
                timespans = (notification.Timespans == null || notification.Timespans.Count() == 0) ? new UpdateNotificationRequest.Timespan[] { } : notification.Timespans.Select(t => new UpdateNotificationRequest.Timespan() { start = t.Start.AsString(), end = t.End.AsString() }).ToArray(),
                data = notification.Data,
                matchRange = notification.MatchRange,
                published = notification.Published
            };

            HttpWebRequest webRequest = GetWebRequestAndSendRequest("PUT", isAdminConnection ? FunctionAdmin : Function, request, null, isAdminConnection ? null : notification.NotificationId);

            UpdateNotificationResponse response = GetJSONresponse<UpdateNotificationResponse>(webRequest);

            response.Check();
        }

        /// <summary>
        /// Publish a v1 notification
        /// </summary>
        /// <param name="notificationId">Unique ID of the v1 notification to publish</param>
        public void PublishNotification(string notificationId)
        {
            const string Function = "publishNotification";

            CheckAdmin();

            if (string.IsNullOrWhiteSpace(notificationId))
                throw new EntityException("NotificationId", "NotificationId may not be empty");
            if (notificationId.Length != 32)
                throw new EntityException("NotificationId", "Length of NotificationId must be 32");

            HttpWebRequest webRequest = GetTrailedWebRequest("PUT", Function, notificationId);

            PublishNotificationResponse response = GetJSONresponse<PublishNotificationResponse>(webRequest);

            if (response == null)
                throw new Exception("No response.");

            // check the result codes:
            response.Check();
        }

        /// <summary>
        /// Delete a v1 notification
        /// </summary>
        /// <param name="notificationId">Unique ID of the v1 notification to delete</param>
        public void DeleteNotification(string notificationId)
        {
            const string FunctionAdmin = "deleteNotification";
            const string Function = "notification";

            if (string.IsNullOrWhiteSpace(notificationId))
                throw new EntityException("NotificationId", "NotificationId may not be empty");
            if (notificationId.Length != 32)
                throw new EntityException("NotificationId", "Length of NotificationId must be 32");

            HttpWebRequest webRequest = GetTrailedWebRequest("DELETE", isAdminConnection ? FunctionAdmin : Function, notificationId);

            DeleteNotificationResponse response = GetJSONresponse<DeleteNotificationResponse>(webRequest);

            if (response == null)
                throw new Exception("No response.");

            // check the result codes:
            response.Check();
        }

        /// <summary>
        /// Get v1 statistics about the notifications
        /// </summary>
        /// <param name="start">Start date (yyyy-mm-dd), default yesterday</param>
        /// <param name="end">End date (yyyy-mm-dd), default today</param>
        /// <returns>An array of statistics</returns>
        private GetStatisticsResponse.Statistic[] GetStatistics(DateTime? start, DateTime? end)
        {
            const string Function = "getStatistics";
            const string Start = "startdate";
            const string End = "enddate";

            CheckAdmin();

            Parameters parameters = new Parameters();
            if (start.HasValue)
                parameters.Add(Start, start.AsStringYMD());
            if (end.HasValue)
                parameters.Add(End, end.AsStringYMD());
            HttpWebRequest webRequest = GetWebRequest("GET", Function, parameters);

            GetStatisticsResponse response = GetJSONresponse<GetStatisticsResponse>(webRequest);

            if (response == null)
                throw new Exception("No response.");

            // check the result codes:
            response.Check();

            return response.Result ?? new GetStatisticsResponse.Statistic[] { };
        }

        /// <summary>
        /// Fetch all statistics from the Plot back-end server, optionally indicating a start and/or end date
        /// </summary>
        /// <param name="start">Start date (optional), default yesterday</param>
        /// <param name="end">End date (optional), default today</param>
        /// <returns>A list of (generic) statistic entities</returns>
        public IList<Statistic> FetchStatistics(DateTime? start = null, DateTime? end = null)
        {
            return GetStatistics(start, end).Select(s => new Statistic()
            {
                NotificationId = s.ID,
                Day = s.Date.AsDateTime(true).Value.ToUniversalTime().Date.AddHours(12),
                TimesSent = s.TimesSent,
                TimesViewed = s.TimesViewed
            }).ToList();
        }
    }

    /// <summary>
    /// Generic connection object to access the Plot back-end server
    /// </summary>
    public class Connection : ConnectionV1
    {
        /// <summary>
        /// Instantiate a connection object to access the Plot back-end server
        /// </summary>
        /// <param name="accountId">Unique ID of the account to use to access the Plot back-end server</param>
        /// <param name="privateKey">The (private) key supplied by Plot to access the account on the Plot back-end server</param>
        /// <param name="isAdminConnection">Indicate if the admin interface of the Plot back-end server shoud be used</param>
        public Connection(string accountId, string privateKey, Boolean isAdminConnection = false) // default is NO admin connection (new interface)
            : base(accountId, privateKey, isAdminConnection)
        {
        }
    }

}
