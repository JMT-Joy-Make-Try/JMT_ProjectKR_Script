namespace JMT.System.StatSystem
{
    public enum StatModifierType
    {
        Addition, // a + b
        Subtraction, // a - b
        Multiplicative, // a * b
        Division, // a / b (b != 0)
        Percent, // a * (1 + b / 100)
        Percent_Minus, // a * (1 - b / 100)
        Percent_Value // a * b / 100
    }
}