using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Data.Common;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Plot;
using Plot.Entities;

namespace TestJSON
{
    public partial class mainForm : Form
    {
        private const string AccountID = "Your AccountID here";
        private const string PrivateKey = "Your PrivateKey here";

        public mainForm()
        {
            InitializeComponent();
        }

        private void AddLogLine(string s)
        {
            if (InvokeRequired)
                BeginInvoke((Action)(() => { logTextBox.Text += s + Environment.NewLine; }));
            else
                logTextBox.Text += s + Environment.NewLine;
        }

        private Connection NewConnection(Boolean isAdminConnection = false)
        {
            return new Connection(AccountID, PrivateKey, isAdminConnection);
        }

        // Stores / places
        private string storeID = null;

        private void findStoresButton_Click(object sender, EventArgs e)
        {
            using (Connection conn = NewConnection())
            {
                // get one page
                IList<SelectStore> result = conn.GetStores();

                if (result.Any() && storeID == null)
                    storeID = result.First().ID;

                AddLogLine(string.Format("Length of this 'page': {0} (of {1})", result.Count(), conn.TotalStores));
            }
        }

        private void findStoresSkipXButton_Click(object sender, EventArgs e)
        {
            using (Connection conn = NewConnection())
            {
                // get one page, skipping 100
                IList<SelectStore> result = conn.GetStores(100);

                if (result.Any() && storeID == null)
                    storeID = result.First().ID;

                AddLogLine(string.Format("Length of this 'page': {0} (of {1})", result.Count(), conn.TotalStores));
            }
        }

        private void findStoresMaxXButton_Click(object sender, EventArgs e)
        {
            using (Connection conn = NewConnection())
            {
                // get (max) 10
                IList<SelectStore> result = conn.GetAllStores(10);

                if (result.Any() && storeID == null)
                    storeID = result.First().ID;

                AddLogLine(string.Format("Fetched: {0} (of {1})", result.Count(), conn.TotalStores));
            }
        }

        private void findAllStoresButton_Click(object sender, EventArgs e)
        {
            using (Connection conn = NewConnection())
            {
                // get (really) ALL
                IList<SelectStore> result = conn.GetAllStores();

                if (result.Any() && storeID == null)
                    storeID = result.First().ID;

                AddLogLine(string.Format("Fetched: {0} (of {1})", result.Count(), conn.TotalStores));

                foreach (SelectStore s in result)
                {
                    AddLogLine(string.Format("{0} - {1}", s.ID, s.Name));
                }
            }
        }

        private void createStoreButton_Click(object sender, EventArgs e)
        {
            CreateStore store = new CreateStore()
            {
                Name = "Name of the store " + DateTime.Now.ToString(),
                Address = "Address of the store",
                PostalCode = "Postal code",
                City = "A place",
                Country = "NL",
                Latitude = 10.31,
                Longitude = 20.21
            };

            using (Connection conn = NewConnection())
            {
                string result = conn.CreateStore(store);

                if (storeID == null)
                    storeID = result;

                AddLogLine(string.Format("ID of the created store: {0}", result));
            }
        }

        private void getPlaceButton_Click(object sender, EventArgs e)
        {
            if (storeID == null)
                throw new Exception("No storeId/placeId known");

            using (Connection conn = NewConnection())
            {
                SelectStore result = conn.GetPlace(storeID);

                AddLogLine(string.Format("ID of the place: {0}; name: {1}", result.ID, result.Name));
            }
        }

        private void updatePlaceButton_Click(object sender, EventArgs e)
        {
            if (storeID == null)
                throw new Exception("No storeId/placeId known");

            using (Connection conn = NewConnection())
            {
                conn.UpdatePlace(storeID, "Include location " + DateTime.Now.ToString(), new Plot.Models.UpdateStoreRequest.StoreLocation() { longitude = 10.10, latitude = 5.5 });
                AddLogLine("Update done...");
            }
        }

        private void deletePlaceButton_Click(object sender, EventArgs e)
        {
            if (storeID == null)
                throw new Exception("No storeId/placeId known");

            using (Connection conn = NewConnection())
            {
                conn.DeletePlace(storeID);
            }

            storeID = null;
            AddLogLine("Deletion done...");
        }

        // Notifications

        private void findNotificationsButton_Click(object sender, EventArgs e)
        {
            if (storeID == null)
                throw new Exception("No storeId/placeId known");

            using (Connection conn = NewConnection())
            {
                IList<SelectNotification> result = conn.GetNotifications(storeID);

                if (result.Any() && notificationId == null)
                    notificationId = result.First().ID;

                AddLogLine(string.Format("Length of this 'page': {0} (of {1})", result.Count(), conn.TotalNotifications));

                // "new" or "published" or "unpublished"

                int nw = result.Count(n => n.State == State.New);
                if (nw != 0)
                    AddLogLine(string.Format("New: {0}", nw));

                int p = result.Count(n => n.State == State.Published);
                if (p != 0)
                    AddLogLine(string.Format("Published: {0}", p));

                int u = result.Count(n => n.State == State.Unpublished);
                if (u != 0)
                    AddLogLine(string.Format("Unpublished: {0}", u));
            }
        }

        private void findNotificationsSkipXButton_Click(object sender, EventArgs e)
        {
            if (storeID == null)
                throw new Exception("No storeId/placeId known");

            using (Connection conn = NewConnection())
            {
                IList<SelectNotification> result = conn.GetNotifications(storeID, 2);

                if (result.Any() && notificationId == null)
                    notificationId = result.First().ID;

                AddLogLine(string.Format("Length of this 'page': {0} (of {1})", result.Count(), conn.TotalNotifications));

                // "new" or "published" or "unpublished"

                int nw = result.Count(n => n.State == State.New);
                if (nw != 0)
                    AddLogLine(string.Format("New: {0}", nw));

                int p = result.Count(n => n.State == State.Published);
                if (p != 0)
                    AddLogLine(string.Format("Published: {0}", p));

                int u = result.Count(n => n.State == State.Unpublished);
                if (u != 0)
                    AddLogLine(string.Format("Unpublished: {0}", u));
            }
        }

        private void findNotificationsMaxXButton_Click(object sender, EventArgs e)
        {
            if (storeID == null)
                throw new Exception("No storeId/placeId known");

            using (Connection conn = NewConnection())
            {
                IList<SelectNotification> result = conn.GetAllNotifications(storeID, 3);

                if (result.Any() && notificationId == null)
                    notificationId = result.First().ID;

                AddLogLine(string.Format("Fetched: {0} (of {1})", result.Count(), conn.TotalNotifications));

                // "new" or "published" or "unpublished"

                int nw = result.Count(n => n.State == State.New);
                if (nw != 0)
                    AddLogLine(string.Format("New: {0}", nw));

                int p = result.Count(n => n.State == State.Published);
                if (p != 0)
                    AddLogLine(string.Format("Published: {0}", p));

                int u = result.Count(n => n.State == State.Unpublished);
                if (u != 0)
                    AddLogLine(string.Format("Unpublished: {0}", u));
            }
        }

        private void findAllNotificationButton_Click(object sender, EventArgs e)
        {
            if (storeID == null)
                throw new Exception("No storeId/placeId known");

            using (Connection conn = NewConnection())
            {
                IList<SelectNotification> result = conn.GetAllNotifications(storeID);

                if (result.Any() && notificationId == null)
                    notificationId = result.First().ID;

                AddLogLine(string.Format("Fetched: {0} (of {1})", result.Count(), conn.TotalNotifications));

                // "new" or "published" or "unpublished"

                int nw = result.Count(n => n.State == State.New);
                if (nw != 0)
                    AddLogLine(string.Format("New: {0}", nw));

                int p = result.Count(n => n.State == State.Published);
                if (p != 0)
                    AddLogLine(string.Format("Published: {0}", p));

                int u = result.Count(n => n.State == State.Unpublished);
                if (u != 0)
                    AddLogLine(string.Format("Unpublished: {0}", u));

                foreach(var n in result)
                    AddLogLine(n.ID + " - " + n.Message + " - " + n.State);
            }
        }

        private string notificationId = null;

        private void createNotificationButton_Click(object sender, EventArgs e)
        {
            if (storeID == null)
                throw new Exception("No storeId/placeId known");

            CreateNotification notification = new CreateNotification()
            {
                StoreId = storeID,

                Message = "Unicode message € î ï è ç blâÿãh - " + DateTime.Now.ToString(),
                Data = "{\"dataId\" : 10}",
                MatchRange = 250,

                Timespans = null // new CreateNotificationV1.Timespan[] { new CreateNotificationV1.Timespan { Start = null, End = null } }
            };

            using (Connection conn = NewConnection())
            {
                string result = conn.CreateNotification(notification);

                if (notificationId == null)
                    notificationId = result;

                AddLogLine(string.Format("ID of the created notification: {0}", result));
            }
        }

        private void getNotificationButton_Click(object sender, EventArgs e)
        {
            if (notificationId == null)
                throw new Exception("No notificationId known");

            using (Connection conn = NewConnection())
            {
                SelectNotification result = conn.GetNotification(notificationId);

                AddLogLine(string.Format("ID of the place: {0}; message: {1}; data: {2}", result.ID, result.Message, result.Data));
            }
        }

        private void updateNotificationButton_Click(object sender, EventArgs e)
        {
            if (notificationId == null)
                throw new Exception("No notificationId known");

            UpdateNotification notification = new UpdateNotification()
            {
                NotificationId = notificationId,

                Message = "New messgae € î ï è ç blâÿãh - " + DateTime.Now.ToString(),
                Data = "{\"dataId\" : 234}",
                MatchRange = 202,
                //Published = true,

                Timespans = new CreateNotificationV1.Timespan[] { new CreateNotificationV1.Timespan { Start = null, End = DateTime.UtcNow.AddDays(3) } }
                //Timespans = new CreateNotificationV1.Timespan[] { new CreateNotificationV1.Timespan { Start = DateTime.UtcNow, End = DateTime.UtcNow.AddDays(3) } }
            };

            using (Connection conn = NewConnection())
            {
                conn.UpdateNotification(notification);
            }
            AddLogLine(string.Format("Updated: {0}", notification.Data));
        }

        private void publishNotificationNonAdminButton_Click(object sender, EventArgs e)
        {
            if (notificationId == null)
                throw new Exception("No notificationId known");

            UpdateNotification notification = new UpdateNotification()
            {
                NotificationId = notificationId,
                Published = true
            };

            using (Connection conn = NewConnection())
            {
                conn.UpdateNotification(notification);
            }
            AddLogLine(string.Format("Published (using update): {0}", notification.Data));
        }

        private void deleteNotificationButton_Click(object sender, EventArgs e)
        {
            if (notificationId == null)
                throw new Exception("No notificationId known");

            using (Connection conn = NewConnection())
            {
                conn.DeleteNotification(notificationId);
            }

            AddLogLine(string.Format("Deleted: {0}", notificationId));
            notificationId = null;
        }

        private void publishNotificationButton_Click(object sender, EventArgs e)
        {
            if (notificationId == null)
                throw new Exception("No notificationId known");

            using (Connection conn = NewConnection(true))
            {
                conn.PublishNotification(notificationId);
            }
            AddLogLine(string.Format("Published: {0}", notificationId));
        }
        
        private void getStatisticsButton_Click(object sender, EventArgs e)
        {
            DateTime? start = DateTime.Now.AddDays(-7); // new DateTime(2013, 4, 28);
            DateTime? end = DateTime.Now;

            IList<Statistic> res = null;
            using (Connection conn = NewConnection(true))
            {
                res = conn.FetchStatistics(start, end);
            }

            AddLogLine(string.Format("Fetched: {0}", res.Count()));

            foreach (Statistic s in res)
            {
                AddLogLine(string.Format("{0} - {1} - {2} - {3}", s.Day, s.NotificationId, s.TimesSent, s.TimesViewed));
            }
        }
    }
}
