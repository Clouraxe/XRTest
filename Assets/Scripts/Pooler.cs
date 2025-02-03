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

    public ObjectPool<T> _objPool;



    public ObjectPool<T> Initiate(GameObject objPrefab, int capacity, int maxCapacity)
    {
        if (objectPrefab != null) return _objPool;
        objectPrefab = objPrefab;
        _objPool = new ObjectPool<T>(CreateObject, GetPoolObj, ReleaseToPool, DestroyPoolObject, true, capacity, maxCapacity);

        return _objPool;
    }


    public T CreateObject()
    {
        if (objectPrefab == null) Debug.LogError("Please set the object prefab before initializing!!");
        T obj = UnityEngine.Object.Instantiate(objectPrefab).GetComponent<T>();
        return obj;
    }

    public void GetPoolObj(T obj)
    {
        obj.gameObject.SetActive(true);
        obj.transform.SetLocalPositionAndRotation(objectPrefab.transform.localPosition, objectPrefab.transform.localRotation);
    }

    public void ReleaseToPool(T obj)
    {
        obj.gameObject.SetActive(false);
    }


    public void DestroyPoolObject(T obj)
    {
        UnityEngine.Object.Destroy(obj.gameObject);
    }


}