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
            protected static string currentGuess = string.Empty;

            protected static int currentTimeGuessing = 1;

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
            private string correctWord = "APPLE";

            public bool IsCorrect()
            {
                bool errorFlag = false;
                if (currentGuess.Length != correctWord.Length)
                {
                    MessageBox.Show("Word is not the correct length");
                    errorFlag = true;
                }
                else if (correctWord.ToUpper() == currentGuess.ToUpper())
                {
                    for (int i = 0; i<5;  i++)
                    {
                        guessColorMap[i] = correctness.correctPlace;
                    }
                    //End Game
                }
                else
                {
                    for (int i = 0; i < guessColorMap.Length; i++)
                    {
                        guessColorMap[i] = correctness.notInWord;
                    }
                    for (int i = 0; i < correctWord.Length; i++)
                    {
                        for (int i2 = 0; i2 < currentGuess.Length; i2++)
                        {
                            if (correctWord.ToUpper()[i] == currentGuess.ToUpper()[i2] & correctness.inWord > guessColorMap[i2])
                            {
                                guessColorMap[i2] = correctness.inWord;
                            }
                            else if (correctness.notInWord == guessColorMap[i2])
                            {
                                guessColorMap[i2] = correctness.notInWord;
                            }
                        }
                        if (correctWord.ToUpper()[i] == currentGuess.ToUpper()[i])
                        {
                            guessColorMap[i] = correctness.correctPlace;
                        }
                    }
                }
                return errorFlag;

            }

            public void CustomWordEntered(string CustomWord)
            {
                correctWord = CustomWord;
            }
        }



        private class Keyboard : GuessHandler
        {
            protected static System.Windows.Forms.Label[][] KeyboardLabels;

            public void changeColor(Label LabelToUpdate, correctness Correctness)
            {
                
                switch (Correctness)
                {
                    case correctness.notInWord:
                        if (LabelToUpdate.BackColor != Color.FromArgb(200, 222, 222, 33) | LabelToUpdate.BackColor != Color.FromArgb(200, 0, 255, 0))
                        {
                            LabelToUpdate.BackColor = Color.FromArgb(200, 200, 200, 200);
                        }
                        break;

                    case correctness.inWord:
                        if (LabelToUpdate.BackColor != Color.FromArgb(200, 0, 255, 0))
                        {
                            LabelToUpdate.BackColor = Color.FromArgb(200, 222, 222, 33);
                        }
                        break;

                    case correctness.correctPlace:
                        LabelToUpdate.BackColor = Color.FromArgb(200, 0, 255, 0);
                        break;

                    default:
                        MessageBox.Show("No Known Correct Location Error");
                        break;

                }
            }

            public void keyPressed(string Key)
            {
                if (currentGuess.Length < 5) { currentGuess += Key; }
            }

            public void resetPressed()
            {
                //reset Board and Grab new Wrd

                currentGuess = string.Empty;
            }

            public void enterPressed(Wordle wrdle, Display disp)
            {
                
                bool errorFlag = wrdle.IsCorrect();

                // Find Label and Update Color
                if (!errorFlag)
                {
                    bool labelFoundFlag = false;
                    for (int i = 0; i < currentGuess.Length; i++)
                    {
                        for (int j = 0; j < KeyboardLabels.Length; j++)
                        {
                            for (int k = 0; k < KeyboardLabels[j].Length; k++)
                            {
                                if (KeyboardLabels[j][k].Text.ToUpper() == currentGuess[i].ToString().ToUpper())
                                {
                                    changeColor(KeyboardLabels[j][k], guessColorMap[i]);
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

                        // Find Display Label and Update Color
                        disp.changeColor();
                    }
                    currentTimeGuessing++;
                    currentGuess = string.Empty;
                }

            }

            public void deletePressed()
            {
                if (currentGuess.Length > 0)
                {
                    currentGuess = currentGuess.Remove(currentGuess.Length - 1);
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


            public void changeColor()
            {
                for (int i = 0; i < guessColorMap.Length; i++)
                {
                    Label LabelToUpdate = DisplayLabels[i, currentTimeGuessing - 1];
                    switch (guessColorMap[i])
                    {
                        case correctness.notInWord:
                            LabelToUpdate.BackColor = Color.FromArgb(200, 200, 200, 200);
                            break;

                        case correctness.inWord:
                            LabelToUpdate.BackColor = Color.FromArgb(200, 222, 222, 33);
                            break;

                        case correctness.correctPlace:
                            LabelToUpdate.BackColor = Color.FromArgb(200, 0, 255, 0);
                            break;

                        default:
                            MessageBox.Show("No Known Correct Location Error");
                            break;

                    }
                }
                
            }


            public void updateDisplay()
            {
                for (int i = 0; i < 5; i++)
                {
                    if (currentTimeGuessing > 6)
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
                    testBoard.deletePressed();
                }
                else if (e.KeyChar == (char)Keys.Enter)
                {
                    testBoard.enterPressed(testWordle, testDisplay);
                }
                else if (e.KeyChar == (char)Keys.Escape)
                {
                    testBoard.resetPressed();
                }
                else
                {
                    testBoard.keyPressed(e.KeyChar.ToString().ToUpper());
                }
                testDisplay.updateDisplay();
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
                testBoard.deletePressed();
            }
            else if (clickedLabel.Text == "ENTER")
            {
                testBoard.enterPressed(testWordle, testDisplay);
            }
            else if (clickedLabel.Text == "RESET")
            {
                testBoard.resetPressed();
            }
            else
            {
                testBoard.keyPressed(clickedLabel.Text.ToUpper());
            }
        }
    }
}
