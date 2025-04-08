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
                switch(Correctness)
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
                if (guessMade.Length < 0) { guessMade += Key; }
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

            }

            public void customWordEntered()
            {

            }

            public Keyboard()
            {

            }
                
        }

        private class Display : Keyboard
        {
            public System.Windows.Forms.Label[,] DisplayLabels = new System.Windows.Forms.Label[5, 6];
            
        }
        
        

        // 
        // Temp Code
        //

        private readonly Keyboard testBoard = new Keyboard();
        private readonly Display testDisplay = new Display();

        //
        //
        //


        public WordleForm()
        {
            
            InitializeComponent(testDisplay.DisplayLabels, testBoard.KeyboardLabels);
        }

        // Get Keyboard KeyStoke
        string lastKey;
        private void WordleForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lastKey != e.KeyChar.ToString().ToUpper())
            {
                MessageBox.Show($"Form.KeyPress: '{e.KeyChar.ToString().ToUpper()}' pressed.");
                lastKey = e.KeyChar.ToString().ToUpper();
            }
        }


        private void WordleForm_KeyUp(object sender, KeyEventArgs e)
        {
            lastKey = "";
        }
        //
    }
}
