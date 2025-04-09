using System;
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
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // WordleForm
            // 
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(900, 750);
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



        /// <summary>
        /// The number of guesses allowed in the game & length of words allowed
        /// </summary>
        private const int GUESSES_ALLOWED = 6;

        private const int WORD_LENGTH = 5;


        /// <summary>
        /// The display and keyboard labels for the wordle game
        /// </summary>
        public System.Windows.Forms.Label[,] DisplayLabels = new System.Windows.Forms.Label[WORD_LENGTH, GUESSES_ALLOWED];
        public System.Windows.Forms.Label[][] KeyboardLabels = new System.Windows.Forms.Label[4][];

        public System.Windows.Forms.Label[] KeyboardRow1 = new System.Windows.Forms.Label[10];
        public string KeyboardRow1LetterMap =  "QWERTYUIOP";

        public System.Windows.Forms.Label[] KeyboardRow2 = new System.Windows.Forms.Label[9];
        public string KeyboardRow2LetterMap = "ASDFGHJKL";

        public System.Windows.Forms.Label[] KeyboardRow3 = new System.Windows.Forms.Label[7];
        public string KeyboardRow3LetterMap = "ZXCVBNM";

        public System.Windows.Forms.Label[] KeyboardSpecialKeys = new System.Windows.Forms.Label[3];
        public string KeyboardSpecialKeysLetterMap = "ENTER,DEL,RESET";



        /// <summary>
        /// Padding for the labels
        /// </summary>
        int xpad = 13;
        int ypad = 13;



        /// <summary>
        /// Initialize Row 1 of Keyboard
        /// </summary>
        /// <returns>Array of Row 1 Labels</returns>
        private System.Windows.Forms.Label[] InitKeyboardRow1()
        {
            
            int x = this.Width / 2 - ((10 * (50 + 13)) + 13) / 2;
            int y = this.Height - 300;
            for (int i = 0; i < 10; i++)
            {
                this.KeyboardRow1[i] = new System.Windows.Forms.Label();
                this.KeyboardRow1[i].Text = KeyboardRow1LetterMap[i].ToString();
                this.KeyboardRow1[i].Name = $"Key {KeyboardRow1LetterMap[i]}";
                this.KeyboardRow1[i].BackColor = Color.Black;
                this.KeyboardRow1[i].ForeColor = Color.White;
                this.KeyboardRow1[i].BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                this.KeyboardRow1[i].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                this.KeyboardRow1[i].Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.KeyboardRow1[i].AutoSize = false;
                this.KeyboardRow1[i].Location = new System.Drawing.Point(x + ((i + 1) * xpad), y);
                this.KeyboardRow1[i].Size = new System.Drawing.Size(50, 50);
                this.KeyboardRow1[i].Click += new System.EventHandler(this.WordleForm_Keyboard_Click);
                this.Controls.Add(this.KeyboardRow1[i]);
                x += 50;
            }
            return KeyboardRow1;
        }



        /// <summary>
        /// Initialize Row 2 of Keyboard
        /// </summary>
        /// <returns>Array of Row 2 Labels</returns>
        private System.Windows.Forms.Label[] InitKeyboardRow2()
        {

            int x = this.Width / 2 - ((9 * (50 + 13)) + 13) / 2;
            int y = this.Height - ((300 - 13 * 1) - 50 * 1);
            for (int i = 0; i < 9; i++)
            {
                this.KeyboardRow2[i] = new System.Windows.Forms.Label();
                this.KeyboardRow2[i].Text = KeyboardRow2LetterMap[i].ToString();
                this.KeyboardRow2[i].Name = $"Key {KeyboardRow2LetterMap[i]}";
                this.KeyboardRow2[i].BackColor = Color.Black;
                this.KeyboardRow2[i].ForeColor = Color.White;
                this.KeyboardRow2[i].BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                this.KeyboardRow2[i].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                this.KeyboardRow2[i].Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.KeyboardRow2[i].AutoSize = false;
                this.KeyboardRow2[i].Location = new System.Drawing.Point(x + ((i + 1) * xpad), y);
                this.KeyboardRow2[i].Size = new System.Drawing.Size(50, 50);
                this.KeyboardRow2[i].Click += new System.EventHandler(this.WordleForm_Keyboard_Click);
                this.Controls.Add(this.KeyboardRow2[i]);
                x += 50;
            }
            return KeyboardRow2;
        }



        /// <summary>
        /// Initialize Row 3 of Keyboard
        /// </summary>
        /// <returns>Array of Row 3 Labels</returns>
        private System.Windows.Forms.Label[] InitKeyboardRow3()
        {

            int x = this.Width / 2 - ((7 * (50 + 13)) + 13) / 2;
            int y = this.Height - ((300 - 13*2) - 50*2);
            for (int i = 0; i < 7; i++)
            {
                this.KeyboardRow3[i] = new System.Windows.Forms.Label();
                this.KeyboardRow3[i].Text = KeyboardRow3LetterMap[i].ToString();
                this.KeyboardRow3[i].Name = $"Key {KeyboardRow3LetterMap[i]}";
                this.KeyboardRow3[i].BackColor = Color.Black;
                this.KeyboardRow3[i].ForeColor = Color.White;
                this.KeyboardRow3[i].BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                this.KeyboardRow3[i].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                this.KeyboardRow3[i].Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.KeyboardRow3[i].AutoSize = false;
                this.KeyboardRow3[i].Location = new System.Drawing.Point(x + ((i + 1) * xpad), y);
                this.KeyboardRow3[i].Size = new System.Drawing.Size(50, 50);
                this.KeyboardRow3[i].Click += new System.EventHandler(this.WordleForm_Keyboard_Click);
                this.Controls.Add(this.KeyboardRow3[i]);
                x += 50;
            }
            return KeyboardRow3;
        }



        /// <summary>
        /// Initialize Row 4 of Keyboard
        /// </summary>
        /// <returns>Array of Row 4 Labels</returns>
        private System.Windows.Forms.Label[] InitKeyboardSpecialKeys()
        {
            int x = this.Width / 2 - ((3 * (150 + 13)) + 13) / 2;
            int y = this.Height - ((300 - 13 * 3) - 50 * 3);
            for (int i = 0; i < 3; i++)
            {
                this.KeyboardSpecialKeys[i] = new System.Windows.Forms.Label();
                this.KeyboardSpecialKeys[i].Text = KeyboardSpecialKeysLetterMap.Split(',')[i].ToString();
                this.KeyboardSpecialKeys[i].Name = $"Key {KeyboardSpecialKeysLetterMap.Split(',')[i]}";
                this.KeyboardSpecialKeys[i].BackColor = Color.Black;
                this.KeyboardSpecialKeys[i].ForeColor = Color.White;
                this.KeyboardSpecialKeys[i].BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                this.KeyboardSpecialKeys[i].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                this.KeyboardSpecialKeys[i].Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.KeyboardSpecialKeys[i].AutoSize = false;
                this.KeyboardSpecialKeys[i].Location = new System.Drawing.Point(x + ((i + 1) * xpad), y);
                this.KeyboardSpecialKeys[i].Size = new System.Drawing.Size(150, 50);
                this.KeyboardSpecialKeys[i].Click += new System.EventHandler(this.WordleForm_Keyboard_Click);
                this.Controls.Add(this.KeyboardSpecialKeys[i]);
                x += 150;
            }
            return KeyboardSpecialKeys;
        }



        /// <summary>
        /// Initialize Keyboard
        /// </summary>
        /// <returns>Jagged Array of KeyboardRows</returns>
        private System.Windows.Forms.Label[][] InitKeyboard()
        {
            this.KeyboardLabels[0] = InitKeyboardRow1();
            this.KeyboardLabels[1] = InitKeyboardRow2();
            this.KeyboardLabels[2] = InitKeyboardRow3();
            this.KeyboardLabels[3] = InitKeyboardSpecialKeys();
            this.ResumeLayout(true);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WordleForm_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WordleForm_KeyUp);
            this.ResumeLayout(false);
            return KeyboardLabels;
        }



        /// <summary>
        /// Initialize Display
        /// </summary>
        /// <returns>MultiDim Array of Display Labels</returns>
        private System.Windows.Forms.Label[,] InitDisplay()
        {
            
            int x = this.Width/2 - ((WORD_LENGTH*(50+13))+13)/2;
            int y = 0;

            for (int i = 0; i < WORD_LENGTH; i++)
            {
                for (int j = 0; j < GUESSES_ALLOWED; j++)
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

