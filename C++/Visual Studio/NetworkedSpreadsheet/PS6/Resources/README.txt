Authors: Andrew Thompson u0879848 and William Meldrum u0934535
Date: 10/22/18
DependencyGraph library version: Commit 3ac4c3c, 9/27/18
Formula library version: Commit 25e6660, 10/3/18
Spreadsheet library version: Commit 7f52ff7, 10/13/18

10/15/18 -To set up the basic functionality between the spreadsheet and its GUI, we followed a lot of the suggestions given in the demo from class, such as: using the enter button as
		 an alternate set button, the handling of the SelectionChanged event, and the basic structure of the GUI and set button.
10/19/18 -For the save-related methods, we relied upon the Changed property of the Spreadsheet to warn the user of losing work before opening/closing a file. We are relying on the default
		 behavior of the OpenFileDialog/SaveFileDialog tools for opening files of the wrong type (it does nothing), and we don't allow file names that are empty strings (pressing enter
		 does nothing.) We used the SpreadsheetApplicationContext given in the demo for PS6Skeleton to handle multiple windows.
10/21/18 -We created our extra feature, it took about 2-3 hours total. We made a dark theme for the spreadsheet, which required us to alter some of the code for the SpreadsheetPanel, namely,
		 the brushes and colors used in the DrawingPanel. Thus, instead of a dll, we copied the code directly into our SpreadsheetGUI project as suggested on Piazza. We had to make the
		 DrawingPanel class public so that we could alter it using methods that affect the View. The private member of the SperadsheetPanel class, however, remains private, and is accessed
		 with a property.
10/22/18 -Added comments to both Form1.cs and CustomCustomSpreadsheetPanel.cs. Found a way to handle SpreadsheetReadWriteExceptions when opening a new file.
		 

Extra Feature -The dark theme can be accessed from the Themes tab on the toolbar. After selecting dark mode, the current window and all windows opened thereafter will have a dark theme.
			  After selecting light mode, the current window and all windows opened thereafter will have the default colors. We have not implemented it so that windows opened before the
			  current window will also update to have a dark theme, as that seemed beyond the scope of what is required of an extra feature (it seemed it would take extra time to make the
			  entire ApplicationContext to update to the current theme).

General Use -To change the contents of a cell, simply click on the cell on the spreadsheet and type the contents into the contents box. To enter the contents, press enter or click the set
			 button. Formulas must be prefaced with the "=" character. The values box will automatically update based on what contents are entered, and the value will also be displayed in 
			 the cell itself. The name box will update based on the selection. 
		 