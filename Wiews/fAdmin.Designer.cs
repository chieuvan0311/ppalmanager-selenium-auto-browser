namespace PAYPAL.Wiews
{
    partial class fAdmin
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
            this.tcAccountTable = new System.Windows.Forms.TabControl();
            this.tabAdmin = new System.Windows.Forms.TabPage();
            this.gvAccountTable = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.grv_acounts_count = new System.Windows.Forms.DataGridView();
            this.cbAccStatusList = new System.Windows.Forms.ComboBox();
            this.btnTableColumnSet = new System.Windows.Forms.Button();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.chromeOFF_Box = new System.Windows.Forms.CheckBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btn_add_new_accounts = new System.Windows.Forms.Button();
            this.delete_account_BTN = new System.Windows.Forms.Button();
            this.tb_count_counts_by_group = new System.Windows.Forms.TextBox();
            this.back_to_fmanager = new System.Windows.Forms.Button();
            this.copy_cbox = new System.Windows.Forms.CheckBox();
            this.checkBox_hold_on = new System.Windows.Forms.CheckBox();
            this.tem_profile_BTN = new System.Windows.Forms.Button();
            this.tcAccountTable.SuspendLayout();
            this.tabAdmin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAccountTable)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grv_acounts_count)).BeginInit();
            this.SuspendLayout();
            // 
            // tcAccountTable
            // 
            this.tcAccountTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcAccountTable.Controls.Add(this.tabAdmin);
            this.tcAccountTable.Controls.Add(this.tabPage2);
            this.tcAccountTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcAccountTable.Location = new System.Drawing.Point(-4, 74);
            this.tcAccountTable.Name = "tcAccountTable";
            this.tcAccountTable.SelectedIndex = 0;
            this.tcAccountTable.Size = new System.Drawing.Size(1357, 544);
            this.tcAccountTable.TabIndex = 3;
            // 
            // tabAdmin
            // 
            this.tabAdmin.Controls.Add(this.gvAccountTable);
            this.tabAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabAdmin.Location = new System.Drawing.Point(4, 25);
            this.tabAdmin.Name = "tabAdmin";
            this.tabAdmin.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdmin.Size = new System.Drawing.Size(1349, 515);
            this.tabAdmin.TabIndex = 0;
            this.tabAdmin.Text = "Danh sách tài khoản";
            this.tabAdmin.UseVisualStyleBackColor = true;
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
            this.gvAccountTable.Location = new System.Drawing.Point(0, 0);
            this.gvAccountTable.Name = "gvAccountTable";
            this.gvAccountTable.RowHeadersWidth = 15;
            this.gvAccountTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvAccountTable.Size = new System.Drawing.Size(1353, 514);
            this.gvAccountTable.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.grv_acounts_count);
            this.tabPage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1349, 515);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tổng quan Accounts";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // grv_acounts_count
            // 
            this.grv_acounts_count.AllowUserToAddRows = false;
            this.grv_acounts_count.AllowUserToDeleteRows = false;
            this.grv_acounts_count.AllowUserToResizeRows = false;
            this.grv_acounts_count.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grv_acounts_count.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grv_acounts_count.BackgroundColor = System.Drawing.SystemColors.ScrollBar;
            this.grv_acounts_count.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grv_acounts_count.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grv_acounts_count.Location = new System.Drawing.Point(0, 0);
            this.grv_acounts_count.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.grv_acounts_count.Name = "grv_acounts_count";
            this.grv_acounts_count.RowHeadersWidth = 15;
            this.grv_acounts_count.Size = new System.Drawing.Size(1349, 512);
            this.grv_acounts_count.TabIndex = 0;
            this.grv_acounts_count.TabStop = false;
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
            "Acc mới",
            "Acc cũ",
            "Hoạt động",
            "Limit",
            "180d",
            "Set fail",
            "Sai thông tin",
            "Xác minh tk",
            "Chưa set",
            "Profiles"});
            this.cbAccStatusList.Location = new System.Drawing.Point(964, 11);
            this.cbAccStatusList.Name = "cbAccStatusList";
            this.cbAccStatusList.Size = new System.Drawing.Size(153, 24);
            this.cbAccStatusList.TabIndex = 2;
            this.cbAccStatusList.TabStop = false;
            this.cbAccStatusList.Text = "Acc mới";
            this.cbAccStatusList.SelectedValueChanged += new System.EventHandler(this.cbAccStatusList_SelectedValueChanged);
            // 
            // btnTableColumnSet
            // 
            this.btnTableColumnSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTableColumnSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTableColumnSet.Location = new System.Drawing.Point(1208, 43);
            this.btnTableColumnSet.Name = "btnTableColumnSet";
            this.btnTableColumnSet.Size = new System.Drawing.Size(117, 29);
            this.btnTableColumnSet.TabIndex = 1;
            this.btnTableColumnSet.TabStop = false;
            this.btnTableColumnSet.Text = "Tùy chỉnh cột";
            this.btnTableColumnSet.UseVisualStyleBackColor = true;
            this.btnTableColumnSet.Click += new System.EventHandler(this.btnTableColumnSet_Click);
            // 
            // tbSearch
            // 
            this.tbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSearch.Location = new System.Drawing.Point(562, 11);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(299, 24);
            this.tbSearch.TabIndex = 3;
            this.tbSearch.TabStop = false;
            // 
            // chromeOFF_Box
            // 
            this.chromeOFF_Box.AutoSize = true;
            this.chromeOFF_Box.Checked = true;
            this.chromeOFF_Box.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chromeOFF_Box.Location = new System.Drawing.Point(17, 46);
            this.chromeOFF_Box.Name = "chromeOFF_Box";
            this.chromeOFF_Box.Size = new System.Drawing.Size(157, 17);
            this.chromeOFF_Box.TabIndex = 4;
            this.chromeOFF_Box.Text = "Tự tắt Chrome khi hết phiên";
            this.chromeOFF_Box.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(869, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(60, 26);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Tìm";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btn_add_new_accounts
            // 
            this.btn_add_new_accounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_add_new_accounts.Location = new System.Drawing.Point(16, 9);
            this.btn_add_new_accounts.Name = "btn_add_new_accounts";
            this.btn_add_new_accounts.Size = new System.Drawing.Size(169, 29);
            this.btn_add_new_accounts.TabIndex = 6;
            this.btn_add_new_accounts.TabStop = false;
            this.btn_add_new_accounts.Text = "+ Thêm tài khoản mới";
            this.btn_add_new_accounts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_add_new_accounts.UseVisualStyleBackColor = true;
            this.btn_add_new_accounts.Click += new System.EventHandler(this.btn_add_new_accounts_Click);
            // 
            // delete_account_BTN
            // 
            this.delete_account_BTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delete_account_BTN.Location = new System.Drawing.Point(195, 9);
            this.delete_account_BTN.Name = "delete_account_BTN";
            this.delete_account_BTN.Size = new System.Drawing.Size(125, 29);
            this.delete_account_BTN.TabIndex = 7;
            this.delete_account_BTN.TabStop = false;
            this.delete_account_BTN.Text = "Xóa tài khoản";
            this.delete_account_BTN.UseVisualStyleBackColor = true;
            this.delete_account_BTN.Click += new System.EventHandler(this.delete_account_BTN_Click);
            // 
            // tb_count_counts_by_group
            // 
            this.tb_count_counts_by_group.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_count_counts_by_group.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_count_counts_by_group.Location = new System.Drawing.Point(1126, 11);
            this.tb_count_counts_by_group.Name = "tb_count_counts_by_group";
            this.tb_count_counts_by_group.Size = new System.Drawing.Size(66, 24);
            this.tb_count_counts_by_group.TabIndex = 8;
            this.tb_count_counts_by_group.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // back_to_fmanager
            // 
            this.back_to_fmanager.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.back_to_fmanager.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.back_to_fmanager.Location = new System.Drawing.Point(1208, 9);
            this.back_to_fmanager.Name = "back_to_fmanager";
            this.back_to_fmanager.Size = new System.Drawing.Size(117, 28);
            this.back_to_fmanager.TabIndex = 9;
            this.back_to_fmanager.Text = "Trở về QLPP";
            this.back_to_fmanager.UseVisualStyleBackColor = true;
            this.back_to_fmanager.Click += new System.EventHandler(this.back_to_fmanager_Click);
            // 
            // copy_cbox
            // 
            this.copy_cbox.AutoSize = true;
            this.copy_cbox.Location = new System.Drawing.Point(331, 46);
            this.copy_cbox.Name = "copy_cbox";
            this.copy_cbox.Size = new System.Drawing.Size(50, 17);
            this.copy_cbox.TabIndex = 10;
            this.copy_cbox.Text = "Copy";
            this.copy_cbox.UseVisualStyleBackColor = true;
            this.copy_cbox.CheckedChanged += new System.EventHandler(this.copy_cbox_CheckedChanged);
            // 
            // checkBox_hold_on
            // 
            this.checkBox_hold_on.AutoSize = true;
            this.checkBox_hold_on.Location = new System.Drawing.Point(196, 46);
            this.checkBox_hold_on.Name = "checkBox_hold_on";
            this.checkBox_hold_on.Size = new System.Drawing.Size(48, 17);
            this.checkBox_hold_on.TabIndex = 11;
            this.checkBox_hold_on.Text = "Hold";
            this.checkBox_hold_on.UseVisualStyleBackColor = true;
            // 
            // tem_profile_BTN
            // 
            this.tem_profile_BTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tem_profile_BTN.Location = new System.Drawing.Point(330, 9);
            this.tem_profile_BTN.Name = "tem_profile_BTN";
            this.tem_profile_BTN.Size = new System.Drawing.Size(96, 29);
            this.tem_profile_BTN.TabIndex = 12;
            this.tem_profile_BTN.TabStop = false;
            this.tem_profile_BTN.Text = "Profile tạm";
            this.tem_profile_BTN.UseVisualStyleBackColor = true;
            this.tem_profile_BTN.Click += new System.EventHandler(this.tem_profile_BTN_Click);
            // 
            // fAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1349, 611);
            this.Controls.Add(this.tem_profile_BTN);
            this.Controls.Add(this.checkBox_hold_on);
            this.Controls.Add(this.copy_cbox);
            this.Controls.Add(this.back_to_fmanager);
            this.Controls.Add(this.tb_count_counts_by_group);
            this.Controls.Add(this.delete_account_BTN);
            this.Controls.Add(this.btn_add_new_accounts);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.chromeOFF_Box);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.btnTableColumnSet);
            this.Controls.Add(this.cbAccStatusList);
            this.Controls.Add(this.tcAccountTable);
            this.MinimumSize = new System.Drawing.Size(1365, 650);
            this.Name = "fAdmin";
            this.Text = "Admin";
            this.tcAccountTable.ResumeLayout(false);
            this.tabAdmin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvAccountTable)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grv_acounts_count)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tcAccountTable;
        private System.Windows.Forms.TabPage tabAdmin;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox cbAccStatusList;
        private System.Windows.Forms.Button btnTableColumnSet;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.DataGridView gvAccountTable;
        private System.Windows.Forms.CheckBox chromeOFF_Box;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btn_add_new_accounts;
        private System.Windows.Forms.Button delete_account_BTN;
        private System.Windows.Forms.DataGridView grv_acounts_count;
        private System.Windows.Forms.TextBox tb_count_counts_by_group;
        private System.Windows.Forms.Button back_to_fmanager;
        private System.Windows.Forms.CheckBox copy_cbox;
        private System.Windows.Forms.CheckBox checkBox_hold_on;
        private System.Windows.Forms.Button tem_profile_BTN;
    }
}