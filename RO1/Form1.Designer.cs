namespace RO1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button4 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbPixel = new System.Windows.Forms.RadioButton();
            this.rbHaara = new System.Windows.Forms.RadioButton();
            this.rbHist = new System.Windows.Forms.RadioButton();
            this.rbHamm = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(317, 488);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Распознать с камеры";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(640, 480);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 488);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(161, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Сохранить тестовое";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(179, 488);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(132, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Распознать из файла";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(663, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(640, 480);
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1227, 499);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(119, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "Обновить примеры";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbHamm);
            this.panel1.Controls.Add(this.rbHist);
            this.panel1.Controls.Add(this.rbHaara);
            this.panel1.Controls.Add(this.rbPixel);
            this.panel1.Location = new System.Drawing.Point(462, 488);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(559, 34);
            this.panel1.TabIndex = 6;
            // 
            // rbPixel
            // 
            this.rbPixel.AutoSize = true;
            this.rbPixel.Location = new System.Drawing.Point(7, 8);
            this.rbPixel.Name = "rbPixel";
            this.rbPixel.Size = new System.Drawing.Size(156, 17);
            this.rbPixel.TabIndex = 0;
            this.rbPixel.Text = "Попиксельное сравнение";
            this.rbPixel.UseVisualStyleBackColor = true;
            // 
            // rbHaara
            // 
            this.rbHaara.AutoSize = true;
            this.rbHaara.Checked = true;
            this.rbHaara.Location = new System.Drawing.Point(177, 9);
            this.rbHaara.Name = "rbHaara";
            this.rbHaara.Size = new System.Drawing.Size(56, 17);
            this.rbHaara.TabIndex = 1;
            this.rbHaara.TabStop = true;
            this.rbHaara.Text = "Хаара";
            this.rbHaara.UseVisualStyleBackColor = true;
            // 
            // rbHist
            // 
            this.rbHist.AutoSize = true;
            this.rbHist.Location = new System.Drawing.Point(255, 8);
            this.rbHist.Name = "rbHist";
            this.rbHist.Size = new System.Drawing.Size(95, 17);
            this.rbHist.TabIndex = 2;
            this.rbHist.Text = "Гистограммы";
            this.rbHist.UseVisualStyleBackColor = true;
            // 
            // rbHamm
            // 
            this.rbHamm.AutoSize = true;
            this.rbHamm.Location = new System.Drawing.Point(404, 8);
            this.rbHamm.Name = "rbHamm";
            this.rbHamm.Size = new System.Drawing.Size(137, 17);
            this.rbHamm.TabIndex = 3;
            this.rbHamm.Text = "РасстоянияХэмминга";
            this.rbHamm.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1358, 534);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbHaara;
        private System.Windows.Forms.RadioButton rbPixel;
        private System.Windows.Forms.RadioButton rbHist;
        private System.Windows.Forms.RadioButton rbHamm;
    }
}

