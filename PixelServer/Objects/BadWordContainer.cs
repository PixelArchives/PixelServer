namespace PixelServer.Objects;

public class BadWordContainer
{

    public List<string> Words { get; set; } = new();
    public List<char> Symbols { get; set; } = new();

    /*public BadWordContainer()
    {
        Words.Add("dashcat");
        Symbols.Add('卐');
        Symbols.Add('卍');
    }*/
}
