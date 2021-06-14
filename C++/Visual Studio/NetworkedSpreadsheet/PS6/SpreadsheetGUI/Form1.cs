///<author>
///William Meldrum u0934535 
///Andrew Thompson u0879848
///Chadd Barney u0964934
/// 10/21/2018
/// </author>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;
using Controller;
using System.Threading;
using SpreadsheetUtilities;

namespace SpreadsheetGUI
{
    public partial class Form1 : Form
    {
        //determines if current theme is dark (light and dark themes available)
        private bool isDark;

        //private backing of the spreadsheet logic
        private SS.Spreadsheet mySpreadsheet;

        private ClientController clientcontroller;


        public Form1()
        {
            InitializeComponent();
            clientcontroller = new ClientController();
            spreadsheetPanel1.SelectionChanged += OnSelectionChanged;
            clientcontroller.receivedLists += PopulateList;
            clientcontroller.receivedPWError += DisplayPasswordError;
            clientcontroller.receivedCells += PopulateCells;
            clientcontroller.receivedCircularError += DisplayCircularError;
            clientcontroller.connected += OnSuccessfulConnection;
            clientcontroller.noServerResponse += LostConnection;
            //set minimum size of spreadsheetPanel1
            spreadsheetPanel1.MinimumSize = new Size(1, 1);
            this.AcceptButton = SetButton;

            PasswordBox.PasswordChar = '*';

            //set name box
            NameBox.Text = GetSelectionName();

            //create new spreadsheet
            mySpreadsheet = new SS.Spreadsheet(s => Regex.IsMatch(s, @"^[A-Z][1-9][0-9]*$"), s => s.ToUpper(), "ps6");
        }


        /// <summary>
        /// Constructor used to keep the current theme consistent when 
        /// opening a new Spreadsheet. Calls the 0 argument constructor first.
        /// </summary>
        /// <param name="isDark">Determines the theme of the new Form.</param>
        public Form1(bool isDark) : this()
        {
            if (isDark)
            {
                darkModeToolStripMenuItem_Click(this, new EventArgs());
            }

        }

        /// <summary>
        /// Handler of the SelectionChanged event. 
        /// </summary>
        /// <param name="ss">The CustomSpreadsheetPanel whose selection was changed.</param>
        private void OnSelectionChanged(CustomSpreadsheetPanel ss)
        {
            //clear and select the ContentsBox
            String cellName = GetSelectionName();
            NameBox.Text = cellName;

            ContentsBox.Text = CellContentsToString(cellName);
            ContentsBox.Focus();

            ValueBox.Text = CellValueToString(cellName);
        }


        /// <summary>
        /// Event when the Set button is clicked. Can be triggered by pressing enter as well.
        /// Saves the contents in the ContentsBox into the cell along with its corresponding 
        /// value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetButton_Click(object sender, EventArgs e)
        {
            //get name of the selected cell
            String cellName = GetSelectionName();

            //ISet to keep track of cells needing to be changed after setbutton is clicked
            List<string> cellsToUpdate;

            if (ContentsBox.Text.StartsWith("="))
            {
                try
                {
                    Formula formula = new Formula(ContentsBox.Text.Substring(1), s => s.ToUpper(), s => Regex.IsMatch(s, @"^[A-Z][1-9][0-9]*$"));
                    cellsToUpdate = formula.GetVariables().ToList<string>();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message + "\n\nCell will not be changed.");
                    return;
                }
            }
            else
            {
                cellsToUpdate = new List<string>();
            }

            ////bad line
            //cellsToUpdate = mySpreadsheet.GetDirectDependents(cellName).ToList<string>();

            clientcontroller.EditMade(cellName, ContentsBox.Text, cellsToUpdate);
        }

        private void SetCell(string cellName, string cellContents)
        {
            //ISet to keep track of cells needing to be changed after setbutton is clicked
            ISet<string> cellsToUpdate;

            try
            {
                cellsToUpdate = mySpreadsheet.SetContentsOfCell(cellName, cellContents);
            }

            //catches any exception thrown during SetContensOfCell. Exception message will be
            //displayed in a message box, the cell value and contents remain unchanged. 
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message + "\n\nCell will not be changed.");
                return;
            }

            //Sets value of the Spreadsheet's ValueBox
            ValueBox.Text = CellValueToString(cellName);

            //updates the values of cells that need to change
            foreach (string name in cellsToUpdate)
            {
                spreadsheetPanel1.SetValue(getColumnFromName(name), getRowFromName(name), CellValueToString(name));
            }
        }

        //HELPER METHODS --------------------------------------------------------------------------------------------

        /// <summary>
        /// Get the name of the current selected cell.
        /// </summary>
        /// <returns>name of the current selected cell.</returns>
        private string GetSelectionName()
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);

            //column letter (add 65 for proper ascii value)
            char colLetter = (char)(col + 65);

            //appends the column letter and the row of the selection
            //(add 1 to row for proper ascii value)
            return colLetter + (row + 1).ToString();
        }

        /// <summary>
        /// Gets a cell's column, represented as an int by 
        /// passing in the name of the cell in question. Done by
        /// converting the string to char[] and returning the first
        /// element - 65 (since columns are 0-based) to convert from char to int.  
        /// </summary>
        /// <param name="cellName">Name of specfied cell.</param>
        /// <returns>int representation of the column name (0-based)</returns>
        private int getColumnFromName(string cellName)
        {

            char[] cellChars = cellName.ToCharArray();

            return cellChars[0] - 65;

        }

        /// <summary>
        /// Helper method that return the row of the passed cellname.
        /// Row is returned as an int. 
        /// </summary>
        /// <param name="cellName">Name of specfied cell.</param>
        /// <returns>int representation of the row (0-based)</returns>
        private int getRowFromName(string cellName)
        {
            //parses everything AFTER the first character
            //subtract 1 for proper int value since row is 0-based
            return int.Parse(cellName.Substring(1)) - 1;

        }

        /// <summary>
        /// Converts the value of the specificed cell to
        /// a string so it can be placed in text boxes. 
        /// </summary>
        /// <param name="cellName">Name of specfied cell.</param>
        /// <returns>The cell value as a string.</returns>
        private string CellValueToString(String cellName)
        {

            object cellValue = mySpreadsheet.GetCellValue(cellName);

            //checks type of cellValue, doubles are parsed to strings
            //All other types are guaranteed to be a FormulaError,
            //in which case "ERROR" will be returned. 
            if (cellValue is string s)
            {
                return s;
            }
            else if (cellValue is double d)
            {
                return d.ToString();
            }
            else
            {
                return "ERROR";
            }
        }

        /// <summary>
        /// Converts the contents of the specificed cell to
        /// a string so it can be placed in text boxes. 
        /// </summary>
        /// <param name="cellName">Name of specfied cell.</param>
        /// <returns>The cell value as a string.</returns>
        private string CellContentsToString(String cellName)
        {

            object cellContents = mySpreadsheet.GetCellContents(cellName);

            //checks type of cellContents, doubles are parsed to strings
            //All other types are guaranteed to be a FormulaError,
            //in which case "ERROR" will be returned. 
            if (cellContents is string s)
            {
                return s;
            }
            else if (cellContents is double d)
            {
                return d.ToString();
            }
            else if (cellContents is SpreadsheetUtilities.Formula f)
            {
                return "=" + f.ToString();
            }
            else
            {
                return "ERROR";
            }
        }

        /// <summary>
        /// Helper method used to warn the user they are closing the document with 
        /// unsaved work.This happens when...
        /// 1. The form is closed.
        /// 2. A new spreadsheet is made.
        /// 3. Another spreadsheet is about to be opened. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveWarning(object sender, EventArgs e)
        {
            //if spreadsheet has unsaved work
            if (mySpreadsheet.Changed)
            {

                if (MessageBox.Show("You have unsaved changes. Would you like to Save?", "Unsaved Changes",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //Fires saving event if user selects "YES" in the Messagebox
                    Save_Click(sender, e);
                }


            }
        }

        /// <summary>
        /// Event that fires when the save button in the file menu 
        /// is clicked. Uses a SaveFileDialog box. This method checks
        /// if the name of saved file is an empty string.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Choose where to save your spreadsheet";
            saveFileDialog1.Filter = "Spreadsheet (*.sprd)|*.sprd|All files (*.*)|*.*";
            saveFileDialog1.ShowDialog();

            //makes sure filename is not an empty string
            if (saveFileDialog1.FileName != "")
                mySpreadsheet.Save(saveFileDialog1.FileName);



        }

        /// <summary>
        /// Replaces the current backing spreadsheet with another from the 
        /// specified filename. Used when another spreadsheet is opened. 
        /// </summary>
        /// <param name="filename">Name of the spreadsheet file.</param>
        private void ReplaceSpreadsheetFromFile(string filename)
        {
            mySpreadsheet = new SS.Spreadsheet(filename, s => Regex.IsMatch(s, @"^[A-Z][1-9][0-9]*$"), s => s.ToUpper(), "ps6");
        }

        //EVENTS

        /// <summary>
        /// Event that fires the the open button is clicked
        /// under the file menu. A save warning will pop up if
        /// there is unsaved progress. The user will then be presented
        /// an OpenFileDialog box with the option of showing *.sprd files
        /// or all files. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Warns user if there is unsaved progress
            //SaveWarning(sender, e);

            //gives the option of all files or spreadsheet files in the open dialog box
            openFileDialog1.Filter = "Spreadsheet (*.sprd)|*.sprd|All files (*.*)|*.*";
            //Title of the open file dialog box
            openFileDialog1.Title = "Select a Spreadsheeet file to Open";

            //show open file dialog box
            openFileDialog1.ShowDialog();


            //assign name of file to string
            string filename = openFileDialog1.FileName;

            //verifies the name of the file is not an empty string. 
            //
            if (filename != "")
            {
                try
                {
                    //replaces the current backing spreadsheet with a new one
                    //the data of the new spreadsheet is derived from the file to be opened.
                    ReplaceSpreadsheetFromFile(filename);

                }
                //catch any exceptions thrown from reading in the file
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message + "\n\nReverting to previous file.", "File not read");
                }


                //Clear the current cells
                spreadsheetPanel1.Clear();

                //updates cells with new values 
                foreach (string name in mySpreadsheet.GetNamesOfAllNonemptyCells())
                {
                    spreadsheetPanel1.SetValue(getColumnFromName(name), getRowFromName(name), CellValueToString(name));
                }
            }
        }

        /// <summary>
        /// Event that fires when the new dropdown button is 
        /// clicked under the file menu. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            SpreadsheetApplicationContext.getAppContext().RunForm(new Form1(isDark));
        }

        /// <summary>
        /// Event that fires when the user clicks the "Close" button 
        /// under the file menu. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Event that fires when the user tries to close the form. Calls
        /// SaveWarning to see if there is any unsaved work. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SaveWarning(sender, e);
        }

        /// <summary>
        /// Event that fires when the "Dark Mode" dropdown button
        /// is clicked. Sets the fore and back colors of both the form and
        /// spreadsheetpanel. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void darkModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //used for opening new spreadsheets in the current theme
            isDark = true;

            //set colors for the form
            this.BackColor = Color.FromArgb(77, 78, 78);
            this.ForeColor = Color.FromArgb(172, 172, 172);

            //set colors for ValueBox
            ValueBox.BackColor = Color.FromArgb(99, 101, 101);
            ValueBox.ForeColor = Color.White;

            //set colors for the NameBox
            NameBox.BackColor = Color.FromArgb(99, 101, 101);
            NameBox.ForeColor = Color.White;

            //set colors for the ContentsBox
            ContentsBox.BackColor = Color.FromArgb(99, 101, 101);
            ContentsBox.ForeColor = Color.White;

            //set color for SetButton
            SetButton.BackColor = Color.FromArgb(23, 24, 24);

            //accesses the spreadsheetpanel to set the back and fore colors
            spreadsheetPanel1.MyDrawingPanel.CustomBackColor = Color.FromArgb(77, 78, 78);
            spreadsheetPanel1.MyDrawingPanel.CustomForeColor = Color.FromArgb(144, 178, 178);
        }

        /// <summary>
        /// Event that fires when the "Light Mode" dropdown button
        /// is clicked. Sets the fore and back colors of both the form and
        /// spreadsheetpanel. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lightModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //used for opening new spreadsheets in the current theme
            isDark = false;

            //set colors for the form
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;

            //set colors for ValueBox
            ValueBox.BackColor = Color.White;
            ValueBox.ForeColor = Color.Black;

            //set colors for the NameBox
            NameBox.BackColor = Color.White;
            NameBox.ForeColor = Color.Black;

            //set colors for the ContentsBox
            ContentsBox.BackColor = Color.White;
            ContentsBox.ForeColor = Color.Black;

            //set color for SetButton
            SetButton.BackColor = Color.White;

            //accesses the spreadsheetpanel to set the back and fore colors
            spreadsheetPanel1.MyDrawingPanel.CustomBackColor = Color.White;
            spreadsheetPanel1.MyDrawingPanel.CustomForeColor = Color.Black;

        }

        /// <summary>
        /// Creates a MessageBox, teaching the user how use the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Welcome! Select cells by left clicking on them. " +
                " The name of the selected cell will be dislpayed in the Name Box." +
                " Once a cell is selected, you can type" +
                " the contents of the cell into the Contents Box.\n\n" +
                "Pressing enter or clicking the Set button will save the contents into the cell and calculate its" +
                "value. This value will displayed in the cell and the Value Box. \n\n" +
                "You can create formulas by beginning the cell contents with \"=\". A formula consists of doubles, the names" +
                " of other cells, and operators \"+-*/()\".\n\n" +
                "Themes:\n\n" +
                "Use the Theme dropdown button to switch between dark and light modes.", "Welcome to PS6!");
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {


            clientcontroller.ConnectButtonPressed(UsernameBox.Text, PasswordBox.Text, ServerBox.Text, FilenameBox.Text);

        }

        private void OnSuccessfulConnection()
        {
            MethodInvoker change = new MethodInvoker(() =>
            {
                ServerBox.Visible = false;
                ConnectButton.Visible = false;
                label1.Visible = false;
                UsernameBox.Visible = true;
                userLabel.Visible = true;
                PasswordBox.Visible = true;
                passLabel.Visible = true;
                FilenameBox.Visible = true;
                fileLabel.Visible = true;
                openButton.Visible = true;
                ssListBox.Visible = true;
                existingLabel.Visible = true;
            });

            this.Invoke(change);
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            UsernameBox.Visible = false;
            userLabel.Visible = false;
            PasswordBox.Visible = false;
            passLabel.Visible = false;
            FilenameBox.Visible = false;
            fileLabel.Visible = false;
            openButton.Visible = false;
            ssListBox.Visible = false;
            existingLabel.Visible = false;
            ContentsBox.Visible = true;
            ContentsLabel.Visible = true;
            ValueBox.Visible = true;
            Value.Visible = true;
            NameBox.Visible = true;
            NameLabel.Visible = true;
            spreadsheetPanel1.Visible = true;
            SetButton.Visible = true;
            EditDropdown.Visible = true;

            clientcontroller.OpenButtonPressed(UsernameBox.Text, PasswordBox.Text, FilenameBox.Text);
        }

        private void PopulateList()
        {

            MethodInvoker populate = new MethodInvoker(() =>
            {
                foreach (string item in clientcontroller.ssList)
                {
                    ssListBox.Items.Add(item);
                }
            });

            this.Invoke(populate);
        }


        private void DisplayPasswordError()
        {
            MethodInvoker get_new_password = new MethodInvoker(() =>
            {

                spreadsheetPanel1.Visible = false;
                NameBox.Visible = false;
                NameLabel.Visible = false;
                Value.Visible = false;
                ValueBox.Visible = false;
                ContentsBox.Visible = false;
                ContentsLabel.Visible = false;
                EditDropdown.Visible = false;
                UsernameBox.Visible = true;
                userLabel.Visible = true;
                PasswordBox.Visible = true;
                passLabel.Visible = true;
                ssListBox.Visible = true;
                existingLabel.Visible = true;
                SetButton.Visible = false;
                openButton.Visible = true;
                fileLabel.Visible = true;
                FilenameBox.Visible = true;
            });

            this.Invoke(get_new_password);

            MessageBox.Show("Your password is incorrect. Please try again.");
            PasswordBox.ReadOnly = false;
        }

        private void DisplayCircularError()
        {
            MessageBox.Show("Circular dependecy found. Cell will not be changed.");
        }

        private void PopulateCells()
        {

            foreach (KeyValuePair<string, string> entry in clientcontroller.cellValues)
            {
                MethodInvoker update = new MethodInvoker(() => { this.SetCell(entry.Key, entry.Value); });
                this.Invoke(update);
            }

        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            clientcontroller.UndoMade();
        }

        private void RevertButton_Click(object sender, EventArgs e)
        {
            //get name of the selected cell
            String cellName = GetSelectionName();
            clientcontroller.RevertMade(cellName);
        }

        private void ContentsBox_TextChanged(object sender, EventArgs e)
        {
            //change cell contents to match the contents box
            string name = GetSelectionName();
            spreadsheetPanel1.SetValue(getColumnFromName(name), getRowFromName(name), ContentsBox.Text);
        }

        private void LostConnection()
        {
            MethodInvoker reset = new MethodInvoker(() => {
                mySpreadsheet = new SS.Spreadsheet(s => Regex.IsMatch(s, @"^[A-Z][1-9][0-9]*$"), s => s.ToUpper(), "ps6");
                MessageBox.Show("Lost connection to server. Please Try again.", "Server Disconnect");
                spreadsheetPanel1.Clear();
                spreadsheetPanel1.Visible = false;
                NameBox.Visible = false;
                NameLabel.Visible = false;
                Value.Visible = false;
                ValueBox.Visible = false;
                ContentsBox.Visible = false;
                ContentsLabel.Visible = false;
                UsernameBox.Visible = false;
                userLabel.Visible = false;
                PasswordBox.Visible = false;
                passLabel.Visible = false;
                FilenameBox.Visible = false;
                fileLabel.Visible = false;
                openButton.Visible = false;
                ssListBox.Visible = false;
                existingLabel.Visible = false;
                SetButton.Visible = false;
                EditDropdown.Visible = false;
                ServerBox.Visible = true;
                ConnectButton.Visible = true;
                label1.Visible = true;
            });

            this.Invoke(reset);
        }
    }
}
