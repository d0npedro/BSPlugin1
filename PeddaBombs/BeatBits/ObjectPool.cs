namespace BeatBits
{
    public class ObjectPool<T>
    {
    	private T[] _pool;

    	private int _index;

    	public T[] pool => _pool;

    	public ObjectPool(T[] array)
    	{
    		_pool = array;
    	}

    	public T Next()
    	{
    		T result = _pool[_index];
    		_index = (_index + 1) % _pool.Length;
    		return result;
    	}

    	public void Reset()
    	{
    		_index = 0;
    	}
    }
}
