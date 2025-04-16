using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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



        /// <summary>
        /// File Manager Class
        /// </summary>
        private class FileManager
        {
            private string filePath = "wordList.txt";
            private WordleForm myWordleForm;
            private OpenFileDialog findWordListDialog = new OpenFileDialog();
            private Dictionary<string, string> wordleWords = new Dictionary<string, string>();



            /// <summary>
            /// Checks to see if the word is in the dictionary
            /// </summary>
            /// <param name="guess"></param>
            /// <returns></returns>
            public bool IsValid(string guess)
            {
                return wordleWords.ContainsKey(guess.ToUpper());
            }


            /// <summary>
            /// Reads the word list from the file and parses it into a dictionary
            /// </summary>
            private void ReadWordList()
            {
                {
                    // Load the file and parse the words into a dictionary
                    bool fileFound = false;
                    while (!fileFound)
                    {
                        try
                        {
                            using (StreamReader inputFile = new StreamReader(filePath))
                            {
                                while (!inputFile.EndOfStream)
                                {
                                    string line = inputFile.ReadLine();
                                    string[] parts = line.Split(',');
                                    if (parts.Length == 2)
                                    {
                                        fileFound = true;
                                        wordleWords.Add(parts[0], parts[1]);
                                    }
                                    else
                                    {
                                        MessageBox.Show("File is not in the correct format");
                                        if (findWordListDialog.ShowDialog() == DialogResult.OK)
                                        {
                                            filePath = findWordListDialog.FileName;
                                        }
                                        else
                                        {
                                            myWordleForm.Close();
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        catch (FileNotFoundException)
                        {
                            MessageBox.Show("File not found");
                            if (findWordListDialog.ShowDialog() == DialogResult.OK)
                            {
                                filePath = findWordListDialog.FileName;
                            }
                            else
                            {
                                myWordleForm.Close();
                            }
                        }
                        catch (IOException)
                        {
                            MessageBox.Show("File is not in the correct format");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred: {ex.Message}");
                        }
                    }
                }
            }



            /// <summary>
            /// stores the word list to the file
            /// </summary>
            public void StoreWordList()
            {
                bool fileFound = false;
                while (!fileFound)
                {
                    try
                    {
                        using (StreamWriter outputFile = new StreamWriter(filePath))
                        {
                            foreach (KeyValuePair<string, string> kvp in wordleWords)
                            {
                                outputFile.WriteLine($"{kvp.Key},{kvp.Value}");
                                fileFound = true;
                            }
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("File not found");
                        if (findWordListDialog.ShowDialog() == DialogResult.OK)
                        {
                            filePath = findWordListDialog.FileName;
                        }
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("File is not in the correct format");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }
            }



            /// <summary>
            /// Gets a random word from the dictionary
            /// </summary>
            /// <returns></returns>
            public string GetWord()
            {

                if (wordleWords.Count == 0 | wordleWords == null)
                {
                    ReadWordList();
                }
                int lowestLeastTimesUsed = int.Parse(wordleWords.Values.Min());
                bool suitableWordFound = false;
                while (!suitableWordFound)
                {
                    Random rand = new Random();
                    int index = rand.Next(0, wordleWords.Count);
                    string word = wordleWords.ElementAt(index).Key;
                    if (int.Parse(wordleWords[word]) == lowestLeastTimesUsed)
                    {
                        wordleWords[word] = (int.Parse(wordleWords[word]) + 1).ToString();
                        myWordleForm.Focus();
                        return word;
                    }
                }
                return null;
            }



            /// <summary>
            /// FileManager Constructor When No file is Given
            /// </summary>
            public FileManager(OpenFileDialog fileDialog, WordleForm MyWordleForm)
            {
                findWordListDialog = fileDialog;
                myWordleForm = MyWordleForm;
            }

        }


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

            protected static bool justWonOrLost = false;

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
            private string correctWord;

            public TextBox CustomWordTextBox;

            private FileManager myFileManager;

            #region Game Logic Methods
            /// <summary>
            /// Checks to see if the current guess is correct and maps the colors to the guessColorMap
            /// </summary>
            /// <returns>True if Correct, False Otherwise</returns>
            public bool[] IsCorrect()
            {
                bool[] returnVariable = new bool[2]; // [0] = IsCorrect, [1] = Error?
                returnVariable[0] = false;
                returnVariable[1] = false;
                if (correctWord.ToUpper() == currentGuess.ToUpper())
                {
                    for (int i = 0; i < 5; i++)
                    {
                        guessColorMap[i] = Correctness.correctPlace;
                        returnVariable[0] = true;
                        returnVariable[1] = false;
                    }
                    return returnVariable;

                }
                else
                {
                    if (!myFileManager.IsValid(currentGuess))
                    {
                        returnVariable[0] = false;
                        returnVariable[1] = true;
                        return returnVariable;
                    }
                    Dictionary<char, int> correctPlaces = new Dictionary<char, int>();
                    foreach (char c in correctWord.ToUpper())
                    {
                        // Check each value and check to see if the correct place has been found yet or not for this letter
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
                return returnVariable;
            }
            #endregion

            #region Game Start/Stop/Debug Methods
            /// <summary>
            /// Updates correct word to the custom word entered by the user
            /// </summary>
            /// <param name="CustomWord"></param>
            public void CustomWordEntered(string CustomWord)
            {
                correctWord = CustomWord;
            }


            /// <summary>
            /// Debug Method to reset the game board and all the variables except the correct word
            /// </summary>
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


            /// <summary>
            /// Load a new word from the file
            /// </summary>
            public void LoadNewWord()
            {
                correctWord = myFileManager.GetWord();
                myFileManager.StoreWordList();
            }
            #endregion


            /// <summary>
            /// Wordle Constructor
            /// </summary>
            /// <param name="customWordTextBox"></param>
            public Wordle(TextBox customWordTextBox, FileManager MyFileManager)
            {
                CustomWordTextBox = customWordTextBox;
                myFileManager = MyFileManager;
                LoadNewWord();
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

            #region Key Press Methods
            /// <summary>
            /// Change the color of the keyboard label based on the correctness of the letter
            /// </summary>
            /// <param name="LabelToUpdate"></param>
            /// <param name="Correctness"></param>
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


            /// <summary>
            /// Add a letter to the current guess
            /// </summary>
            /// <param name="Key"></param>
            /// <param name="IsCustomWordEnabled"></param>
            public void KeyPressed(string Key, bool IsCustomWordEnabled)
            
            {
                if (currentGuess.Length < WORD_LENGTH & !IsCustomWordEnabled) { currentGuess += Key; }
                else if (currentTempCustomWord.Length < WORD_LENGTH & IsCustomWordEnabled) { currentTempCustomWord += Key; }
            }


            /// <summary>
            /// Handles Reset Button Pressed
            /// </summary>
            /// <param name="wrdl"></param>
            /// <param name="disp"></param>
            public void ResetPressed(Wordle wrdl, Display disp)
            {
                //reset Board and Grab new Wrd
                wrdl.DebugResetGame();
                disp.RefreshWholeDisplay();
                justWonOrLost = false;
                currentTempCustomWord = string.Empty;
            }


            /// <summary>
            /// Handles Load Button Pressed
            /// </summary>
            /// <param name="wrdl"></param>
            /// <param name="disp"></param>
            public void LoadPressed(Wordle wrdl, Display disp)
            {
                //Load new Wrd
                wrdl.DebugResetGame();
                wrdl.LoadNewWord();
                disp.RefreshWholeDisplay();
                justWonOrLost = false;
                currentTempCustomWord = string.Empty;
            }


            /// <summary>
            /// Handles Enter Button Pressed
            /// </summary>
            /// <param name="wrdle"></param>
            /// <param name="disp"></param>
            /// <param name="IsCustomWordEnabled"></param>
            public void EnterPressed(Wordle wrdle, Display disp, bool IsCustomWordEnabled)
            {
                if (IsCustomWordEnabled)
                {
                    if (currentTempCustomWord.Length != WORD_LENGTH)
                    {
                        MessageBox.Show("Word is not the correct length");
                        return;
                    }

                    CustomWordEntered(wrdle, currentTempCustomWord);
                    ResetPressed(wrdle, disp);

                }
                else if (!justWonOrLost)
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
                    bool[] guessIsCorrect = wrdle.IsCorrect();
                    if (guessIsCorrect[1])
                    {
                        MessageBox.Show("Not a Known Word!");
                        return;
                    }
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

                    if (currentTimeGuessing <= GUESSES_ALLOWED & guessIsCorrect[0])
                    {
                        MessageBox.Show($"You Win! The word was {currentGuess.ToUpper()}! \nClick 'OK' to Play Again!");
                        justWonOrLost = true;
                        timesWon++;
                    }
                    else if (currentTimeGuessing == GUESSES_ALLOWED & !guessIsCorrect[0])
                    {
                        MessageBox.Show($"You Lose! \nClick 'OK' to Play Again!");
                        justWonOrLost = true;
                        timesLost++;
                    }
                    guessesMade[currentTimeGuessing - 1] = currentGuess;
                    currentTimeGuessing++;
                    currentGuess = string.Empty;
                }
            }


            /// <summary>
            /// Handles Delete Button Pressed
            /// </summary>
            /// <param name="IsCustomWordEnabled"></param>
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


            /// <summary>
            /// Handles Clear Button Pressed
            /// </summary>
            /// <param name="IsCustomWordEnabled"></param>
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


            /// <summary>
            /// Handles Custom Word Button Pressed
            /// </summary>
            /// <param name="wrdle"></param>
            /// <param name="CustomWrd"></param>
            public void CustomWordEntered(Wordle wrdle, string CustomWrd)
            {
                wrdle.CustomWordEntered(CustomWrd);
            }
            #endregion

            #region Constructors
            /// <summary>
            /// Keyboard Constructor
            /// </summary>
            /// <param name="keyboardLabels"></param>
            public Keyboard(Label[][] keyboardLabels)
            {
                KeyboardLabels = keyboardLabels;
            }


            /// <summary>
            /// Keyboard Constructor So that no error Occurs with the Display Class
            /// </summary>
            public Keyboard()
            {
            }
            #endregion

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


            /// <summary>
            /// Change the color of the display label based on the correctness of the letter
            /// </summary>
            public void ChangeColor()
            {
                for (int i = 0; i < guessColorMap.Length; i++)
                {
                    if (currentTimeGuessing <= GUESSES_ALLOWED)
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
                
            }


            /// <summary>
            /// Update the display labels with the current guess
            /// </summary>
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


            /// <summary>
            /// Update the display labels with the current custom word
            /// </summary>
            /// <param name="customWordTextBox"></param>
            public void UpdateDisplay(TextBox customWordTextBox)
            {
                customWordTextBox.Text = currentTempCustomWord;
            }


            /// <summary>
            /// Refresh the whole display
            /// </summary>
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
        private FileManager testFileManager;
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
            testFileManager = new FileManager(InitFileDialog(), this);
            testWordle = new Wordle(InitCustomWord(), testFileManager);
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
            foreach (char c in "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")
            {
                if (e.KeyChar == c & lastKey != e.KeyChar.ToString().ToUpper())
                {
                    testBoard.KeyPressed(e.KeyChar.ToString().ToUpper(), isCustomWordEnabled);
                    break;
                }
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
                if (isCustomWordEnabled) 
                { 
                    testDisplay.UpdateDisplay(testWordle.CustomWordTextBox);
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
