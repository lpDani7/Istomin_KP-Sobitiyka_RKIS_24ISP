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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCorrectionTypesOfExpenses));
            this.labelCorrectionTypesOfExpensesID = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonCorrectionTypesOfExpensesConfirm = new System.Windows.Forms.Button();
            this.buttonCorrectionTypesOfExpensesCancel = new System.Windows.Forms.Button();
            this.textBoxCorrectionTypesOfExpensesName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelCorrectionTypesOfExpensesID
            // 
            this.labelCorrectionTypesOfExpensesID.AutoSize = true;
            this.labelCorrectionTypesOfExpensesID.Location = new System.Drawing.Point(12, 9);
            this.labelCorrectionTypesOfExpensesID.Name = "labelCorrectionTypesOfExpensesID";
            this.labelCorrectionTypesOfExpensesID.Size = new System.Drawing.Size(26, 16);
            this.labelCorrectionTypesOfExpensesID.TabIndex = 2;
            this.labelCorrectionTypesOfExpensesID.Text = "ID: ";
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
            // buttonCorrectionTypesOfExpensesConfirm
            // 
            this.buttonCorrectionTypesOfExpensesConfirm.Location = new System.Drawing.Point(149, 71);
            this.buttonCorrectionTypesOfExpensesConfirm.Name = "buttonCorrectionTypesOfExpensesConfirm";
            this.buttonCorrectionTypesOfExpensesConfirm.Size = new System.Drawing.Size(234, 47);
            this.buttonCorrectionTypesOfExpensesConfirm.TabIndex = 17;
            this.buttonCorrectionTypesOfExpensesConfirm.Text = "Подтвердить";
            this.buttonCorrectionTypesOfExpensesConfirm.UseVisualStyleBackColor = true;
            // 
            // buttonCorrectionTypesOfExpensesCancel
            // 
            this.buttonCorrectionTypesOfExpensesCancel.Location = new System.Drawing.Point(12, 71);
            this.buttonCorrectionTypesOfExpensesCancel.Name = "buttonCorrectionTypesOfExpensesCancel";
            this.buttonCorrectionTypesOfExpensesCancel.Size = new System.Drawing.Size(131, 47);
            this.buttonCorrectionTypesOfExpensesCancel.TabIndex = 16;
            this.buttonCorrectionTypesOfExpensesCancel.Text = "Отменить";
            this.buttonCorrectionTypesOfExpensesCancel.UseVisualStyleBackColor = true;
            this.buttonCorrectionTypesOfExpensesCancel.Click += new System.EventHandler(this.buttonCorrectionTypesOfExpensesCancel_Click);
            // 
            // textBoxCorrectionTypesOfExpensesName
            // 
            this.textBoxCorrectionTypesOfExpensesName.Location = new System.Drawing.Point(106, 40);
            this.textBoxCorrectionTypesOfExpensesName.Name = "textBoxCorrectionTypesOfExpensesName";
            this.textBoxCorrectionTypesOfExpensesName.Size = new System.Drawing.Size(277, 22);
            this.textBoxCorrectionTypesOfExpensesName.TabIndex = 18;
            // 
            // FormCorrectionTypesOfExpenses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 132);
            this.Controls.Add(this.textBoxCorrectionTypesOfExpensesName);
            this.Controls.Add(this.buttonCorrectionTypesOfExpensesConfirm);
            this.Controls.Add(this.buttonCorrectionTypesOfExpensesCancel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelCorrectionTypesOfExpensesID);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormCorrectionTypesOfExpenses";
            this.Text = "Редактирование/Добавление Типов расходов";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCorrectionTypesOfExpensesID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonCorrectionTypesOfExpensesConfirm;
        private System.Windows.Forms.Button buttonCorrectionTypesOfExpensesCancel;
        private System.Windows.Forms.TextBox textBoxCorrectionTypesOfExpensesName;
    }
}