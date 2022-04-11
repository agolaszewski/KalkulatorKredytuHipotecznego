namespace Provider.Indexes;

public class IndexProviderFactory
{
    private readonly Dictionary<string, IIndexProvider> _map;

    public IndexProviderFactory(IDictionary<Index, IIndexProvider> map)
    {
        _map = map.ToDictionary(x => (string)x.Key, x => x.Value);
    }

    public IIndexProvider IndexProvider(Index index)
    {
        if (!_map.ContainsKey((string)index))
        {
            throw new ArgumentOutOfRangeException((string)index);
        }

        return _map[(string)index];
    }
}