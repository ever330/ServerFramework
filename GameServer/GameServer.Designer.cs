
namespace GameServer
{
    partial class GameServer
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serverLogRTB = new System.Windows.Forms.RichTextBox();
            this.dbListBox = new System.Windows.Forms.ListBox();
            this.IPTextBox = new System.Windows.Forms.TextBox();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.serverOpenBtn = new System.Windows.Forms.Button();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.dbUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.dbBtn = new System.Windows.Forms.Button();
            this.tableListBox = new System.Windows.Forms.ListBox();
            this.tableBtn = new System.Windows.Forms.Button();
            this.tableDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.tableDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // serverLogRTB
            // 
            this.serverLogRTB.Location = new System.Drawing.Point(12, 12);
            this.serverLogRTB.Name = "serverLogRTB";
            this.serverLogRTB.Size = new System.Drawing.Size(265, 425);
            this.serverLogRTB.TabIndex = 0;
            this.serverLogRTB.Text = "";
            // 
            // dbListBox
            // 
            this.dbListBox.FormattingEnabled = true;
            this.dbListBox.ItemHeight = 12;
            this.dbListBox.Location = new System.Drawing.Point(292, 12);
            this.dbListBox.Name = "dbListBox";
            this.dbListBox.Size = new System.Drawing.Size(120, 88);
            this.dbListBox.TabIndex = 1;
            // 
            // IPTextBox
            // 
            this.IPTextBox.Location = new System.Drawing.Point(12, 471);
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(117, 21);
            this.IPTextBox.TabIndex = 2;
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(135, 471);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(77, 21);
            this.portTextBox.TabIndex = 3;
            // 
            // serverOpenBtn
            // 
            this.serverOpenBtn.Location = new System.Drawing.Point(218, 471);
            this.serverOpenBtn.Name = "serverOpenBtn";
            this.serverOpenBtn.Size = new System.Drawing.Size(59, 23);
            this.serverOpenBtn.TabIndex = 4;
            this.serverOpenBtn.Text = "Open";
            this.serverOpenBtn.UseVisualStyleBackColor = true;
            this.serverOpenBtn.Click += new System.EventHandler(this.serverOpenBtn_Click);
            // 
            // updateTimer
            // 
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // dbUpdateTimer
            // 
            this.dbUpdateTimer.Tick += new System.EventHandler(this.dbUpdateTimer_Tick);
            // 
            // dbBtn
            // 
            this.dbBtn.Location = new System.Drawing.Point(292, 106);
            this.dbBtn.Name = "dbBtn";
            this.dbBtn.Size = new System.Drawing.Size(120, 23);
            this.dbBtn.TabIndex = 5;
            this.dbBtn.Text = "DB 조회";
            this.dbBtn.UseVisualStyleBackColor = true;
            this.dbBtn.Click += new System.EventHandler(this.dbBtn_Click);
            // 
            // tableListBox
            // 
            this.tableListBox.FormattingEnabled = true;
            this.tableListBox.ItemHeight = 12;
            this.tableListBox.Location = new System.Drawing.Point(292, 144);
            this.tableListBox.Name = "tableListBox";
            this.tableListBox.Size = new System.Drawing.Size(120, 88);
            this.tableListBox.TabIndex = 6;
            // 
            // tableBtn
            // 
            this.tableBtn.Location = new System.Drawing.Point(292, 238);
            this.tableBtn.Name = "tableBtn";
            this.tableBtn.Size = new System.Drawing.Size(120, 23);
            this.tableBtn.TabIndex = 7;
            this.tableBtn.Text = "Table 조회";
            this.tableBtn.UseVisualStyleBackColor = true;
            this.tableBtn.Click += new System.EventHandler(this.tableBtn_Click);
            // 
            // tableDataGridView
            // 
            this.tableDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableDataGridView.Location = new System.Drawing.Point(431, 12);
            this.tableDataGridView.Name = "tableDataGridView";
            this.tableDataGridView.RowTemplate.Height = 23;
            this.tableDataGridView.Size = new System.Drawing.Size(672, 425);
            this.tableDataGridView.TabIndex = 8;
            // 
            // GameServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1115, 534);
            this.Controls.Add(this.tableDataGridView);
            this.Controls.Add(this.tableBtn);
            this.Controls.Add(this.tableListBox);
            this.Controls.Add(this.dbBtn);
            this.Controls.Add(this.serverOpenBtn);
            this.Controls.Add(this.portTextBox);
            this.Controls.Add(this.IPTextBox);
            this.Controls.Add(this.dbListBox);
            this.Controls.Add(this.serverLogRTB);
            this.Name = "GameServer";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.tableDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox serverLogRTB;
        private System.Windows.Forms.ListBox dbListBox;
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.Button serverOpenBtn;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Timer dbUpdateTimer;
        private System.Windows.Forms.Button dbBtn;
        private System.Windows.Forms.ListBox tableListBox;
        private System.Windows.Forms.Button tableBtn;
        private System.Windows.Forms.DataGridView tableDataGridView;
    }
}

