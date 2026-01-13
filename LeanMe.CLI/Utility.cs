namespace LeanMe;

public static class Utility
{
    // Wir lesen hier den Int bzw. Parsen ihn und geben bei Bedar wenn es keine Zahl ist eine ArgumentException aus.
    public static int ReadInt()
    {
        var text = Console.ReadLine()?.Trim();
        if (!int.TryParse(text, out var val))
            throw new ArgumentException("Bitte eine Zahl eingeben!");

        return val;
    }
}