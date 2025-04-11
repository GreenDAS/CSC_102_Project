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
            // put outside of class to make it easier to access

            protected static string[] guessesMade = new string[GUESSES_ALLOWED];

            protected static string currentGuess = string.Empty;

            protected static int currentTimeGuessing = 1;

            public enum Correctness
            {
                notInWord = 0,
                inWord = 1,
                correctPlace = 2,
            }

            public static Correctness[] guessColorMap = new Correctness[WORD_LENGTH];

        }



        private class Wordle : GuessHandler
        {
            private string correctWord = "APPLE";

            public void IsCorrect()
            {
                if (correctWord.ToUpper() == currentGuess.ToUpper())
                {
                    for (int i = 0; i < 5; i++)
                    {
                        guessColorMap[i] = Correctness.correctPlace;
                    }
                    //End Game
                }
                else
                {
                    for (int i = 0; i < guessColorMap.Length; i++)
                    {
                        guessColorMap[i] = Correctness.notInWord;
                    }
                    for (int i = 0; i < correctWord.Length; i++)
                    {
                        for (int i2 = 0; i2 < currentGuess.Length; i2++)
                        {
                            if (correctWord.ToUpper()[i] == currentGuess.ToUpper()[i2] & Correctness.inWord > guessColorMap[i2])
                            {
                                guessColorMap[i2] = Correctness.inWord;
                            }
                            else if (Correctness.notInWord == guessColorMap[i2])
                            {
                                guessColorMap[i2] = Correctness.notInWord;
                            }
                        }
                        if (correctWord.ToUpper()[i] == currentGuess.ToUpper()[i])
                        {
                            guessColorMap[i] = Correctness.correctPlace;
                        }
                    }
                }
            }

            public void CustomWordEntered(string CustomWord)
            {
                correctWord = CustomWord;
            }



            public void ResetGame()
            {
                correctWord = "APPLE";
                currentGuess = string.Empty;
                for (int i = 0; i < guessesMade.Length; i++)
                {
                    guessesMade[i] = string.Empty;
                }
                for (int i = 0; i < guessColorMap.Length; i++)
                {
                    guessColorMap[i] = Correctness.notInWord;
                }
                currentTimeGuessing = 1;
            }
        }



        private class Keyboard : GuessHandler
        {
            protected static System.Windows.Forms.Label[][] KeyboardLabels;

            public void ChangeColor(Label LabelToUpdate, Correctness Correctness)
            {
                
                switch (Correctness)
                {
                    case Correctness.notInWord:
                        if (LabelToUpdate.BackColor != Color.FromArgb(200, 222, 222, 33) | LabelToUpdate.BackColor != Color.FromArgb(200, 0, 255, 0))
                        {
                            LabelToUpdate.BackColor = Color.FromArgb(200, 200, 200, 200);
                        }
                        break;

                    case Correctness.inWord:
                        if (LabelToUpdate.BackColor != Color.FromArgb(200, 0, 255, 0))
                        {
                            LabelToUpdate.BackColor = Color.FromArgb(200, 222, 222, 33);
                        }
                        break;

                    case Correctness.correctPlace:
                        LabelToUpdate.BackColor = Color.FromArgb(200, 0, 255, 0);
                        break;

                    default:
                        MessageBox.Show("No Known Correct Location Error");
                        break;

                }
            }

            public void KeyPressed(string Key)
            {
                if (currentGuess.Length < WORD_LENGTH) { currentGuess += Key; }
            }

            public void ResetPressed(Wordle wrdl, Display disp)
            {
                //reset Board and Grab new Wrd
                wrdl.ResetGame();
                disp.RefreshWholeDisplay();

            }

            public void EnterPressed(Wordle wrdle, Display disp)
            {

                for (int i = 0; i < guessesMade.Length; i++)
                {
                    if (guessesMade[i] == currentGuess)
                    {
                        MessageBox.Show("Word Already Guessed");
                        return;
                    }
                }
                if (currentGuess.Length != WORD_LENGTH)
                {
                    MessageBox.Show("Word is not the correct length");
                    return;
                }

                wrdle.IsCorrect();

                // Find Label and Update Color

                bool labelFoundFlag = false;
                for (int i = 0; i < currentGuess.Length; i++)
                {
                    for (int j = 0; j < KeyboardLabels.Length; j++)
                    {
                        for (int k = 0; k < KeyboardLabels[j].Length; k++)
                        {
                            if (KeyboardLabels[j][k].Text.ToUpper() == currentGuess[i].ToString().ToUpper())
                            {
                                ChangeColor(KeyboardLabels[j][k], guessColorMap[i]);
                                labelFoundFlag = true;
                                break;
                            }
                        }
                        if (labelFoundFlag)
                        {
                            labelFoundFlag = false;
                            break;
                        }
                    }
                }



                // Find Display Label and Update Color
                disp.ChangeColor();
                guessesMade[currentTimeGuessing - 1] = currentGuess;
                currentTimeGuessing++;
                currentGuess = string.Empty;
            }

            public void DeletePressed()
            {
                if (currentGuess.Length > 0)
                {
                    currentGuess = currentGuess.Remove(currentGuess.Length - 1);
                }
            }

            public void CustomWordEntered(Wordle wrdle, string CustomWrd)
            {
                wrdle.CustomWordEntered(CustomWrd);
            }

            public string DegbugPrint
            {
                get
                {
                    return currentGuess;
                }
            }

            public Keyboard(Label[][] keyboardLabels)
            {
                KeyboardLabels = keyboardLabels;
            }

            public Keyboard()
            {
            }

        }



        private class Display : Keyboard
        {
            public static Label[,] DisplayLabels;


            public void ChangeColor()
            {
                for (int i = 0; i < guessColorMap.Length; i++)
                {
                    Label LabelToUpdate = DisplayLabels[i, currentTimeGuessing - 1];
                    switch (guessColorMap[i])
                    {
                        case Correctness.notInWord:
                            LabelToUpdate.BackColor = Color.FromArgb(200, 200, 200, 200);
                            break;

                        case Correctness.inWord:
                            LabelToUpdate.BackColor = Color.FromArgb(200, 222, 222, 33);
                            break;

                        case Correctness.correctPlace:
                            LabelToUpdate.BackColor = Color.FromArgb(200, 0, 255, 0);
                            break;

                        default:
                            MessageBox.Show("No Known Correct Location Error");
                            break;

                    }
                }
                
            }


            public void UpdateDisplay()
            {
                for (int i = 0; i < WORD_LENGTH; i++)
                {
                    if (currentTimeGuessing > GUESSES_ALLOWED)
                    {
                        break;
                    }
                    else
                    {
                        try
                        {
                            DisplayLabels[i, currentTimeGuessing - 1].Text = currentGuess[i].ToString();
                        }
                        catch (IndexOutOfRangeException)
                        {
                            DisplayLabels[i, currentTimeGuessing - 1].Text = " ";
                        }
                    }
                }
            }



            public void RefreshWholeDisplay()
            {
                
                for (int i = 0; i < DisplayLabels.GetLength(0); i++)
                {
                    for (int j = 0; j < DisplayLabels.GetLength(1) -1; j++)
                    {
                        DisplayLabels[j, i].BackColor = Color.FromArgb(0,0,0,0);
                        DisplayLabels[j, i].Text = " ";
                    }
                }
                for (int i = 0; i < KeyboardLabels.Length; i++)
                {
                    for (int j = 0; j < KeyboardLabels[i].Length; j++)
                    {
                        KeyboardLabels[i][j].BackColor = Color.FromArgb(0, 0, 0, 0);
                    }
                }
                
            }


            public Display(Label[,] dispLabels)
            {
                DisplayLabels = dispLabels;
            }


        }



        // 
        // Temp Code
        //

        private Keyboard testBoard;
        private Display testDisplay;
        private Wordle testWordle;

        //
        //
        //



        public WordleForm()
        {
            InitializeComponent();
            InitCustomWord();
            testDisplay = new Display(InitDisplay());
            testBoard = new Keyboard(InitKeyboard());
            testWordle = new Wordle();
        }



        // Get Keyboard KeyStoke
        string lastKey;



        private void WordleForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lastKey != e.KeyChar.ToString().ToUpper())
            {
                if (e.KeyChar == (char)Keys.Back | (char)Keys.Delete == e.KeyChar)
                {
                    testBoard.DeletePressed();
                }
                else if (e.KeyChar == (char)Keys.Enter)
                {
                    testBoard.EnterPressed(testWordle, testDisplay);
                }
                else if (e.KeyChar == (char)Keys.Escape)
                {
                    testBoard.ResetPressed(testWordle, testDisplay);
                }
                else
                {
                    testBoard.KeyPressed(e.KeyChar.ToString().ToUpper());
                }
                testDisplay.UpdateDisplay();
            }
        }



        private void WordleForm_KeyUp(object sender, KeyEventArgs e)
        {
            lastKey = "";
        }



        private void WordleForm_Keyboard_Click(object sender, EventArgs e)
        {
            Label clickedLabel = (Label)sender;
            if (clickedLabel.Text == "DEL")
            {
                testBoard.DeletePressed();
            }
            else if (clickedLabel.Text == "ENTER")
            {
                testBoard.EnterPressed(testWordle, testDisplay);
            }
            else if (clickedLabel.Text == "RESET")
            {
                testBoard.ResetPressed(testWordle, testDisplay);
            }
            else
            {
                testBoard.KeyPressed(clickedLabel.Text.ToUpper());
            }
            testDisplay.UpdateDisplay();
        }

        private void WordleForm_CustomWordEnableButton_Click(object sender, EventArgs e)
        {
            this.KeyPreview = !this.KeyPreview;
            CustomWordtextBox.Enabled = !CustomWordtextBox.Enabled;
            CustomWordButton.Enabled = !CustomWordButton.Enabled;
            CustomWordButton.Visible = !CustomWordButton.Visible;
        }

        private void WordleForm_CustomWordButton_Click(object sender, EventArgs e)
        {
            //string customWord = Microsoft.VisualBasic.Interaction.InputBox("Enter a custom word", "Custom Word", "APPLE");
            //testBoard.CustomWordEntered(testWordle, customWord);
            //testDisplay.RefreshWholeDisplay();
        }
    }
}
