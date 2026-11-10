namespace Istomin_KP_Sobitiyka_RKIS_24ISP
{
    partial class FormCollectives
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.правкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавтьКоллективToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.редактироватьКоллективToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьКоллективToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.правкаToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1067, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // правкаToolStripMenuItem
            // 
            this.правкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавтьКоллективToolStripMenuItem,
            this.редактироватьКоллективToolStripMenuItem,
            this.удалитьКоллективToolStripMenuItem});
            this.правкаToolStripMenuItem.Name = "правкаToolStripMenuItem";
            this.правкаToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.правкаToolStripMenuItem.Text = "Правка";
            // 
            // добавтьКоллективToolStripMenuItem
            // 
            this.добавтьКоллективToolStripMenuItem.Name = "добавтьКоллективToolStripMenuItem";
            this.добавтьКоллективToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.добавтьКоллективToolStripMenuItem.Text = "Добавть коллектив";
            // 
            // редактироватьКоллективToolStripMenuItem
            // 
            this.редактироватьКоллективToolStripMenuItem.Name = "редактироватьКоллективToolStripMenuItem";
            this.редактироватьКоллективToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.редактироватьКоллективToolStripMenuItem.Text = "Редактировать коллектив";
            // 
            // удалитьКоллективToolStripMenuItem
            // 
            this.удалитьКоллективToolStripMenuItem.Name = "удалитьКоллективToolStripMenuItem";
            this.удалитьКоллективToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.удалитьКоллективToolStripMenuItem.Text = "Удалить коллектив";
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // FormCollectives
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormCollectives";
            this.Text = "Коллективы";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem правкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавтьКоллективToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem редактироватьКоллективToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьКоллективToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
    }
}