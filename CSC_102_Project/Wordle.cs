using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSC_102_Project
{
    public partial class WordleForm : Form
    {

        private class GuessHandler
        {
            static string currentGuess = string.Empty;

            static int currentTimeGuessing = 1;

            public enum correctness
            {
                notInWord = 0,
                inWord = 1,
                correctPlace = 2,
            }

            public static correctness[] guessColorMap = new correctness[5];

        }


        private class Wordle : GuessHandler
        {
            string correctWord = string.Empty;

            public void IsCorrect(string guess)
            {
                if (correctWord.ToUpper() == guess.ToUpper())
                {
                    foreach (char c in guess)
                    {
                        guessColorMap[c] = correctness.correctPlace;
                    }
                    //End Game
                }
                else
                {
                    for (int i = 0; i < correctWord.Length; i++)
                    {
                        for (int i2 = 0; i2 < guess.Length; i2++)
                        {
                            if (correctWord.ToUpper()[i] == guess.ToUpper()[i])
                            {
                                guessColorMap[i] = correctness.correctPlace;
                                break;
                            }
                            else if (correctWord.ToUpper()[i] == guess.ToUpper()[i2])
                            {
                                guessColorMap[i] = correctness.inWord;
                                break;
                            }
                            else
                            {
                                guessColorMap[i] = correctness.notInWord;
                            }
                        }
                    }
                }

            }

            public void CustomWordEntered(string CustomWord)
            {
                correctWord = CustomWord;
            }
        }

        private class Keyboard : GuessHandler
        {
            public System.Windows.Forms.Label[] KeyboardLabels = new System.Windows.Forms.Label[26];

            static string guessMade = string.Empty;

            public void changeColor(Label LabelToUpdate, correctness Correctness)
            {
                switch (Correctness)
                {
                    case correctness.notInWord:
                        LabelToUpdate.BackColor = Color.DarkGray;
                        break;

                    case correctness.inWord:
                        LabelToUpdate.BackColor = Color.Yellow;
                        break;

                    case correctness.correctPlace:
                        LabelToUpdate.BackColor = Color.Green;
                        break;

                    default:
                        MessageBox.Show("No Known Correct Location Error");
                        break;

                }
            }

            public void keyPressed(string Key)
            {
                if (guessMade.Length < 5) { guessMade += Key; }
            }

            public void resetPressed()
            {
                //reset Board and Grab new Wrd

                guessMade = string.Empty;
            }

            public void enterPressed(Wordle wrdle)
            {
                wrdle.IsCorrect(guessMade);
            }

            public void deletePressed()
            {
                if (guessMade.Length > 0)
                {
                    guessMade = guessMade.Remove(guessMade.Length - 1);
                }
            }

            public void customWordEntered(Wordle wrdle, string CustomWrd)
            {
                wrdle.CustomWordEntered(CustomWrd);
            }

            public string DegbugPrint
            {
                get
                {
                    return guessMade;
                }
            }

            public Keyboard()
            {

            }

        }

        private class Display : Keyboard
        {
            public Label[,] DisplayLabels = new Label[5, 6];
            
            public Display()
            {
                int xpad = 13;
                int ypad = 13;
                int x = 0;
                int y = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        DisplayLabels[i, j] = new Label();
                        DisplayLabels[i, j].Text = " ";
                        DisplayLabels[i, j].BackColor = Color.Black;
                        DisplayLabels[i, j].BackColor = Color.White;
                        DisplayLabels[i, j].AutoSize = false;
                        DisplayLabels[i, j].Size = new Size(50, 50);
                        x += 50;
                        
                        DisplayLabels[i, j].Location = new Point(x + (i * xpad), y + (j * ypad));

                    }
                    y += 50;
                }
            }


        }   
    
        
        

        // 
        // Temp Code
        //

        private readonly Keyboard testBoard = new Keyboard();
        private readonly Display testDisplay = new Display();
        private readonly Wordle testWordle = new Wordle();

        //
        //
        //


        public WordleForm()
        {
            
            InitializeComponent(testDisplay.DisplayLabels, testBoard.KeyboardLabels);
            InitDisplay();
            InitKeyboard();
        }

        // Get Keyboard KeyStoke
        string lastKey;
        private void WordleForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lastKey != e.KeyChar.ToString().ToUpper())
            {
                if (e.KeyChar == (char)Keys.Back)
                {
                    testBoard.deletePressed();
                }
                else if (e.KeyChar == (char)Keys.Enter)
                {
                    testBoard.enterPressed(testWordle);
                }
                else if (e.KeyChar == (char)Keys.Escape)
                {
                    testBoard.resetPressed();
                }
                else
                {
                    testBoard.keyPressed(e.KeyChar.ToString().ToUpper());
                }

                if (testBoard.DegbugPrint.Length == 5)
                {
                MessageBox.Show($"Form.KeyPress: '{testBoard.DegbugPrint}' pressed.");
                }
                lastKey = e.KeyChar.ToString().ToUpper();
            }
        }


        private void WordleForm_KeyUp(object sender, KeyEventArgs e)
        {
            lastKey = "";
        }

    }
}
