namespace TestJSON
{
    partial class mainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.findStoresButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.publishNotificationNonAdminButton = new System.Windows.Forms.Button();
            this.getNotificationButton = new System.Windows.Forms.Button();
            this.deletePlaceButton = new System.Windows.Forms.Button();
            this.updatePlaceButton = new System.Windows.Forms.Button();
            this.getPlaceButton = new System.Windows.Forms.Button();
            this.getStatisticsButton = new System.Windows.Forms.Button();
            this.deleteNotificationButton = new System.Windows.Forms.Button();
            this.publishNotificationButton = new System.Windows.Forms.Button();
            this.updateNotificationButton = new System.Windows.Forms.Button();
            this.createNotificationButton = new System.Windows.Forms.Button();
            this.findAllNotificationButton = new System.Windows.Forms.Button();
            this.findNotificationsMaxXButton = new System.Windows.Forms.Button();
            this.findNotificationsSkipXButton = new System.Windows.Forms.Button();
            this.findAllStoresButton = new System.Windows.Forms.Button();
            this.findStoresMaxXButton = new System.Windows.Forms.Button();
            this.findStoresSkipXButton = new System.Windows.Forms.Button();
            this.findNotificationsButton = new System.Windows.Forms.Button();
            this.createStoreButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // findStoresButton
            // 
            this.findStoresButton.Location = new System.Drawing.Point(12, 12);
            this.findStoresButton.Name = "findStoresButton";
            this.findStoresButton.Size = new System.Drawing.Size(196, 23);
            this.findStoresButton.TabIndex = 0;
            this.findStoresButton.Text = "Find stores (one page)";
            this.findStoresButton.UseVisualStyleBackColor = true;
            this.findStoresButton.Click += new System.EventHandler(this.findStoresButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.publishNotificationNonAdminButton);
            this.panel1.Controls.Add(this.getNotificationButton);
            this.panel1.Controls.Add(this.deletePlaceButton);
            this.panel1.Controls.Add(this.updatePlaceButton);
            this.panel1.Controls.Add(this.getPlaceButton);
            this.panel1.Controls.Add(this.getStatisticsButton);
            this.panel1.Controls.Add(this.deleteNotificationButton);
            this.panel1.Controls.Add(this.publishNotificationButton);
            this.panel1.Controls.Add(this.updateNotificationButton);
            this.panel1.Controls.Add(this.createNotificationButton);
            this.panel1.Controls.Add(this.findAllNotificationButton);
            this.panel1.Controls.Add(this.findNotificationsMaxXButton);
            this.panel1.Controls.Add(this.findNotificationsSkipXButton);
            this.panel1.Controls.Add(this.findAllStoresButton);
            this.panel1.Controls.Add(this.findStoresMaxXButton);
            this.panel1.Controls.Add(this.findStoresSkipXButton);
            this.panel1.Controls.Add(this.findNotificationsButton);
            this.panel1.Controls.Add(this.createStoreButton);
            this.panel1.Controls.Add(this.findStoresButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(982, 277);
            this.panel1.TabIndex = 2;
            // 
            // publishNotificationNonAdminButton
            // 
            this.publishNotificationNonAdminButton.Location = new System.Drawing.Point(433, 215);
            this.publishNotificationNonAdminButton.Name = "publishNotificationNonAdminButton";
            this.publishNotificationNonAdminButton.Size = new System.Drawing.Size(125, 23);
            this.publishNotificationNonAdminButton.TabIndex = 18;
            this.publishNotificationNonAdminButton.Text = "Publish notification";
            this.publishNotificationNonAdminButton.UseVisualStyleBackColor = true;
            this.publishNotificationNonAdminButton.Click += new System.EventHandler(this.publishNotificationNonAdminButton_Click);
            // 
            // getNotificationButton
            // 
            this.getNotificationButton.Location = new System.Drawing.Point(433, 157);
            this.getNotificationButton.Name = "getNotificationButton";
            this.getNotificationButton.Size = new System.Drawing.Size(133, 23);
            this.getNotificationButton.TabIndex = 17;
            this.getNotificationButton.Text = "Get notification";
            this.getNotificationButton.UseVisualStyleBackColor = true;
            this.getNotificationButton.Click += new System.EventHandler(this.getNotificationButton_Click);
            // 
            // deletePlaceButton
            // 
            this.deletePlaceButton.Location = new System.Drawing.Point(12, 215);
            this.deletePlaceButton.Name = "deletePlaceButton";
            this.deletePlaceButton.Size = new System.Drawing.Size(196, 23);
            this.deletePlaceButton.TabIndex = 16;
            this.deletePlaceButton.Text = "Delete place";
            this.deletePlaceButton.UseVisualStyleBackColor = true;
            this.deletePlaceButton.Click += new System.EventHandler(this.deletePlaceButton_Click);
            // 
            // updatePlaceButton
            // 
            this.updatePlaceButton.Location = new System.Drawing.Point(12, 186);
            this.updatePlaceButton.Name = "updatePlaceButton";
            this.updatePlaceButton.Size = new System.Drawing.Size(196, 23);
            this.updatePlaceButton.TabIndex = 15;
            this.updatePlaceButton.Text = "Update place";
            this.updatePlaceButton.UseVisualStyleBackColor = true;
            this.updatePlaceButton.Click += new System.EventHandler(this.updatePlaceButton_Click);
            // 
            // getPlaceButton
            // 
            this.getPlaceButton.Location = new System.Drawing.Point(12, 157);
            this.getPlaceButton.Name = "getPlaceButton";
            this.getPlaceButton.Size = new System.Drawing.Size(196, 23);
            this.getPlaceButton.TabIndex = 14;
            this.getPlaceButton.Text = "Get place";
            this.getPlaceButton.UseVisualStyleBackColor = true;
            this.getPlaceButton.Click += new System.EventHandler(this.getPlaceButton_Click);
            // 
            // getStatisticsButton
            // 
            this.getStatisticsButton.Location = new System.Drawing.Point(710, 12);
            this.getStatisticsButton.Name = "getStatisticsButton";
            this.getStatisticsButton.Size = new System.Drawing.Size(125, 23);
            this.getStatisticsButton.TabIndex = 13;
            this.getStatisticsButton.Text = "Get statistics";
            this.getStatisticsButton.UseVisualStyleBackColor = true;
            this.getStatisticsButton.Click += new System.EventHandler(this.getStatisticsButton_Click);
            // 
            // deleteNotificationButton
            // 
            this.deleteNotificationButton.Location = new System.Drawing.Point(433, 244);
            this.deleteNotificationButton.Name = "deleteNotificationButton";
            this.deleteNotificationButton.Size = new System.Drawing.Size(133, 23);
            this.deleteNotificationButton.TabIndex = 12;
            this.deleteNotificationButton.Text = "Delete notification";
            this.deleteNotificationButton.UseVisualStyleBackColor = true;
            this.deleteNotificationButton.Click += new System.EventHandler(this.deleteNotificationButton_Click);
            // 
            // publishNotificationButton
            // 
            this.publishNotificationButton.Location = new System.Drawing.Point(572, 186);
            this.publishNotificationButton.Name = "publishNotificationButton";
            this.publishNotificationButton.Size = new System.Drawing.Size(125, 23);
            this.publishNotificationButton.TabIndex = 11;
            this.publishNotificationButton.Text = "Publish notification";
            this.publishNotificationButton.UseVisualStyleBackColor = true;
            this.publishNotificationButton.Click += new System.EventHandler(this.publishNotificationButton_Click);
            // 
            // updateNotificationButton
            // 
            this.updateNotificationButton.Location = new System.Drawing.Point(433, 186);
            this.updateNotificationButton.Name = "updateNotificationButton";
            this.updateNotificationButton.Size = new System.Drawing.Size(133, 23);
            this.updateNotificationButton.TabIndex = 10;
            this.updateNotificationButton.Text = "Update notification";
            this.updateNotificationButton.UseVisualStyleBackColor = true;
            this.updateNotificationButton.Click += new System.EventHandler(this.updateNotificationButton_Click);
            // 
            // createNotificationButton
            // 
            this.createNotificationButton.Location = new System.Drawing.Point(433, 128);
            this.createNotificationButton.Name = "createNotificationButton";
            this.createNotificationButton.Size = new System.Drawing.Size(133, 23);
            this.createNotificationButton.TabIndex = 9;
            this.createNotificationButton.Text = "Create notification";
            this.createNotificationButton.UseVisualStyleBackColor = true;
            this.createNotificationButton.Click += new System.EventHandler(this.createNotificationButton_Click);
            // 
            // findAllNotificationButton
            // 
            this.findAllNotificationButton.Location = new System.Drawing.Point(214, 99);
            this.findAllNotificationButton.Name = "findAllNotificationButton";
            this.findAllNotificationButton.Size = new System.Drawing.Size(213, 23);
            this.findAllNotificationButton.TabIndex = 8;
            this.findAllNotificationButton.Text = "Find ALL notifications";
            this.findAllNotificationButton.UseVisualStyleBackColor = true;
            this.findAllNotificationButton.Click += new System.EventHandler(this.findAllNotificationButton_Click);
            // 
            // findNotificationsMaxXButton
            // 
            this.findNotificationsMaxXButton.Location = new System.Drawing.Point(214, 70);
            this.findNotificationsMaxXButton.Name = "findNotificationsMaxXButton";
            this.findNotificationsMaxXButton.Size = new System.Drawing.Size(213, 23);
            this.findNotificationsMaxXButton.TabIndex = 7;
            this.findNotificationsMaxXButton.Text = "Find ALL notifications (max 3)";
            this.findNotificationsMaxXButton.UseVisualStyleBackColor = true;
            this.findNotificationsMaxXButton.Click += new System.EventHandler(this.findNotificationsMaxXButton_Click);
            // 
            // findNotificationsSkipXButton
            // 
            this.findNotificationsSkipXButton.Location = new System.Drawing.Point(214, 41);
            this.findNotificationsSkipXButton.Name = "findNotificationsSkipXButton";
            this.findNotificationsSkipXButton.Size = new System.Drawing.Size(213, 23);
            this.findNotificationsSkipXButton.TabIndex = 6;
            this.findNotificationsSkipXButton.Text = "Find notifications (one page, skipping 2)";
            this.findNotificationsSkipXButton.UseVisualStyleBackColor = true;
            this.findNotificationsSkipXButton.Click += new System.EventHandler(this.findNotificationsSkipXButton_Click);
            // 
            // findAllStoresButton
            // 
            this.findAllStoresButton.Location = new System.Drawing.Point(12, 99);
            this.findAllStoresButton.Name = "findAllStoresButton";
            this.findAllStoresButton.Size = new System.Drawing.Size(196, 23);
            this.findAllStoresButton.TabIndex = 5;
            this.findAllStoresButton.Text = "Find ALL stores";
            this.findAllStoresButton.UseVisualStyleBackColor = true;
            this.findAllStoresButton.Click += new System.EventHandler(this.findAllStoresButton_Click);
            // 
            // findStoresMaxXButton
            // 
            this.findStoresMaxXButton.Location = new System.Drawing.Point(12, 70);
            this.findStoresMaxXButton.Name = "findStoresMaxXButton";
            this.findStoresMaxXButton.Size = new System.Drawing.Size(196, 23);
            this.findStoresMaxXButton.TabIndex = 4;
            this.findStoresMaxXButton.Text = "Find ALL stores (max 10)";
            this.findStoresMaxXButton.UseVisualStyleBackColor = true;
            this.findStoresMaxXButton.Click += new System.EventHandler(this.findStoresMaxXButton_Click);
            // 
            // findStoresSkipXButton
            // 
            this.findStoresSkipXButton.Location = new System.Drawing.Point(12, 41);
            this.findStoresSkipXButton.Name = "findStoresSkipXButton";
            this.findStoresSkipXButton.Size = new System.Drawing.Size(196, 23);
            this.findStoresSkipXButton.TabIndex = 3;
            this.findStoresSkipXButton.Text = "Find stores (one page skipping 100)";
            this.findStoresSkipXButton.UseVisualStyleBackColor = true;
            this.findStoresSkipXButton.Click += new System.EventHandler(this.findStoresSkipXButton_Click);
            // 
            // findNotificationsButton
            // 
            this.findNotificationsButton.Location = new System.Drawing.Point(214, 12);
            this.findNotificationsButton.Name = "findNotificationsButton";
            this.findNotificationsButton.Size = new System.Drawing.Size(213, 23);
            this.findNotificationsButton.TabIndex = 2;
            this.findNotificationsButton.Text = "Find notifications (one page)";
            this.findNotificationsButton.UseVisualStyleBackColor = true;
            this.findNotificationsButton.Click += new System.EventHandler(this.findNotificationsButton_Click);
            // 
            // createStoreButton
            // 
            this.createStoreButton.Location = new System.Drawing.Point(12, 128);
            this.createStoreButton.Name = "createStoreButton";
            this.createStoreButton.Size = new System.Drawing.Size(196, 23);
            this.createStoreButton.TabIndex = 1;
            this.createStoreButton.Text = "Create store/place";
            this.createStoreButton.UseVisualStyleBackColor = true;
            this.createStoreButton.Click += new System.EventHandler(this.createStoreButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(996, 566);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(988, 540);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Test";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(982, 534);
            this.panel3.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.logTextBox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 277);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(982, 257);
            this.panel2.TabIndex = 4;
            // 
            // logTextBox
            // 
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Location = new System.Drawing.Point(0, 0);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(982, 257);
            this.logTextBox.TabIndex = 1;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(996, 566);
            this.Controls.Add(this.tabControl1);
            this.Name = "mainForm";
            this.Text = "Plot JSON library test project";
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button findStoresButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button createStoreButton;
        private System.Windows.Forms.Button findNotificationsButton;
        private System.Windows.Forms.Button findStoresSkipXButton;
        private System.Windows.Forms.Button findAllStoresButton;
        private System.Windows.Forms.Button findStoresMaxXButton;
        private System.Windows.Forms.Button findNotificationsMaxXButton;
        private System.Windows.Forms.Button findNotificationsSkipXButton;
        private System.Windows.Forms.Button findAllNotificationButton;
        private System.Windows.Forms.Button createNotificationButton;
        private System.Windows.Forms.Button updateNotificationButton;
        private System.Windows.Forms.Button publishNotificationButton;
        private System.Windows.Forms.Button deleteNotificationButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Button getStatisticsButton;
        private System.Windows.Forms.Button getPlaceButton;
        private System.Windows.Forms.Button updatePlaceButton;
        private System.Windows.Forms.Button deletePlaceButton;
        private System.Windows.Forms.Button getNotificationButton;
        private System.Windows.Forms.Button publishNotificationNonAdminButton;
    }
}

