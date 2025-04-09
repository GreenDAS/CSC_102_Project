using System.Drawing;

namespace CSC_102_Project
{
    partial class WordleForm
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
        private void InitializeComponent(System.Windows.Forms.Label[,] Display, System.Windows.Forms.Label[] Keyboard)
        {
            this.SuspendLayout();
            // 
            // WordleForm
            // 
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(884, 711);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.MaximumSize = new System.Drawing.Size(900, 750);
            this.MinimumSize = new System.Drawing.Size(900, 750);
            this.Name = "WordleForm";
            this.Text = "Wordle";
            this.ResumeLayout(false);

        }

        #endregion


        //
        // Coder Implemented Code
        //
        public System.Windows.Forms.Label[,] DisplayLabels = new System.Windows.Forms.Label[5, 6];

        private System.Windows.Forms.Label[,] InitDisplay()
        {
            
            int xpad = 13;
            int ypad = 13;
            int x = this.Width/2 - ((5*(50+13))+13)/2;
            int y = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    this.DisplayLabels[i, j] = new System.Windows.Forms.Label();
                    this.DisplayLabels[i, j].Text = " ";
                    this.DisplayLabels[i, j].BackColor = Color.Black;
                    this.DisplayLabels[i, j].ForeColor = Color.White;
                    this.DisplayLabels[i, j].BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    this.DisplayLabels[i, j].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    this.DisplayLabels[i, j].Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.DisplayLabels[i, j].AutoSize = false;
                    
                    this.DisplayLabels[i, j].Location = new System.Drawing.Point(x + ((i + 1) * xpad), y + ((j +1) * ypad));
                    this.DisplayLabels[i, j].Size = new System.Drawing.Size(50, 50);
                    this.Controls.Add(this.DisplayLabels[i, j]);
                    y += 50;
                }
                y = 0;
                x += 50;

                
            }
            return DisplayLabels;
        }

    }
}

