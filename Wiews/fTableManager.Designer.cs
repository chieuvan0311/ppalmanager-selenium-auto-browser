namespace PAYPAL
{
    partial class fTableManager
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gvAccountTable = new System.Windows.Forms.DataGridView();
            this.btnTableColumnSet = new System.Windows.Forms.Button();
            this.cbAccStatusList = new System.Windows.Forms.ComboBox();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.tbCount = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnCountAccount = new System.Windows.Forms.Button();
            this.btnAdmin = new System.Windows.Forms.Button();
            this.chromeOFF_Box = new System.Windows.Forms.CheckBox();
            this.btn_Add_New_Accounts = new System.Windows.Forms.Button();
            this.Temprory_Profile_BTN = new System.Windows.Forms.Button();
            this.checkBox_hold_on = new System.Windows.Forms.CheckBox();
            this.copy_cbox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAccountTable)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gvAccountTable);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(-3, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1357, 569);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // gvAccountTable
            // 
            this.gvAccountTable.AllowUserToAddRows = false;
            this.gvAccountTable.AllowUserToDeleteRows = false;
            this.gvAccountTable.AllowUserToResizeRows = false;
            this.gvAccountTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvAccountTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvAccountTable.BackgroundColor = System.Drawing.SystemColors.ScrollBar;
            this.gvAccountTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gvAccountTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAccountTable.Location = new System.Drawing.Point(2, 21);
            this.gvAccountTable.Name = "gvAccountTable";
            this.gvAccountTable.RowHeadersWidth = 15;
            this.gvAccountTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvAccountTable.Size = new System.Drawing.Size(1357, 548);
            this.gvAccountTable.TabIndex = 0;
            // 
            // btnTableColumnSet
            // 
            this.btnTableColumnSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTableColumnSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTableColumnSet.Location = new System.Drawing.Point(1215, 9);
            this.btnTableColumnSet.Name = "btnTableColumnSet";
            this.btnTableColumnSet.Size = new System.Drawing.Size(118, 31);
            this.btnTableColumnSet.TabIndex = 1;
            this.btnTableColumnSet.TabStop = false;
            this.btnTableColumnSet.Text = "Tùy chỉnh cột";
            this.btnTableColumnSet.UseVisualStyleBackColor = true;
            this.btnTableColumnSet.Click += new System.EventHandler(this.btnTableColumnSet_Click);
            // 
            // cbAccStatusList
            // 
            this.cbAccStatusList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAccStatusList.DropDownWidth = 144;
            this.cbAccStatusList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAccStatusList.FormattingEnabled = true;
            this.cbAccStatusList.ItemHeight = 16;
            this.cbAccStatusList.Items.AddRange(new object[] {
            "Tất cả",
            "Hoạt động",
            "Limit",
            "180d",
            "Set fail",
            "Sai thông tin",
            "Xác minh tk",
            "Chưa set",
            "Profiles"});
            this.cbAccStatusList.Location = new System.Drawing.Point(887, 14);
            this.cbAccStatusList.Name = "cbAccStatusList";
            this.cbAccStatusList.Size = new System.Drawing.Size(148, 24);
            this.cbAccStatusList.TabIndex = 2;
            this.cbAccStatusList.TabStop = false;
            this.cbAccStatusList.Text = "Hoạt động";
            this.cbAccStatusList.SelectedValueChanged += new System.EventHandler(this.cbAccStatusList_SelectedValueChanged);
            // 
            // tbSearch
            // 
            this.tbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSearch.Location = new System.Drawing.Point(502, 13);
            this.tbSearch.Multiline = true;
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(296, 25);
            this.tbSearch.TabIndex = 3;
            this.tbSearch.TabStop = false;
            // 
            // tbCount
            // 
            this.tbCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCount.Location = new System.Drawing.Point(1043, 14);
            this.tbCount.Multiline = true;
            this.tbCount.Name = "tbCount";
            this.tbCount.Size = new System.Drawing.Size(57, 24);
            this.tbCount.TabIndex = 4;
            this.tbCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(805, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(57, 26);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.TabStop = false;
            this.btnSearch.Text = "Tìm";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnCountAccount
            // 
            this.btnCountAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCountAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCountAccount.Location = new System.Drawing.Point(1121, 9);
            this.btnCountAccount.Name = "btnCountAccount";
            this.btnCountAccount.Size = new System.Drawing.Size(86, 31);
            this.btnCountAccount.TabIndex = 6;
            this.btnCountAccount.TabStop = false;
            this.btnCountAccount.Text = "Đếm Acc";
            this.btnCountAccount.UseVisualStyleBackColor = true;
            this.btnCountAccount.Click += new System.EventHandler(this.btnCountAccount_Click);
            // 
            // btnAdmin
            // 
            this.btnAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdmin.Location = new System.Drawing.Point(204, 9);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new System.Drawing.Size(74, 31);
            this.btnAdmin.TabIndex = 7;
            this.btnAdmin.TabStop = false;
            this.btnAdmin.Text = "Admin";
            this.btnAdmin.UseVisualStyleBackColor = true;
            this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click);
            // 
            // chromeOFF_Box
            // 
            this.chromeOFF_Box.AutoSize = true;
            this.chromeOFF_Box.Location = new System.Drawing.Point(19, 46);
            this.chromeOFF_Box.Name = "chromeOFF_Box";
            this.chromeOFF_Box.Size = new System.Drawing.Size(157, 17);
            this.chromeOFF_Box.TabIndex = 9;
            this.chromeOFF_Box.Text = "Tự tắt Chrome khi hết phiên";
            this.chromeOFF_Box.UseVisualStyleBackColor = true;
            // 
            // btn_Add_New_Accounts
            // 
            this.btn_Add_New_Accounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Add_New_Accounts.Location = new System.Drawing.Point(18, 9);
            this.btn_Add_New_Accounts.Name = "btn_Add_New_Accounts";
            this.btn_Add_New_Accounts.Size = new System.Drawing.Size(178, 31);
            this.btn_Add_New_Accounts.TabIndex = 10;
            this.btn_Add_New_Accounts.TabStop = false;
            this.btn_Add_New_Accounts.Text = "+ Thêm tài khoản mới";
            this.btn_Add_New_Accounts.UseVisualStyleBackColor = true;
            this.btn_Add_New_Accounts.Click += new System.EventHandler(this.btn_Add_New_Accounts_Click);
            // 
            // Temprory_Profile_BTN
            // 
            this.Temprory_Profile_BTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Temprory_Profile_BTN.Location = new System.Drawing.Point(286, 9);
            this.Temprory_Profile_BTN.Name = "Temprory_Profile_BTN";
            this.Temprory_Profile_BTN.Size = new System.Drawing.Size(93, 31);
            this.Temprory_Profile_BTN.TabIndex = 11;
            this.Temprory_Profile_BTN.TabStop = false;
            this.Temprory_Profile_BTN.Text = "Profile tạm";
            this.Temprory_Profile_BTN.UseVisualStyleBackColor = true;
            this.Temprory_Profile_BTN.Click += new System.EventHandler(this.Temprory_Profile_BTN_Click);
            // 
            // checkBox_hold_on
            // 
            this.checkBox_hold_on.AutoSize = true;
            this.checkBox_hold_on.Location = new System.Drawing.Point(205, 45);
            this.checkBox_hold_on.Name = "checkBox_hold_on";
            this.checkBox_hold_on.Size = new System.Drawing.Size(48, 17);
            this.checkBox_hold_on.TabIndex = 12;
            this.checkBox_hold_on.Text = "Hold";
            this.checkBox_hold_on.UseVisualStyleBackColor = true;
            // 
            // copy_cbox
            // 
            this.copy_cbox.AutoSize = true;
            this.copy_cbox.Location = new System.Drawing.Point(287, 46);
            this.copy_cbox.Name = "copy_cbox";
            this.copy_cbox.Size = new System.Drawing.Size(50, 17);
            this.copy_cbox.TabIndex = 13;
            this.copy_cbox.Text = "Copy";
            this.copy_cbox.UseVisualStyleBackColor = true;
            this.copy_cbox.CheckedChanged += new System.EventHandler(this.copy_cbox_CheckedChanged);
            // 
            // fTableManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1354, 637);
            this.Controls.Add(this.copy_cbox);
            this.Controls.Add(this.checkBox_hold_on);
            this.Controls.Add(this.Temprory_Profile_BTN);
            this.Controls.Add(this.btn_Add_New_Accounts);
            this.Controls.Add(this.chromeOFF_Box);
            this.Controls.Add(this.btnAdmin);
            this.Controls.Add(this.btnCountAccount);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.tbCount);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.cbAccStatusList);
            this.Controls.Add(this.btnTableColumnSet);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(1370, 650);
            this.Name = "fTableManager";
            this.Text = "Quản lý paypal";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvAccountTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnTableColumnSet;
        private System.Windows.Forms.ComboBox cbAccStatusList;
        private System.Windows.Forms.TextBox tbSearch;
        protected System.Windows.Forms.DataGridView gvAccountTable;
        private System.Windows.Forms.TextBox tbCount;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnCountAccount;
        private System.Windows.Forms.Button btnAdmin;
        private System.Windows.Forms.CheckBox chromeOFF_Box;
        private System.Windows.Forms.Button btn_Add_New_Accounts;
        private System.Windows.Forms.Button Temprory_Profile_BTN;
        private System.Windows.Forms.CheckBox checkBox_hold_on;
        private System.Windows.Forms.CheckBox copy_cbox;
    }
}

