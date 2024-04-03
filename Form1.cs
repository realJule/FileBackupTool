using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileBackupTool
{
    public partial class Form1 : Form
    {
        BackupChecker backupChecker = null;

        public Form1()
        {
            InitializeComponent();
#if DEBUG
            ui_textbox_source.Text = @"C:\Users\jule\Desktop\small system";
            ui_textbox_destination.Text = @"C:\Users\jule\Desktop\small backup";
#endif
        }

        private async void ui_button_scan_Click(object sender, EventArgs e)
        {
            if (!validatePathInput(ui_textbox_source.Text, "source")) { return; }
            if (!validatePathInput(ui_textbox_destination.Text, "destination")) { return; }

            ui_button_scan.Enabled = false;
            ui_checkbox_display_all.Enabled = false;
            ui_button_update.Enabled = false;
            resetFileListView();

            this.Text = "Scanning filesystem...";
            Stopwatch watch = new Stopwatch();
            watch.Start();
            await performFileScan();
            watch.Stop();
            this.Text = $"Done ({watch.Elapsed.Seconds}s elapsed)";

            resizeFileListViewColumns();
            ui_button_scan.Enabled = true;
            ui_checkbox_display_all.Enabled = true;
            ui_button_update.Enabled = true; // After scanning the files, the user can now use the "update" button
        }

        private bool validatePathInput(string path, string pathDescription)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show($"Please provide a valid {pathDescription} folder path.", "Invalid path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (!Directory.Exists(path))
            {
                MessageBox.Show($"The {pathDescription} folder path <{path}> does not exist!", "Invalid path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private async Task performFileScan()
        {
            var progress = new Progress<DetectedFile>(value =>
            {
                if ((value.status != FileStatus.OK) || (ui_checkbox_display_all.Checked))
                {
                    if (value.status == FileStatus.Moved)
                    {
                        updateMovedFile(value);
                    }
                    else
                    {
                        addFile(value);
                    }
                }
            });

            backupChecker = new BackupChecker(!ui_checkbox_no_checksum.Checked, progress);
            
            await Task.Run(async () => {
                await backupChecker.check(ui_textbox_source.Text, ui_textbox_destination.Text);
            });
        }

        private async void ui_button_update_Click(object sender, EventArgs e)
        {
            if (backupChecker != null)
            {
                ui_button_scan.Enabled = false;
                ui_button_update.Enabled = false;
                
                this.Text = "Updating your backup...";
                Stopwatch watch = new Stopwatch();
                watch.Start();
                await performFileUpdate();
                watch.Stop();
                this.Text = $"Done ({watch.Elapsed.Seconds}s elapsed)";

                ui_button_scan.Enabled = true;
                // Not enabling the "update" button here, because first a new "scan" is required
            }
        }

        private async Task performFileUpdate()
        {
            var progress = new Progress<DetectedFile>(value =>
            {
                ui_list_files.Items.RemoveByKey(value.getUiKey());
            });

            BackupUpdater updater = new BackupUpdater(progress);

            // Collect files that will be updated (e.g. only those that are selected/checked in the list-view)
            var files = backupChecker.GetDetectedFiles().Where((file) => {
                int index = ui_list_files.Items.IndexOfKey(file.getUiKey());
                return ui_list_files.Items[index].Checked;
            }).ToList();

            await Task.Run(async () => {
                await updater.run(files);
            });
        }

        private void ui_button_select_source_Click(object sender, EventArgs e)
        {
            ui_textbox_source.Text = getDirectoryPathFromUserPromptOrDefault(ui_textbox_source.Text);
        }

        private void ui_button_select_destination_Click(object sender, EventArgs e)
        {
            ui_textbox_destination.Text = getDirectoryPathFromUserPromptOrDefault(ui_textbox_destination.Text);
        }

        private string getDirectoryPathFromUserPromptOrDefault(string defaultValue)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = defaultValue;
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    return fbd.SelectedPath;
                }
                else
                {
                    return defaultValue;
                }
            }
        }

        private void resetFileListView()
        {
            ui_list_files.Items.Clear();
            ui_list_files.Columns.Clear();
            ui_list_files.Columns.Add("Status", 75, HorizontalAlignment.Left);
            ui_list_files.Columns.Add("File", 350, HorizontalAlignment.Left);
            ui_list_files.Columns.Add("Modified Date", -2, HorizontalAlignment.Left);
            ui_list_files.Columns.Add("Bytes", 50, HorizontalAlignment.Left);
            ui_list_files.Columns.Add("Checksum", -2, HorizontalAlignment.Left);
            ui_list_files.Columns.Add("Moved From", -2, HorizontalAlignment.Left);
        }

        private void addFile(DetectedFile file)
        {
            ListViewItem item = new ListViewItem(file.status.ToString());
            item.Checked = file.status != FileStatus.OK ? true : false;
            item.Name = file.getUiKey();
            item.SubItems.Add(file.sourcePath);
            item.SubItems.Add(file.modifiedDate.ToString());
            item.SubItems.Add(file.bytes.ToString());
            item.SubItems.Add(file.checksum);
            ui_list_files.Items.Add(item);
        }

        private void updateMovedFile(DetectedFile item)
        {
            int index = ui_list_files.Items.IndexOfKey(item.getUiKey());
            ui_list_files.Items[index].SubItems[0].Text = item.status.ToString();
            ui_list_files.Items[index].SubItems.Add(item.targetPathMovedFrom.Replace(ui_textbox_destination.Text, ""));
        }

        private void resizeFileListViewColumns()
        {
            
            ui_list_files.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            ui_list_files.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.HeaderSize);
            ui_list_files.AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.HeaderSize);
        }
    }
}
