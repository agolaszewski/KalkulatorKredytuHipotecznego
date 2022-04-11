namespace Provider.Indexes;

public interface IIndexProvider
{
    decimal GetValue(DateTime date);
}