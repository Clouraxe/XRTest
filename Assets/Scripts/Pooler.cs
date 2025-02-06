using UnityEngine;
using UnityEngine.Pool;
using System;
using UnityEditor.PackageManager;

public class Pooler<T> where T : MonoBehaviour
{

    private static Pooler<T> instance;
    private Pooler() { }
    public static Pooler<T> Instance {
        get {
            if (instance == null)
            {
                instance = new Pooler<T>();
            }

            return instance;
        }
    }
    public GameObject objectPrefab;

    private ObjectPool<T> _objPool;



    public void Initiate(GameObject objPrefab, int capacity, int maxCapacity)
    {
        if (objectPrefab != null) return;
        objectPrefab = objPrefab;
        _objPool = new ObjectPool<T>(CreateObject, GetPoolObj, ReleaseToPool, DestroyPoolObject, true, capacity, maxCapacity);
    }


    private T CreateObject()
    {
        if (objectPrefab == null) Debug.LogError("Please set the object prefab before initializing!!");
        T obj = UnityEngine.Object.Instantiate(objectPrefab).GetComponent<T>();
        return obj;
    }

    private void GetPoolObj(T obj)
    {
        obj.gameObject.SetActive(true);
        obj.transform.SetLocalPositionAndRotation(objectPrefab.transform.localPosition, objectPrefab.transform.localRotation);
    }

    private void ReleaseToPool(T obj)
    {
        obj.gameObject.SetActive(false);
    }


    private void DestroyPoolObject(T obj)
    {
        UnityEngine.Object.Destroy(obj.gameObject);
    }


    public T Get() => _objPool.Get();

    public void Release(T obj) => _objPool.Release(obj);

    public void Clear() => _objPool.Clear();


}