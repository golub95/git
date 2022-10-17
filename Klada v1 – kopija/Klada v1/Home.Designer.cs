namespace Klada_v3
{
    partial class Home
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_Favbet = new System.Windows.Forms.CheckBox();
            this.cb_Mozzart = new System.Windows.Forms.CheckBox();
            this.cb_HL = new System.Windows.Forms.CheckBox();
            this.cb_Germania = new System.Windows.Forms.CheckBox();
            this.cb_IsprazniTablicu = new System.Windows.Forms.CheckBox();
            this.cb_PSK = new System.Windows.Forms.CheckBox();
            this.cb_Stanleybet = new System.Windows.Forms.CheckBox();
            this.cb_SuperSport = new System.Windows.Forms.CheckBox();
            this.btn_run = new System.Windows.Forms.Button();
            this.cb_Calc = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_run, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cb_Calc, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(292, 223);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_Favbet);
            this.groupBox1.Controls.Add(this.cb_Mozzart);
            this.groupBox1.Controls.Add(this.cb_HL);
            this.groupBox1.Controls.Add(this.cb_Germania);
            this.groupBox1.Controls.Add(this.cb_IsprazniTablicu);
            this.groupBox1.Controls.Add(this.cb_PSK);
            this.groupBox1.Controls.Add(this.cb_Stanleybet);
            this.groupBox1.Controls.Add(this.cb_SuperSport);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(286, 137);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Kladionice";
            // 
            // cb_Favbet
            // 
            this.cb_Favbet.AutoSize = true;
            this.cb_Favbet.Location = new System.Drawing.Point(116, 42);
            this.cb_Favbet.Name = "cb_Favbet";
            this.cb_Favbet.Size = new System.Drawing.Size(59, 17);
            this.cb_Favbet.TabIndex = 7;
            this.cb_Favbet.Text = "Favbet";
            this.cb_Favbet.UseVisualStyleBackColor = true;
            this.cb_Favbet.CheckedChanged += new System.EventHandler(this.cb_Favbet_CheckedChanged);
            // 
            // cb_Mozzart
            // 
            this.cb_Mozzart.AutoSize = true;
            this.cb_Mozzart.Location = new System.Drawing.Point(116, 19);
            this.cb_Mozzart.Name = "cb_Mozzart";
            this.cb_Mozzart.Size = new System.Drawing.Size(63, 17);
            this.cb_Mozzart.TabIndex = 6;
            this.cb_Mozzart.Text = "Mozzart";
            this.cb_Mozzart.UseVisualStyleBackColor = true;
            this.cb_Mozzart.CheckedChanged += new System.EventHandler(this.cb_Mozzart_CheckedChanged);
            // 
            // cb_HL
            // 
            this.cb_HL.AutoSize = true;
            this.cb_HL.Location = new System.Drawing.Point(9, 114);
            this.cb_HL.Name = "cb_HL";
            this.cb_HL.Size = new System.Drawing.Size(40, 17);
            this.cb_HL.TabIndex = 5;
            this.cb_HL.Text = "HL";
            this.cb_HL.UseVisualStyleBackColor = true;
            // 
            // cb_Germania
            // 
            this.cb_Germania.AutoSize = true;
            this.cb_Germania.Location = new System.Drawing.Point(9, 88);
            this.cb_Germania.Name = "cb_Germania";
            this.cb_Germania.Size = new System.Drawing.Size(71, 17);
            this.cb_Germania.TabIndex = 4;
            this.cb_Germania.Text = "Germania";
            this.cb_Germania.UseVisualStyleBackColor = true;
            this.cb_Germania.CheckedChanged += new System.EventHandler(this.cb_Germania_CheckedChanged);
            // 
            // cb_IsprazniTablicu
            // 
            this.cb_IsprazniTablicu.AutoSize = true;
            this.cb_IsprazniTablicu.Location = new System.Drawing.Point(116, 114);
            this.cb_IsprazniTablicu.Name = "cb_IsprazniTablicu";
            this.cb_IsprazniTablicu.Size = new System.Drawing.Size(100, 17);
            this.cb_IsprazniTablicu.TabIndex = 3;
            this.cb_IsprazniTablicu.Text = "Isprazni Tablicu";
            this.cb_IsprazniTablicu.UseVisualStyleBackColor = true;
            // 
            // cb_PSK
            // 
            this.cb_PSK.AutoSize = true;
            this.cb_PSK.Location = new System.Drawing.Point(9, 65);
            this.cb_PSK.Name = "cb_PSK";
            this.cb_PSK.Size = new System.Drawing.Size(47, 17);
            this.cb_PSK.TabIndex = 2;
            this.cb_PSK.Text = "PSK";
            this.cb_PSK.UseVisualStyleBackColor = true;
            // 
            // cb_Stanleybet
            // 
            this.cb_Stanleybet.AutoSize = true;
            this.cb_Stanleybet.Location = new System.Drawing.Point(9, 42);
            this.cb_Stanleybet.Name = "cb_Stanleybet";
            this.cb_Stanleybet.Size = new System.Drawing.Size(76, 17);
            this.cb_Stanleybet.TabIndex = 1;
            this.cb_Stanleybet.Text = "Stanleybet";
            this.cb_Stanleybet.UseVisualStyleBackColor = true;
            this.cb_Stanleybet.CheckedChanged += new System.EventHandler(this.cb_Stanleybet_CheckedChanged);
            // 
            // cb_SuperSport
            // 
            this.cb_SuperSport.AutoSize = true;
            this.cb_SuperSport.Location = new System.Drawing.Point(9, 19);
            this.cb_SuperSport.Name = "cb_SuperSport";
            this.cb_SuperSport.Size = new System.Drawing.Size(79, 17);
            this.cb_SuperSport.TabIndex = 0;
            this.cb_SuperSport.Text = "SuperSport";
            this.cb_SuperSport.UseVisualStyleBackColor = true;
            // 
            // btn_run
            // 
            this.btn_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_run.Location = new System.Drawing.Point(3, 186);
            this.btn_run.Name = "btn_run";
            this.btn_run.Size = new System.Drawing.Size(286, 23);
            this.btn_run.TabIndex = 1;
            this.btn_run.Text = "Run";
            this.btn_run.UseVisualStyleBackColor = true;
            this.btn_run.Click += new System.EventHandler(this.btn_run_Click);
            // 
            // cb_Calc
            // 
            this.cb_Calc.AutoSize = true;
            this.cb_Calc.Location = new System.Drawing.Point(3, 146);
            this.cb_Calc.Name = "cb_Calc";
            this.cb_Calc.Size = new System.Drawing.Size(77, 17);
            this.cb_Calc.TabIndex = 2;
            this.cb_Calc.Text = "Kalkulacije";
            this.cb_Calc.UseVisualStyleBackColor = true;
            this.cb_Calc.CheckedChanged += new System.EventHandler(this.cb_Calc_CheckedChanged);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 223);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Home";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cb_PSK;
        private System.Windows.Forms.CheckBox cb_Stanleybet;
        private System.Windows.Forms.CheckBox cb_SuperSport;
        private System.Windows.Forms.Button btn_run;
        private System.Windows.Forms.CheckBox cb_Calc;
        private System.Windows.Forms.CheckBox cb_IsprazniTablicu;
        private System.Windows.Forms.CheckBox cb_Germania;
        private System.Windows.Forms.CheckBox cb_HL;
        private System.Windows.Forms.CheckBox cb_Mozzart;
        private System.Windows.Forms.CheckBox cb_Favbet;
    }
}