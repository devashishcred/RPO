namespace Rpo.Efiling
{
    partial class ModalLoadingUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModalLoadingUI));
            this.picShadow = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picShadow)).BeginInit();
            this.SuspendLayout();
            // 
            // picShadow
            // 
            this.picShadow.BackColor = System.Drawing.Color.Transparent;
            this.picShadow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picShadow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picShadow.Image = ((System.Drawing.Image)(resources.GetObject("picShadow.Image")));
            this.picShadow.Location = new System.Drawing.Point(0, 0);
            this.picShadow.Name = "picShadow";
            this.picShadow.Size = new System.Drawing.Size(134, 134);
            this.picShadow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picShadow.TabIndex = 4;
            this.picShadow.TabStop = false;
            // 
            // ModalLoadingUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(134, 134);
            this.ControlBox = false;
            this.Controls.Add(this.picShadow);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ModalLoadingUI";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ModalLoadingUI_FormClosed);
            this.Load += new System.EventHandler(this.ModalLoadingUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picShadow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picShadow;
    }
}