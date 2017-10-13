namespace TaiwanSmaile_Taipei101
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBoxCamera = new System.Windows.Forms.PictureBox();
            this.Camera_timer = new System.Windows.Forms.Timer(this.components);
            this.Picture_timer = new System.Windows.Forms.Timer(this.components);
            this.pictureBoxCenter = new System.Windows.Forms.PictureBox();
            this.logo_pic = new System.Windows.Forms.PictureBox();
            this.Redbox = new System.Windows.Forms.PictureBox();
            this.Animation_timer = new System.Windows.Forms.Timer(this.components);
            this.Count_timer = new System.Windows.Forms.Timer(this.components);
            this.Countbox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCenter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logo_pic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Redbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Countbox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxCamera
            // 
            this.pictureBoxCamera.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxCamera.Name = "pictureBoxCamera";
            this.pictureBoxCamera.Size = new System.Drawing.Size(260, 237);
            this.pictureBoxCamera.TabIndex = 0;
            this.pictureBoxCamera.TabStop = false;
            this.pictureBoxCamera.Visible = false;
            // 
            // Camera_timer
            // 
            this.Camera_timer.Enabled = true;
            this.Camera_timer.Interval = 1000;
            this.Camera_timer.Tick += new System.EventHandler(this.Camera_timer_Tick);
            // 
            // Picture_timer
            // 
            this.Picture_timer.Enabled = true;
            this.Picture_timer.Interval = 3000;
            this.Picture_timer.Tick += new System.EventHandler(this.Picture_timer_Tick);
            // 
            // pictureBoxCenter
            // 
            this.pictureBoxCenter.Location = new System.Drawing.Point(1420, 871);
            this.pictureBoxCenter.Name = "pictureBoxCenter";
            this.pictureBoxCenter.Size = new System.Drawing.Size(260, 225);
            this.pictureBoxCenter.TabIndex = 1;
            this.pictureBoxCenter.TabStop = false;
            // 
            // logo_pic
            // 
            this.logo_pic.BackColor = System.Drawing.Color.Transparent;
            this.logo_pic.Location = new System.Drawing.Point(58, 300);
            this.logo_pic.Name = "logo_pic";
            this.logo_pic.Size = new System.Drawing.Size(340, 300);
            this.logo_pic.TabIndex = 2;
            this.logo_pic.TabStop = false;
            // 
            // Redbox
            // 
            this.Redbox.BackColor = System.Drawing.Color.Red;
            this.Redbox.Location = new System.Drawing.Point(496, 519);
            this.Redbox.Name = "Redbox";
            this.Redbox.Size = new System.Drawing.Size(90, 90);
            this.Redbox.TabIndex = 3;
            this.Redbox.TabStop = false;
            this.Redbox.Visible = false;
            // 
            // Animation_timer
            // 
            this.Animation_timer.Enabled = true;
            this.Animation_timer.Tick += new System.EventHandler(this.Animation_timer_Tick);
            // 
            // Count_timer
            // 
            this.Count_timer.Interval = 3000;
            this.Count_timer.Tick += new System.EventHandler(this.Count_timer_Tick);
            // 
            // Countbox
            // 
            this.Countbox.Location = new System.Drawing.Point(660, 196);
            this.Countbox.Name = "Countbox";
            this.Countbox.Size = new System.Drawing.Size(300, 350);
            this.Countbox.TabIndex = 4;
            this.Countbox.TabStop = false;
            this.Countbox.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.Countbox);
            this.Controls.Add(this.Redbox);
            this.Controls.Add(this.logo_pic);
            this.Controls.Add(this.pictureBoxCenter);
            this.Controls.Add(this.pictureBoxCamera);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCenter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logo_pic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Redbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Countbox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxCamera;
        private System.Windows.Forms.Timer Camera_timer;
        private System.Windows.Forms.Timer Picture_timer;
        private System.Windows.Forms.PictureBox pictureBoxCenter;
        private System.Windows.Forms.PictureBox logo_pic;
        private System.Windows.Forms.PictureBox Redbox;
        private System.Windows.Forms.Timer Animation_timer;
        private System.Windows.Forms.Timer Count_timer;
        private System.Windows.Forms.PictureBox Countbox;

    }
}

