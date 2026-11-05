namespace Istomin_KP_Sobitiyka_RKIS_24ISP
{
    partial class FormCorrectionTypesOfExpenses
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
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonCorrectionResponsibleConfirm = new System.Windows.Forms.Button();
            this.buttonCorrectionResponsibleCancel = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "ID: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 16);
            this.label5.TabIndex = 15;
            this.label5.Text = "Тип расхода";
            // 
            // buttonCorrectionResponsibleConfirm
            // 
            this.buttonCorrectionResponsibleConfirm.Location = new System.Drawing.Point(149, 71);
            this.buttonCorrectionResponsibleConfirm.Name = "buttonCorrectionResponsibleConfirm";
            this.buttonCorrectionResponsibleConfirm.Size = new System.Drawing.Size(234, 47);
            this.buttonCorrectionResponsibleConfirm.TabIndex = 17;
            this.buttonCorrectionResponsibleConfirm.Text = "Подтвердить";
            this.buttonCorrectionResponsibleConfirm.UseVisualStyleBackColor = true;
            // 
            // buttonCorrectionResponsibleCancel
            // 
            this.buttonCorrectionResponsibleCancel.Location = new System.Drawing.Point(12, 71);
            this.buttonCorrectionResponsibleCancel.Name = "buttonCorrectionResponsibleCancel";
            this.buttonCorrectionResponsibleCancel.Size = new System.Drawing.Size(131, 47);
            this.buttonCorrectionResponsibleCancel.TabIndex = 16;
            this.buttonCorrectionResponsibleCancel.Text = "Отменить";
            this.buttonCorrectionResponsibleCancel.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(106, 40);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(277, 22);
            this.textBox3.TabIndex = 18;
            // 
            // FormCorrectionTypesOfExpenses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 132);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.buttonCorrectionResponsibleConfirm);
            this.Controls.Add(this.buttonCorrectionResponsibleCancel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormCorrectionTypesOfExpenses";
            this.Text = "Редактирование/Добавление Типов расходов";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonCorrectionResponsibleConfirm;
        private System.Windows.Forms.Button buttonCorrectionResponsibleCancel;
        private System.Windows.Forms.TextBox textBox3;
    }
}