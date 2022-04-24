using Provider.Indexes;

namespace Core
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Today => DateTime.Today;
    }
}