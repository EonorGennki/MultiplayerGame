using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly ConcurrentStack<T> pool = new ConcurrentStack<T>();
    private readonly T prefab;
    private readonly Transform parent;
    private readonly int maxSize;

    public int Count => pool.Count;

    public ObjectPool(T prefab, Transform parent, int initialSize, int maxSize = 100)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.maxSize = maxSize;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = CreateNew();
            obj.gameObject.SetActive(false);
            pool.Push(obj.GetComponent<T>());
        }
    }

    /// <summary>
    /// 创建新对象
    /// </summary>
    /// <returns></returns>
    private T CreateNew() => Object.Instantiate(prefab, parent.position, Quaternion.identity);

    /// <summary>
    /// 从池中获取对象
    /// </summary>
    /// <returns></returns>
    public T Get()
    {
        if (pool.Count > 0)
        {
            pool.TryPop(out T obj);
            obj.gameObject.SetActive(true);
            return obj;
        }

        //池空创建新对象
        T newObj = CreateNew();
        newObj.gameObject.SetActive(true);
        return newObj;
    }

    /// <summary>
    /// 回收对象
    /// </summary>
    /// <param name="obj"></param>
    public void Return(T obj)
    {
        if (obj is null)
        {
            return;
        }

        if (pool.Count >= maxSize)
        {
            Object.Destroy(obj.gameObject);
        }

        obj.gameObject.SetActive(false);
        obj.transform.SetParent(parent);
        pool.Push(obj);
    }

    public void ReturnAll()
    {

    }
}
