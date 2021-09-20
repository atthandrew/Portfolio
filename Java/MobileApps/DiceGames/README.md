a. DiceGames, Andrew Thompson, u0879848, a.t.thandrew@gmail.com
b. This application simulates the roll of a 6-faced die. To roll the die, the user must click the
    numbered square, which updates its number to be the result of the roll. If the user rolls a 6,
    5 coins are added to their wallet, which is displayed above the button. If a 6 is rolled
    immediately after another 6, 10 coins are added instead. If any other number matches the
    previous roll, 5 coins are taken away. The user's wallet can go into the negatives (debt).
    Statistics on the previous roll: the total times rolled, the number of sixes, the number of
    repeated (double) sixes, and the number of other repeated numbers are logged below the button.
    No bugs are known at this time, but I would like to note that tasks 3 and 4 are slightly
    contradictory. Task 3 asks you to show how often you roll a six vs total dice rolls. Task 4,
    however, makes you split the sixes rolled into singular sixes and double sixes. This split can't
    be avoided if you wish to calculate your coin count using the computation technique demonstrated
    in that question. So it is assumed that the format and splitting of win types in Figure 3 is
    what is desired.
c.  1.  Task 1 was completed with no major changes to the steps in the videos.
    2.  For Task 2, I made sure there were two getter methods in the ViewModel: getDieValue and
        getWinValue. In OnClickListener in WalletActivity, if these two values match, then I create 
        a Toast according to the instructions located in the section labeled "The Basics" in the 
        link provided. I did not mess with the positioning or customize the view since the default 
        settings seemed to be serve the purposes of this assignment.
    3.  For Task 3, I started by creating another LinearLayout for both versions of the XML. The
        layout contains two text labels and two texts I would later access in code, txt_num_wins and
        txt_total_rolls. Once these were set up and parented properly, I mimicked the code we used
        to set up obtain mBalance from both the ViewModel and the XML for these two TextViews inside
        the activity. Finally, within ViewModel, I made a couple variables to keep track of the
        number of wins and the number of rolls, incrementing them appropriately within the roll
        function. After creating getters for both of these, the activity was able to track them.
    4.  For Task 4, I followed mostly the same process as I did for Task 3. I began in the XML,
        making the needed LinearLayouts and TextViews, and then set them up inside of the activity
        by following the process we had for mBalance. Moving on to the ViewModel, I created 4
        variables, one to keep track of the current roll, another for the previous, another for the
        double sixes, and another for other doubles. To keep track of the previous roll, I set it to
        the last roll's current roll before it updates the new one. In the if statement checking if
        we won, I check if we one last round too, doubling the coins if so and incrementing the
        double sixes count. I do likewise for double others. These results are then passed to the
        activity appropriately.
d.  To further enhance the app, I feel that payouts shouldn't be capped at ten, each consecutive win
    should double the coins earned, rather than a flat increase. That way triples or quads would
    feel more satisfying. Also, it would be interesting if doubles of certain numbers had special
    effects, for example, double 2's could double your current coins, or double 1's would make you
    divide your coins by the next roll. An auto-roll feature might be nice too, that is, the app
    would automatically roll very quickly, eliminating the need to click over and over in order to
    collect coins.
e. Estimated 7 hours for this assignment.
f. Difficulty rating: 6.
        