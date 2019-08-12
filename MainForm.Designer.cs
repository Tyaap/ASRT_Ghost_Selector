namespace GhostSelector
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AllLobbiesColumnPlayers = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AllLobbiesColumnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AllLobbiesColumnCreator = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BanlistColumnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BanlistColumnSteamId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.GroupBoxGraphics = new System.Windows.Forms.GroupBox();
            this.CheckedListBoxGraphics = new System.Windows.Forms.CheckedListBox();
            this.GroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonConfirm = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.NumericUpDownPosition = new System.Windows.Forms.NumericUpDown();
            this.LabelPosition = new System.Windows.Forms.Label();
            this.CheckBoxPositionSelector = new System.Windows.Forms.CheckBox();
            this.FastestPlayerSelector = new System.Windows.Forms.GroupBox();
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonAdd = new System.Windows.Forms.Button();
            this.ButtonEdit = new System.Windows.Forms.Button();
            this.ButtonRemove = new System.Windows.Forms.Button();
            this.CheckBoxFastestPlayerSelector = new System.Windows.Forms.CheckBox();
            this.ListViewPlayers = new System.Windows.Forms.ListView();
            this.NameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SteamIdColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel2.SuspendLayout();
            this.GroupBoxGraphics.SuspendLayout();
            this.GroupBox.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPosition)).BeginInit();
            this.FastestPlayerSelector.SuspendLayout();
            this.TableLayoutPanel1.SuspendLayout();
            this.TableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Players";
            this.columnHeader3.Width = 58;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Creator";
            this.columnHeader1.Width = 195;
            // 
            // AllLobbiesColumnPlayers
            // 
            this.AllLobbiesColumnPlayers.Text = "Players";
            this.AllLobbiesColumnPlayers.Width = 58;
            // 
            // AllLobbiesColumnType
            // 
            this.AllLobbiesColumnType.Text = "Type";
            this.AllLobbiesColumnType.Width = 195;
            // 
            // AllLobbiesColumnCreator
            // 
            this.AllLobbiesColumnCreator.Text = "Creator";
            this.AllLobbiesColumnCreator.Width = 195;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 195;
            // 
            // BanlistColumnName
            // 
            this.BanlistColumnName.Text = "Name";
            this.BanlistColumnName.Width = 263;
            // 
            // BanlistColumnSteamId
            // 
            this.BanlistColumnSteamId.Text = "Steam Id";
            this.BanlistColumnSteamId.Width = 185;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.GroupBoxGraphics, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.GroupBox, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.FastestPlayerSelector, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(381, 490);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // GroupBoxGraphics
            // 
            this.GroupBoxGraphics.Controls.Add(this.CheckedListBoxGraphics);
            this.GroupBoxGraphics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupBoxGraphics.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.GroupBoxGraphics.Location = new System.Drawing.Point(3, 341);
            this.GroupBoxGraphics.Name = "GroupBoxGraphics";
            this.GroupBoxGraphics.Size = new System.Drawing.Size(375, 80);
            this.GroupBoxGraphics.TabIndex = 10;
            this.GroupBoxGraphics.TabStop = false;
            this.GroupBoxGraphics.Text = "Graphics";
            // 
            // CheckedListBoxGraphics
            // 
            this.CheckedListBoxGraphics.CheckOnClick = true;
            this.CheckedListBoxGraphics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckedListBoxGraphics.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckedListBoxGraphics.FormattingEnabled = true;
            this.CheckedListBoxGraphics.Items.AddRange(new object[] {
            "Hide nametags",
            "Hide ghost cars",
            "Hide personal best ghost"});
            this.CheckedListBoxGraphics.Location = new System.Drawing.Point(3, 19);
            this.CheckedListBoxGraphics.Name = "CheckedListBoxGraphics";
            this.CheckedListBoxGraphics.Size = new System.Drawing.Size(369, 58);
            this.CheckedListBoxGraphics.TabIndex = 11;
            // 
            // GroupBox
            // 
            this.GroupBox.Controls.Add(this.tableLayoutPanel5);
            this.GroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.GroupBox.Location = new System.Drawing.Point(3, 427);
            this.GroupBox.Name = "GroupBox";
            this.GroupBox.Size = new System.Drawing.Size(375, 60);
            this.GroupBox.TabIndex = 12;
            this.GroupBox.TabStop = false;
            this.GroupBox.Text = "Confirm Options";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.94851F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.05149F));
            this.tableLayoutPanel5.Controls.Add(this.ButtonConfirm, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.ButtonCancel, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(369, 38);
            this.tableLayoutPanel5.TabIndex = 6;
            // 
            // ButtonConfirm
            // 
            this.ButtonConfirm.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonConfirm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonConfirm.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ButtonConfirm.Location = new System.Drawing.Point(190, 3);
            this.ButtonConfirm.Name = "ButtonConfirm";
            this.ButtonConfirm.Size = new System.Drawing.Size(176, 32);
            this.ButtonConfirm.TabIndex = 14;
            this.ButtonConfirm.Text = "Save and Apply";
            this.ButtonConfirm.UseVisualStyleBackColor = true;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonCancel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ButtonCancel.Location = new System.Drawing.Point(3, 3);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(181, 32);
            this.ButtonCancel.TabIndex = 13;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(375, 86);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fixed Position";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.NumericUpDownPosition, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.LabelPosition, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.CheckBoxPositionSelector, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(369, 64);
            this.tableLayoutPanel3.TabIndex = 6;
            // 
            // NumericUpDownPosition
            // 
            this.NumericUpDownPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NumericUpDownPosition.Location = new System.Drawing.Point(187, 38);
            this.NumericUpDownPosition.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.NumericUpDownPosition.Name = "NumericUpDownPosition";
            this.NumericUpDownPosition.Size = new System.Drawing.Size(179, 23);
            this.NumericUpDownPosition.TabIndex = 3;
            // 
            // LabelPosition
            // 
            this.LabelPosition.AutoSize = true;
            this.LabelPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabelPosition.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelPosition.Location = new System.Drawing.Point(3, 35);
            this.LabelPosition.Name = "LabelPosition";
            this.LabelPosition.Padding = new System.Windows.Forms.Padding(3);
            this.LabelPosition.Size = new System.Drawing.Size(178, 29);
            this.LabelPosition.TabIndex = 2;
            this.LabelPosition.Text = "Enter a position:";
            this.LabelPosition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CheckBoxPositionSelector
            // 
            this.CheckBoxPositionSelector.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.CheckBoxPositionSelector, 2);
            this.CheckBoxPositionSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckBoxPositionSelector.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CheckBoxPositionSelector.Location = new System.Drawing.Point(3, 3);
            this.CheckBoxPositionSelector.Name = "CheckBoxPositionSelector";
            this.CheckBoxPositionSelector.Padding = new System.Windows.Forms.Padding(3);
            this.CheckBoxPositionSelector.Size = new System.Drawing.Size(363, 29);
            this.CheckBoxPositionSelector.TabIndex = 1;
            this.CheckBoxPositionSelector.Text = "Use this selector";
            this.CheckBoxPositionSelector.UseVisualStyleBackColor = true;
            this.CheckBoxPositionSelector.Click += new System.EventHandler(this.CheckBoxPositionSelector_Click);
            // 
            // FastestPlayerSelector
            // 
            this.FastestPlayerSelector.Controls.Add(this.TableLayoutPanel1);
            this.FastestPlayerSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FastestPlayerSelector.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.FastestPlayerSelector.Location = new System.Drawing.Point(3, 95);
            this.FastestPlayerSelector.Name = "FastestPlayerSelector";
            this.FastestPlayerSelector.Size = new System.Drawing.Size(375, 240);
            this.FastestPlayerSelector.TabIndex = 4;
            this.FastestPlayerSelector.TabStop = false;
            this.FastestPlayerSelector.Text = "Fastest Player";
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.ColumnCount = 1;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel1.Controls.Add(this.TableLayoutPanel4, 0, 2);
            this.TableLayoutPanel1.Controls.Add(this.CheckBoxFastestPlayerSelector, 0, 0);
            this.TableLayoutPanel1.Controls.Add(this.ListViewPlayers, 0, 1);
            this.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel1.Location = new System.Drawing.Point(3, 19);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 3;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(369, 218);
            this.TableLayoutPanel1.TabIndex = 5;
            // 
            // TableLayoutPanel4
            // 
            this.TableLayoutPanel4.ColumnCount = 3;
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.TableLayoutPanel4.Controls.Add(this.ButtonAdd, 0, 0);
            this.TableLayoutPanel4.Controls.Add(this.ButtonEdit, 2, 0);
            this.TableLayoutPanel4.Controls.Add(this.ButtonRemove, 1, 0);
            this.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel4.Location = new System.Drawing.Point(3, 181);
            this.TableLayoutPanel4.Name = "TableLayoutPanel4";
            this.TableLayoutPanel4.RowCount = 1;
            this.TableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.TableLayoutPanel4.Size = new System.Drawing.Size(363, 34);
            this.TableLayoutPanel4.TabIndex = 30;
            // 
            // ButtonAdd
            // 
            this.ButtonAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonAdd.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ButtonAdd.Location = new System.Drawing.Point(3, 3);
            this.ButtonAdd.Name = "ButtonAdd";
            this.ButtonAdd.Size = new System.Drawing.Size(114, 28);
            this.ButtonAdd.TabIndex = 7;
            this.ButtonAdd.Text = "Add";
            this.ButtonAdd.UseVisualStyleBackColor = true;
            this.ButtonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // ButtonEdit
            // 
            this.ButtonEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonEdit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ButtonEdit.Location = new System.Drawing.Point(244, 3);
            this.ButtonEdit.Name = "ButtonEdit";
            this.ButtonEdit.Size = new System.Drawing.Size(116, 28);
            this.ButtonEdit.TabIndex = 9;
            this.ButtonEdit.Text = "Edit";
            this.ButtonEdit.UseVisualStyleBackColor = true;
            this.ButtonEdit.Click += new System.EventHandler(this.ButtonEdit_Click);
            // 
            // ButtonRemove
            // 
            this.ButtonRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonRemove.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ButtonRemove.Location = new System.Drawing.Point(123, 3);
            this.ButtonRemove.Name = "ButtonRemove";
            this.ButtonRemove.Size = new System.Drawing.Size(115, 28);
            this.ButtonRemove.TabIndex = 8;
            this.ButtonRemove.Text = "Remove";
            this.ButtonRemove.UseVisualStyleBackColor = true;
            this.ButtonRemove.Click += new System.EventHandler(this.ButtonRemove_Click);
            // 
            // CheckBoxFastestPlayerSelector
            // 
            this.CheckBoxFastestPlayerSelector.AutoSize = true;
            this.CheckBoxFastestPlayerSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckBoxFastestPlayerSelector.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CheckBoxFastestPlayerSelector.Location = new System.Drawing.Point(3, 3);
            this.CheckBoxFastestPlayerSelector.Name = "CheckBoxFastestPlayerSelector";
            this.CheckBoxFastestPlayerSelector.Padding = new System.Windows.Forms.Padding(3);
            this.CheckBoxFastestPlayerSelector.Size = new System.Drawing.Size(363, 24);
            this.CheckBoxFastestPlayerSelector.TabIndex = 5;
            this.CheckBoxFastestPlayerSelector.Text = "Use this selector";
            this.CheckBoxFastestPlayerSelector.UseVisualStyleBackColor = true;
            this.CheckBoxFastestPlayerSelector.Click += new System.EventHandler(this.CheckBoxFastestPlayerSelector_Click);
            // 
            // ListViewPlayers
            // 
            this.ListViewPlayers.CheckBoxes = true;
            this.ListViewPlayers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumn,
            this.SteamIdColumn});
            this.ListViewPlayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListViewPlayers.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.ListViewPlayers.FullRowSelect = true;
            this.ListViewPlayers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ListViewPlayers.Location = new System.Drawing.Point(3, 33);
            this.ListViewPlayers.Name = "ListViewPlayers";
            this.ListViewPlayers.Size = new System.Drawing.Size(363, 142);
            this.ListViewPlayers.TabIndex = 6;
            this.ListViewPlayers.UseCompatibleStateImageBehavior = false;
            this.ListViewPlayers.View = System.Windows.Forms.View.Details;
            // 
            // NameColumn
            // 
            this.NameColumn.Text = "Name";
            this.NameColumn.Width = 171;
            // 
            // SteamIdColumn
            // 
            this.SteamIdColumn.Text = "Steam ID";
            this.SteamIdColumn.Width = 171;
            // 
            // MainForm
            // 
            this.AcceptButton = this.ButtonConfirm;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(381, 490);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Ghost Selector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.GroupBoxGraphics.ResumeLayout(false);
            this.GroupBox.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPosition)).EndInit();
            this.FastestPlayerSelector.ResumeLayout(false);
            this.TableLayoutPanel1.ResumeLayout(false);
            this.TableLayoutPanel1.PerformLayout();
            this.TableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnHeader3;
        internal System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader AllLobbiesColumnPlayers;
        internal System.Windows.Forms.ColumnHeader AllLobbiesColumnType;
        internal System.Windows.Forms.ColumnHeader AllLobbiesColumnCreator;
        internal System.Windows.Forms.ColumnHeader columnHeader2;
        internal System.Windows.Forms.ColumnHeader BanlistColumnName;
        internal System.Windows.Forms.ColumnHeader BanlistColumnSteamId;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox FastestPlayerSelector;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
        internal System.Windows.Forms.CheckBox CheckBoxFastestPlayerSelector;
        private System.Windows.Forms.ListView ListViewPlayers;
        private System.Windows.Forms.ColumnHeader NameColumn;
        private System.Windows.Forms.ColumnHeader SteamIdColumn;
        internal System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        internal System.Windows.Forms.CheckBox CheckBoxPositionSelector;
        internal System.Windows.Forms.Label LabelPosition;
        private System.Windows.Forms.NumericUpDown NumericUpDownPosition;
        private System.Windows.Forms.GroupBox GroupBox;
        internal System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        internal System.Windows.Forms.Button ButtonConfirm;
        internal System.Windows.Forms.Button ButtonCancel;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel4;
        internal System.Windows.Forms.Button ButtonAdd;
        internal System.Windows.Forms.Button ButtonEdit;
        internal System.Windows.Forms.Button ButtonRemove;
        private System.Windows.Forms.GroupBox GroupBoxGraphics;
        private System.Windows.Forms.CheckedListBox CheckedListBoxGraphics;
    }
}