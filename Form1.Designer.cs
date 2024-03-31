namespace FileBackupTool
{
    partial class Form1
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
            this.ui_list_files = new System.Windows.Forms.ListView();
            this.ui_button_scan = new System.Windows.Forms.Button();
            this.ui_button_update = new System.Windows.Forms.Button();
            this.ui_textbox_source = new System.Windows.Forms.TextBox();
            this.ui_button_select_source = new System.Windows.Forms.Button();
            this.ui_label_source = new System.Windows.Forms.Label();
            this.ui_label_destination = new System.Windows.Forms.Label();
            this.ui_textbox_destination = new System.Windows.Forms.TextBox();
            this.ui_button_select_destination = new System.Windows.Forms.Button();
            this.ui_checkbox_display_all = new System.Windows.Forms.CheckBox();
            this.ui_checkbox_no_checksum = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ui_list_files
            // 
            this.ui_list_files.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_list_files.CheckBoxes = true;
            this.ui_list_files.FullRowSelect = true;
            this.ui_list_files.HideSelection = false;
            this.ui_list_files.Location = new System.Drawing.Point(12, 103);
            this.ui_list_files.Name = "ui_list_files";
            this.ui_list_files.Size = new System.Drawing.Size(597, 174);
            this.ui_list_files.TabIndex = 0;
            this.ui_list_files.UseCompatibleStateImageBehavior = false;
            this.ui_list_files.View = System.Windows.Forms.View.Details;
            // 
            // ui_button_scan
            // 
            this.ui_button_scan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_button_scan.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ui_button_scan.Location = new System.Drawing.Point(200, 74);
            this.ui_button_scan.Name = "ui_button_scan";
            this.ui_button_scan.Size = new System.Drawing.Size(409, 23);
            this.ui_button_scan.TabIndex = 1;
            this.ui_button_scan.Text = "Scan";
            this.ui_button_scan.UseVisualStyleBackColor = false;
            this.ui_button_scan.Click += new System.EventHandler(this.ui_button_scan_Click);
            // 
            // ui_button_update
            // 
            this.ui_button_update.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_button_update.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ui_button_update.Enabled = false;
            this.ui_button_update.Location = new System.Drawing.Point(12, 283);
            this.ui_button_update.Name = "ui_button_update";
            this.ui_button_update.Size = new System.Drawing.Size(597, 23);
            this.ui_button_update.TabIndex = 2;
            this.ui_button_update.Text = "Update Files";
            this.ui_button_update.UseVisualStyleBackColor = false;
            this.ui_button_update.Click += new System.EventHandler(this.ui_button_update_Click);
            // 
            // ui_textbox_source
            // 
            this.ui_textbox_source.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_textbox_source.Location = new System.Drawing.Point(157, 12);
            this.ui_textbox_source.Name = "ui_textbox_source";
            this.ui_textbox_source.Size = new System.Drawing.Size(452, 20);
            this.ui_textbox_source.TabIndex = 3;
            // 
            // ui_button_select_source
            // 
            this.ui_button_select_source.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ui_button_select_source.Location = new System.Drawing.Point(113, 10);
            this.ui_button_select_source.Name = "ui_button_select_source";
            this.ui_button_select_source.Size = new System.Drawing.Size(38, 23);
            this.ui_button_select_source.TabIndex = 4;
            this.ui_button_select_source.Text = "...";
            this.ui_button_select_source.UseVisualStyleBackColor = false;
            this.ui_button_select_source.Click += new System.EventHandler(this.ui_button_select_source_Click);
            // 
            // ui_label_source
            // 
            this.ui_label_source.AutoSize = true;
            this.ui_label_source.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ui_label_source.Location = new System.Drawing.Point(12, 15);
            this.ui_label_source.Name = "ui_label_source";
            this.ui_label_source.Size = new System.Drawing.Size(76, 13);
            this.ui_label_source.TabIndex = 5;
            this.ui_label_source.Text = "Source Folder:";
            // 
            // ui_label_destination
            // 
            this.ui_label_destination.AutoSize = true;
            this.ui_label_destination.Location = new System.Drawing.Point(12, 50);
            this.ui_label_destination.Name = "ui_label_destination";
            this.ui_label_destination.Size = new System.Drawing.Size(95, 13);
            this.ui_label_destination.TabIndex = 6;
            this.ui_label_destination.Text = "Destination Folder:";
            // 
            // ui_textbox_destination
            // 
            this.ui_textbox_destination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_textbox_destination.Location = new System.Drawing.Point(157, 47);
            this.ui_textbox_destination.Name = "ui_textbox_destination";
            this.ui_textbox_destination.Size = new System.Drawing.Size(452, 20);
            this.ui_textbox_destination.TabIndex = 7;
            // 
            // ui_button_select_destination
            // 
            this.ui_button_select_destination.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ui_button_select_destination.Location = new System.Drawing.Point(113, 45);
            this.ui_button_select_destination.Name = "ui_button_select_destination";
            this.ui_button_select_destination.Size = new System.Drawing.Size(38, 23);
            this.ui_button_select_destination.TabIndex = 8;
            this.ui_button_select_destination.Text = "...";
            this.ui_button_select_destination.UseVisualStyleBackColor = false;
            this.ui_button_select_destination.Click += new System.EventHandler(this.ui_button_select_destination_Click);
            // 
            // ui_checkbox_display_all
            // 
            this.ui_checkbox_display_all.AutoSize = true;
            this.ui_checkbox_display_all.Location = new System.Drawing.Point(15, 78);
            this.ui_checkbox_display_all.Name = "ui_checkbox_display_all";
            this.ui_checkbox_display_all.Size = new System.Drawing.Size(87, 17);
            this.ui_checkbox_display_all.TabIndex = 9;
            this.ui_checkbox_display_all.Text = "Show all files";
            this.ui_checkbox_display_all.UseVisualStyleBackColor = true;
            // 
            // ui_checkbox_no_checksum
            // 
            this.ui_checkbox_no_checksum.AutoSize = true;
            this.ui_checkbox_no_checksum.Location = new System.Drawing.Point(108, 78);
            this.ui_checkbox_no_checksum.Name = "ui_checkbox_no_checksum";
            this.ui_checkbox_no_checksum.Size = new System.Drawing.Size(92, 17);
            this.ui_checkbox_no_checksum.TabIndex = 10;
            this.ui_checkbox_no_checksum.Text = "No checksum";
            this.ui_checkbox_no_checksum.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(621, 318);
            this.Controls.Add(this.ui_checkbox_no_checksum);
            this.Controls.Add(this.ui_checkbox_display_all);
            this.Controls.Add(this.ui_button_select_destination);
            this.Controls.Add(this.ui_textbox_destination);
            this.Controls.Add(this.ui_label_destination);
            this.Controls.Add(this.ui_label_source);
            this.Controls.Add(this.ui_button_select_source);
            this.Controls.Add(this.ui_textbox_source);
            this.Controls.Add(this.ui_button_update);
            this.Controls.Add(this.ui_button_scan);
            this.Controls.Add(this.ui_list_files);
            this.MinimumSize = new System.Drawing.Size(350, 350);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FileBackupTool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView ui_list_files;
        private System.Windows.Forms.Button ui_button_scan;
        private System.Windows.Forms.Button ui_button_update;
        private System.Windows.Forms.TextBox ui_textbox_source;
        private System.Windows.Forms.Button ui_button_select_source;
        private System.Windows.Forms.Label ui_label_source;
        private System.Windows.Forms.Label ui_label_destination;
        private System.Windows.Forms.TextBox ui_textbox_destination;
        private System.Windows.Forms.Button ui_button_select_destination;
        private System.Windows.Forms.CheckBox ui_checkbox_display_all;
        private System.Windows.Forms.CheckBox ui_checkbox_no_checksum;
    }
}

