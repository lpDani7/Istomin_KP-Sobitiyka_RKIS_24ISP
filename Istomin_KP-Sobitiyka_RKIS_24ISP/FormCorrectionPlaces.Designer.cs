namespace Istomin_KP_Sobitiyka_RKIS_24ISP
{
    partial class FormCorrectionPlaces
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCorrectionPlaces));
            this.labelCorrectionPlacesID = new System.Windows.Forms.Label();
            this.buttonCorrectionPlacesConfirm = new System.Windows.Forms.Button();
            this.buttonCorrectionPlacesCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxCorrectionPlacesAdress = new System.Windows.Forms.TextBox();
            this.textBoxCorrectionPlacesName = new System.Windows.Forms.TextBox();
            this.textBoxCorrectionPlacesCapacity = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelCorrectionPlacesID
            // 
            this.labelCorrectionPlacesID.AutoSize = true;
            this.labelCorrectionPlacesID.Location = new System.Drawing.Point(12, 9);
            this.labelCorrectionPlacesID.Name = "labelCorrectionPlacesID";
            this.labelCorrectionPlacesID.Size = new System.Drawing.Size(26, 16);
            this.labelCorrectionPlacesID.TabIndex = 1;
            this.labelCorrectionPlacesID.Text = "ID: ";
            // 
            // buttonCorrectionPlacesConfirm
            // 
            this.buttonCorrectionPlacesConfirm.Location = new System.Drawing.Point(152, 121);
            this.buttonCorrectionPlacesConfirm.Name = "buttonCorrectionPlacesConfirm";
            this.buttonCorrectionPlacesConfirm.Size = new System.Drawing.Size(234, 47);
            this.buttonCorrectionPlacesConfirm.TabIndex = 13;
            this.buttonCorrectionPlacesConfirm.Text = "Подтвердить";
            this.buttonCorrectionPlacesConfirm.UseVisualStyleBackColor = true;
            // 
            // buttonCorrectionPlacesCancel
            // 
            this.buttonCorrectionPlacesCancel.Location = new System.Drawing.Point(15, 121);
            this.buttonCorrectionPlacesCancel.Name = "buttonCorrectionPlacesCancel";
            this.buttonCorrectionPlacesCancel.Size = new System.Drawing.Size(131, 47);
            this.buttonCorrectionPlacesCancel.TabIndex = 12;
            this.buttonCorrectionPlacesCancel.Text = "Отменить";
            this.buttonCorrectionPlacesCancel.UseVisualStyleBackColor = true;
            this.buttonCorrectionPlacesCancel.Click += new System.EventHandler(this.buttonCorrectionPlacesCancel_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "Адрес";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "Название";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "Вместимость";
            // 
            // textBoxCorrectionPlacesAdress
            // 
            this.textBoxCorrectionPlacesAdress.Location = new System.Drawing.Point(65, 66);
            this.textBoxCorrectionPlacesAdress.Name = "textBoxCorrectionPlacesAdress";
            this.textBoxCorrectionPlacesAdress.Size = new System.Drawing.Size(321, 22);
            this.textBoxCorrectionPlacesAdress.TabIndex = 17;
            // 
            // textBoxCorrectionPlacesName
            // 
            this.textBoxCorrectionPlacesName.Location = new System.Drawing.Point(91, 38);
            this.textBoxCorrectionPlacesName.Name = "textBoxCorrectionPlacesName";
            this.textBoxCorrectionPlacesName.Size = new System.Drawing.Size(295, 22);
            this.textBoxCorrectionPlacesName.TabIndex = 18;
            // 
            // textBoxCorrectionPlacesCapacity
            // 
            this.textBoxCorrectionPlacesCapacity.Location = new System.Drawing.Point(111, 93);
            this.textBoxCorrectionPlacesCapacity.Name = "textBoxCorrectionPlacesCapacity";
            this.textBoxCorrectionPlacesCapacity.Size = new System.Drawing.Size(275, 22);
            this.textBoxCorrectionPlacesCapacity.TabIndex = 19;
            // 
            // FormCorrectionPlaces
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 178);
            this.Controls.Add(this.textBoxCorrectionPlacesCapacity);
            this.Controls.Add(this.textBoxCorrectionPlacesName);
            this.Controls.Add(this.textBoxCorrectionPlacesAdress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonCorrectionPlacesConfirm);
            this.Controls.Add(this.buttonCorrectionPlacesCancel);
            this.Controls.Add(this.labelCorrectionPlacesID);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormCorrectionPlaces";
            this.Text = "Редактирование/Добавление Мест проведения";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCorrectionPlacesID;
        private System.Windows.Forms.Button buttonCorrectionPlacesConfirm;
        private System.Windows.Forms.Button buttonCorrectionPlacesCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxCorrectionPlacesAdress;
        private System.Windows.Forms.TextBox textBoxCorrectionPlacesName;
        private System.Windows.Forms.TextBox textBoxCorrectionPlacesCapacity;
    }
}