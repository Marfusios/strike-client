using System.Collections;

namespace Strike.Client;

public record ResponseCollection<TModel> : ResponseBase, ICollection<TModel>
{
	private readonly ICollection<TModel> _collection;

	public ResponseCollection()
	{
		_collection = [];
	}

	public ResponseCollection(ICollection<TModel> collection)
	{
		_collection = collection;
	}

	public IEnumerator<TModel> GetEnumerator()
	{
		// ReSharper disable once NotDisposedResourceIsReturned
		return _collection.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		// ReSharper disable once NotDisposedResourceIsReturned
		return ((IEnumerable)_collection).GetEnumerator();
	}

	public void Add(TModel item)
	{
		_collection.Add(item);
	}

	public void Clear()
	{
		_collection.Clear();
	}

	public bool Contains(TModel item)
	{
		return _collection.Contains(item);
	}

	public void CopyTo(TModel[] array, int arrayIndex)
	{
		_collection.CopyTo(array, arrayIndex);
	}

	public bool Remove(TModel item)
	{
		return _collection.Remove(item);
	}

	public int Count => _collection.Count;

	public bool IsReadOnly => _collection.IsReadOnly;
}
