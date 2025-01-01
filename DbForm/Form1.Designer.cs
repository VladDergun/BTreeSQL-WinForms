using System.Windows.Forms;

namespace DbForm
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
            this.titleLabel = new System.Windows.Forms.Label();
            this.addRowButton = new System.Windows.Forms.Button();
            this.deleteRowButton = new System.Windows.Forms.Button();
            this.searchInput = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.saveChangesButton = new System.Windows.Forms.Button();
            this.dbTable = new System.Windows.Forms.DataGridView();
            this.IdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dbTable)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(375, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(300, 65);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "SMLSQL Manager";
            this.titleLabel.ForeColor = System.Drawing.Color.DarkSlateBlue;
            // 
            // addRowButton
            // 
            this.addRowButton.BackColor = System.Drawing.Color.LightGreen;
            this.addRowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addRowButton.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.addRowButton.Location = new System.Drawing.Point(50, 100);
            this.addRowButton.Name = "addRowButton";
            this.addRowButton.Size = new System.Drawing.Size(150, 50);
            this.addRowButton.TabIndex = 1;
            this.addRowButton.Text = "Add Row";
            this.addRowButton.UseVisualStyleBackColor = false;
            this.addRowButton.Click += new System.EventHandler(this.AddRowButton_Click);
            // 
            // deleteRowButton
            // 
            this.deleteRowButton.BackColor = System.Drawing.Color.LightCoral;
            this.deleteRowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteRowButton.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.deleteRowButton.Location = new System.Drawing.Point(220, 100);
            this.deleteRowButton.Name = "deleteRowButton";
            this.deleteRowButton.Size = new System.Drawing.Size(150, 50);
            this.deleteRowButton.TabIndex = 2;
            this.deleteRowButton.Text = "Delete Row";
            this.deleteRowButton.UseVisualStyleBackColor = false;
            this.deleteRowButton.Click += new System.EventHandler(this.DeleteRowButton_Click);
            // 
            // searchInput
            // 
            this.searchInput.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.searchInput.Location = new System.Drawing.Point(600, 110);
            this.searchInput.Name = "searchInput";
            this.searchInput.Size = new System.Drawing.Size(250, 39);
            this.searchInput.TabIndex = 3;
            this.searchInput.Text = "Search";
            // 
            // searchButton
            // 
            this.searchButton.BackColor = System.Drawing.Color.CornflowerBlue;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.searchButton.ForeColor = System.Drawing.Color.White;
            this.searchButton.Location = new System.Drawing.Point(870, 100);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(150, 50);
            this.searchButton.TabIndex = 4;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // saveChangesButton
            // 
            this.saveChangesButton.BackColor = System.Drawing.Color.Goldenrod;
            this.saveChangesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveChangesButton.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.saveChangesButton.Location = new System.Drawing.Point(50, 20);
            this.saveChangesButton.Name = "saveChangesButton";
            this.saveChangesButton.Size = new System.Drawing.Size(200, 50);
            this.saveChangesButton.TabIndex = 5;
            this.saveChangesButton.Text = "Save Changes";
            this.saveChangesButton.UseVisualStyleBackColor = false;
            this.saveChangesButton.Click += new System.EventHandler(this.SaveTreeAndExportButton_Click);
            // 
            // dbTable
            // 
            this.dbTable.AllowUserToAddRows = false;
            this.dbTable.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dbTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dbTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dbTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.IdColumn, this.NameColumn });
            this.dbTable.Location = new System.Drawing.Point(50, 170);
            this.dbTable.Name = "dbTable";
            this.dbTable.Size = new System.Drawing.Size(970, 400);
            this.dbTable.TabIndex = 6;
            // 
            // IdColumn
            // 
            this.IdColumn.HeaderText = "ID";
            this.IdColumn.MinimumWidth = 8;
            this.IdColumn.Name = "IdColumn";
            this.IdColumn.Width = 200;
            this.IdColumn.ReadOnly = true;
            // 
            // NameColumn
            // 
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.MinimumWidth = 8;
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.Width = 300;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 650);
            this.Controls.Add(this.dbTable);
            this.Controls.Add(this.saveChangesButton);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchInput);
            this.Controls.Add(this.deleteRowButton);
            this.Controls.Add(this.addRowButton);
            this.Controls.Add(this.titleLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SMLSQL Database Manager";
            ((System.ComponentModel.ISupportInitialize)(this.dbTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }


        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button addRowButton;
        private System.Windows.Forms.Button deleteRowButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.DataGridView dbTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private TextBox searchInput;
        private Button searchButton;
        private Button saveChangesButton;
    }
}

