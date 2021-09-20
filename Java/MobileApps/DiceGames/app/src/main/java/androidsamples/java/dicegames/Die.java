package androidsamples.java.dicegames;

public interface Die {
    /**
     * Rolls the die.
     */
    void roll();

    /**
     * Reports the value of the top face of the die.
     *
     * @return number of dots on the top face of the die
     */
    int value();
}
