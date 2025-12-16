namespace LeanMe;

public static class Utility
{
    public static int ReadInt()
    {
        var text = Console.ReadLine()?.Trim();
        if (!int.TryParse(text, out var val))
            throw new ArgumentException("Bitte eine Zahl eingeben!");

        return val;
    }
}