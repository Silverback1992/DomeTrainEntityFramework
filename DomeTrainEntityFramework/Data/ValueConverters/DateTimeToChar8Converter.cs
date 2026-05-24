using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;

namespace DomeTrainEntityFramework.Data.ValueConverters;

public class DateTimeToChar8Converter : ValueConverter<DateTime, string>
{
    public DateTimeToChar8Converter() : base(
        v => v.ToString("yyyyMMdd", CultureInfo.InvariantCulture),
        v => DateTime.ParseExact(v, "yyyyMMdd", CultureInfo.InvariantCulture))
    {


    }
}
