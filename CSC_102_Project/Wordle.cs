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



        #region Class Delcarations
        #region GuessHandler Class
        /// <summary>
        /// GuessHandler Class
        /// Defines static variables that all other classes can access
        /// </summary>
        private class GuessHandler
        {

            protected static string[] guessesMade = new string[GUESSES_ALLOWED];

            protected static string currentGuess = string.Empty;

            protected static string currentTempCustomWord = string.Empty;

            protected static int currentTimeGuessing = 1;

            protected static int timesWon = 0;

            protected static int timesLost = 0;

            public enum Correctness
            {
                notInWord = 0,
                inWord = 1,
                correctPlace = 2,
            }

            public static Correctness[] guessColorMap = new Correctness[WORD_LENGTH];

        }
        #endregion


        #region Wordle Class
        /// <summary>
        /// Wordle Class
        /// Handles all the guessing logic and communicating with the Filemanager
        /// Only class that knows of the correct word
        /// </summary>
        private class Wordle : GuessHandler
        {
            private string correctWord = "APPLE";
            public TextBox CustomWordTextBox;
            

            public bool IsCorrect()
            {
                if (correctWord.ToUpper() == currentGuess.ToUpper())
                {
                    for (int i = 0; i < 5; i++)
                    {
                        guessColorMap[i] = Correctness.correctPlace;
                    }
                    return true;

                }
                else
                {
                    Dictionary<char, int> correctPlaces = new Dictionary<char, int>();
                    foreach (char c in correctWord.ToUpper())
                    {
                        // Check each value nad check to see if the correct place has been found yet or not for this letter
                        if (!correctPlaces.ContainsKey(c))
                        {
                            correctPlaces.Add(c, 1);
                        }
                        else
                        {
                            // If the letter is already in the dictionary, then increment the value by 1
                            // This will allow us to keep track of how many times the letter appears in the word
                            // This will also allow us to keep track of how many times the letter has been guessed
                            // This will also allow us to keep track of how many times the letter has been guessed in the correct place
                            correctPlaces[c]++;
                        }
                    }
                    for (int i = 0; i < guessColorMap.Length; i++)
                    {
                        guessColorMap[i] = Correctness.notInWord;
                    }
                    for (int i = 0; i < correctWord.Length; i++)
                    {
                        if (correctWord.ToUpper()[i] == currentGuess.ToUpper()[i])
                        {
                            guessColorMap[i] = Correctness.correctPlace;
                            correctPlaces[correctWord.ToUpper()[i]]--;
                            continue;
                        }
                        for (int i2 = 0; i2 < currentGuess.Length; i2++)
                        {
                            // If the letter is in the correct place, then skip this letter and move to the next one
                            // This will prevent double counting letters that are in the correct place
                            // This will also prevent double counting letters that are not in the correct place
                            // This will also prevent double counting letters that are not in the word at all
                            // This will also prevent double counting letters that are in the word but not in the correct place

                            if (correctWord.ToUpper()[i] == currentGuess.ToUpper()[i2] & correctPlaces[correctWord.ToUpper()[i]] > 0 & guessColorMap[i2] < Correctness.correctPlace)
                            {
                                guessColorMap[i2] = Correctness.inWord;
                                correctPlaces[correctWord.ToUpper()[i]]--;
                            }
                            else if (Correctness.notInWord == guessColorMap[i2])
                            {
                                guessColorMap[i2] = Correctness.notInWord;
                            }
                        }
                        
                    }
                }
                return false;
            }

            public void CustomWordEntered(string CustomWord)
            {
                correctWord = CustomWord;
            }



            public void DebugResetGame()
            {
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

            public void LoadNewWord()
            {
                // Load new word from file
            }

            public Wordle(TextBox customWordTextBox)
            {
                CustomWordTextBox = customWordTextBox;
            }
        }
        #endregion


        #region Keyboard Class
        /// <summary>
        /// Keyboard Class
        /// Handles all keypresses and update the keyboard colors
        /// </summary>
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

            public void KeyPressed(string Key, bool IsCustomWordEnabled)
            
            {
                if (currentGuess.Length < WORD_LENGTH & !IsCustomWordEnabled) { currentGuess += Key; }
                else if (currentTempCustomWord.Length < WORD_LENGTH & IsCustomWordEnabled) { currentTempCustomWord += Key; }
            }

            public void ResetPressed(Wordle wrdl, Display disp)
            {
                //reset Board and Grab new Wrd
                wrdl.DebugResetGame();
                disp.RefreshWholeDisplay();
                currentTempCustomWord = string.Empty;
            }

            public void LoadPressed(Wordle wrdl, Display disp)
            {
                //Load new Wrd
                wrdl.DebugResetGame();
                wrdl.LoadNewWord();
                disp.RefreshWholeDisplay();
                currentTempCustomWord = string.Empty;
            }

            public void EnterPressed(Wordle wrdle, Display disp, bool IsCustomWordEnabled)
            {
                if (!IsCustomWordEnabled)
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
                }
                else
                {
                    if (currentTempCustomWord.Length != WORD_LENGTH)
                    {
                        MessageBox.Show("Word is not the correct length");
                        return;
                    }
                    CustomWordEntered(wrdle, currentTempCustomWord);
                    currentTempCustomWord = string.Empty;


                    return;
                }


                bool guessIsCorrect = wrdle.IsCorrect();

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

                if (guessIsCorrect)
                {
                    MessageBox.Show($"You Win! The word was {currentGuess.ToUpper()}! \nClick 'OK' to Play Again!");
                    timesWon++;
                    return;
                }
                else if (currentTimeGuessing >= GUESSES_ALLOWED & !guessIsCorrect)
                {
                    MessageBox.Show($"You Lose! The word was {currentGuess.ToUpper()}! \nClick 'OK' to Play Again!");
                    timesLost++;
                    return;
                }

                guessesMade[currentTimeGuessing - 1] = currentGuess;
                currentTimeGuessing++;
                currentGuess = string.Empty;
            }

            public void DeletePressed(bool IsCustomWordEnabled)
            {
                if (currentGuess.Length > 0 & !IsCustomWordEnabled)
                {
                    currentGuess = currentGuess.Remove(currentGuess.Length - 1);
                }
                else if (currentTempCustomWord.Length > 0 & IsCustomWordEnabled)
                {
                    currentTempCustomWord = currentTempCustomWord.Remove(currentTempCustomWord.Length - 1);
                }
            }

            public void ClearPressed(bool IsCustomWordEnabled)
            {
                if (!IsCustomWordEnabled)
                {
                    currentGuess = string.Empty;
                }
                else
                {
                    currentTempCustomWord = string.Empty;
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
        #endregion


        #region Display Class
        /// <summary>
        /// Display Class
        /// Handles all display refreshes and updates, Except for changing the color of the keyboard
        /// </summary>
        private class Display : Keyboard
        {
            public Label[,] DisplayLabels;
            public Label[,] ScoreLabels;

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
                ScoreLabels[1, 0].Text = $"{timesWon}";
                ScoreLabels[1, 1].Text = $"{timesLost}";
            }


            public void UpdateDisplay(TextBox customWordTextBox)
            {
                customWordTextBox.Text = currentTempCustomWord;
            }



            public void RefreshWholeDisplay()
            {
                
                for (int i = 0; i < GUESSES_ALLOWED; i++)
                {
                    for (int j = 0; j < WORD_LENGTH; j++)
                    {
                        DisplayLabels[j, i].BackColor = Color.FromArgb(255,0,0,0);
                        DisplayLabels[j, i].Text = " ";
                    }
                }
                for (int i = 0; i < KeyboardLabels.Length; i++)
                {
                    for (int j = 0; j < KeyboardLabels[i].Length; j++)
                    {
                        KeyboardLabels[i][j].BackColor = Color.FromArgb(255, 0, 0, 0);
                    }
                }
                
            }


            public Display(Label[,] dispLabels, Label[,] scoreLabels)
            {
                DisplayLabels = dispLabels;
                ScoreLabels = scoreLabels;
            }


        }
        #endregion
        #endregion


        #region Form Variables
        /// <summary>
        /// Init Display, Keyboard, and Wordle Vars
        /// </summary>
        private Keyboard testBoard;
        private Display testDisplay;
        private Wordle testWordle;



        /// <summary>
        /// Last Keyboard KeyStoke
        /// </summary>
        string lastKey;



        /// <summary>
        /// Is a Cutom Word being Entered
        /// </summary>
        bool isCustomWordEnabled = false;
        #endregion


        #region Form Init
        /// <summary>
        /// Initializes the Form and all of the controls and appropriate variables/classes
        /// </summary>
        public WordleForm()
        {
            InitializeComponent();
            
            testDisplay = new Display(InitDisplay(), InitScoreBoard());
            testBoard = new Keyboard(InitKeyboard());
            testWordle = new Wordle(InitCustomWord());
        }
        #endregion


        #region Keyboard Key Press Event Handlers
        /// <summary>
        /// Key Press Event Handler
        /// Checks to see if the key pressed is a special key or a letter and clals the appropriate method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordleForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {
                return;
            }
            if (lastKey != e.KeyChar.ToString().ToUpper())
            {
                if (e.KeyChar == (char)Keys.Back | (char)Keys.Delete == e.KeyChar)
                {
                    testBoard.DeletePressed(isCustomWordEnabled);
                }
                else if (e.KeyChar == (char)Keys.Enter)
                {
                    testBoard.EnterPressed(testWordle, testDisplay, isCustomWordEnabled);
                    if (isCustomWordEnabled) { ToggleCustomControls(); }
                }
                else if (e.KeyChar == (char)Keys.Escape)
                {
                    testBoard.ResetPressed(testWordle, testDisplay);
                    if (isCustomWordEnabled) { ToggleCustomControls(); }
                }
                else
                {
                    testBoard.KeyPressed(e.KeyChar.ToString().ToUpper(), isCustomWordEnabled);
                }
                if (isCustomWordEnabled) { testDisplay.UpdateDisplay(testWordle.CustomWordTextBox);
                }
                else
                {
                    testDisplay.UpdateDisplay();
                }
            }
        }


        
        /// <summary>
        /// Key Up Event Handler
        /// Checks to see if the key has been released (denotes a click of a key)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordleForm_KeyUp(object sender, KeyEventArgs e)
        {
            lastKey = "";
        }



        /// <summary>
        /// Keyboard Click Event Handler
        /// Checks to see if key press was a special key or a letter and calls the appropriate method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordleForm_Keyboard_Click(object sender, EventArgs e)
        {
            Label clickedLabel = (Label)sender;

            if (clickedLabel.Text == "DEL")
            {
                testBoard.DeletePressed(isCustomWordEnabled);
            }
            else if (clickedLabel.Text == "ENTER")
            {
                testBoard.EnterPressed(testWordle, testDisplay, isCustomWordEnabled);
                if (isCustomWordEnabled) { ToggleCustomControls(); }
            }
            else if (clickedLabel.Text == "CLEAR")
            {
                testBoard.ClearPressed(isCustomWordEnabled);
            }
            else if (clickedLabel.Text == "RESET")
            {
                testBoard.ResetPressed(testWordle, testDisplay);
                if (isCustomWordEnabled) { ToggleCustomControls(); }

            }
            else if (clickedLabel.Text == "LOAD")
            {
                testBoard.LoadPressed(testWordle, testDisplay);
                if (isCustomWordEnabled) { ToggleCustomControls(); }
            }
            else
            {
                testBoard.KeyPressed(clickedLabel.Text.ToUpper(), isCustomWordEnabled);
            }
            if (isCustomWordEnabled)
            {
                testDisplay.UpdateDisplay(testWordle.CustomWordTextBox);
            }
            else
            {
                testDisplay.UpdateDisplay();
            }
        }
        #endregion


        #region Toggle Custom Word Controls Method
        /// <summary>
        /// Toggles the Custom Word Controls
        /// </summary>
        private void ToggleCustomControls()
        {
            CustomWordtextBox.Visible = !CustomWordtextBox.Visible;
            isCustomWordEnabled = !isCustomWordEnabled;
            CustomWordButton.Enabled = !CustomWordButton.Enabled;
            CustomWordButton.Visible = !CustomWordButton.Visible;
            testWordle.CustomWordTextBox.Text = string.Empty;
        }
        #endregion


        #region CustomWord Event Hanldlers
        /// <summary>
        /// Tells the Program that the user is typig in a custom word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordleForm_CustomWordEnableButton_Click(object sender, EventArgs e)
        {
            KeyPreview = true;
            if (CustomWordtextBox.MaxLength == 5) 
            {
                CustomWordtextBox.MaxLength = 1;
            }
            else
            {
                CustomWordtextBox.MaxLength = 5;
            }

            ToggleCustomControls();
        }



        /// <summary>
        /// Tells the program that the user has entered a custom word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordleForm_CustomWordButton_Click(object sender, EventArgs e)
        {
            if (CustomWordtextBox.Text.Length != 5)
            {
                MessageBox.Show("Word is not the correct length");
                return;
            }
            ToggleCustomControls();
            string customWord = testWordle.CustomWordTextBox.Text.ToUpper();
            testWordle.CustomWordTextBox.Text = string.Empty;
            testWordle.DebugResetGame();
            testBoard.CustomWordEntered(testWordle, customWord);
            testDisplay.RefreshWholeDisplay();

        }
        #endregion


    }
}
