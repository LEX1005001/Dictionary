namespace DictionaryWebApp
{
    public class Word_Tr
    {
        public int Id { get; set; }

        public string Word { get; set; } = string.Empty;

        public string Translation { get; set; } = string.Empty;

        public int ThemeId { get; set; }
    }
}
