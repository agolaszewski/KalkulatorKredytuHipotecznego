namespace Provider.Indexes;

public class IndexProviderFactory
{
    private readonly Dictionary<string, IIndexProvider> _map;

    public IndexProviderFactory(IDictionary<Index, IIndexProvider> map)
    {
        _map = map.ToDictionary(x => x.Key.Name, x => x.Value);
    }

    public IIndexProvider IndexProvider(Index index)
    {
        if (!_map.ContainsKey(index.Name))
        {
            throw new ArgumentOutOfRangeException(index.Name);
        }

        return _map[index.Name];
    }
}