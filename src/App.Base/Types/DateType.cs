using NepDate;

namespace App.Base.Types;

public class DateType
{
    public DateTime EnglishDate { get; set; }

    public string NepaliDate { get; set; }

    public DateType(string nepaliDate)
    {
        var nepDate = NepDate.NepaliDate.Parse(nepaliDate);
        NepaliDate = nepDate.ToString();
        EnglishDate = nepDate.EnglishDate;
    }

    public DateType(DateTime englishDate)
    {
        NepaliDate = englishDate.ToNepaliDate().ToString();
        EnglishDate = englishDate;
    }
}
