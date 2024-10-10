using System;
using System.Collections.Generic;
using UnityEngine;
public class PoolSystem : Singleton<PoolSystem> {
    [SerializeField] List<PoolItem> ListPrefab;

    static Dictionary<Type, GameObject> DicPrefab;

    static Dictionary<Type, List<GameObject>> ListPool = new Dictionary<Type, List<GameObject>>();

    protected override void Awake()
    {
        base.Awake();

        InitPrefab();
    }

    private void InitPrefab()
    {

        DicPrefab = new Dictionary<Type, GameObject>();

        foreach (PoolItem item in ListPrefab)
        {
            DicPrefab.Add(item.GetItemType(), item.gameObject);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        ListPool.Clear();
    }

    public static GameObject GetItem<T>()
    {
        Type type = typeof(T);
        if (!ListPool.ContainsKey(type))
            ListPool.Add(type, new List<GameObject>());

        List<GameObject> list = ListPool[type];
        GameObject go = null;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] == null)
            {
                list.RemoveAt(i);
                continue;
            }
            if (list[i].activeInHierarchy) continue;

            go = list[i];

        }

        if (!go)
            go = CreateItem(type);

        return go;
    }

    public static T GetItem<T>(Transform transParent)
    {
        GameObject go = GetItem<T>();
        go.transform.SetParent(transParent);
        resetZ(go);
        go.transform.localScale = Vector3.one;
        go.SetActive(true);
        return go.GetComponent<T>();
    }

    public static T GetItem<T>(Transform transParent, Vector3 position)
    {
        GameObject go = GetItem<T>();
        go.transform.SetParent(transParent);
        go.transform.localScale = Vector3.one;
        go.SetActive(true);
        go.transform.position = position;

        return go.GetComponent<T>();
    }

    static void resetZ(GameObject go)
    {
        Vector3 pos = go.transform.localPosition;
        pos.z = 0;
        go.transform.localPosition = pos;
    }

    private static GameObject CreateItem(Type type)
    {
        if (!DicPrefab.ContainsKey(type))
        {
            Debug.Log("Dont have pool item type " + type);
            return null;
        }
        GameObject prefab = DicPrefab[type];
        GameObject item = Instantiate(prefab);
        ListPool[type].Add(item);
        return item;
    }

    public static void ReturnItem<T>(T item) where T : Component
    {
        if (!Instance) return;
        //DOTween.Kill(item);
        item.gameObject.SetActive(false);
        item.transform.SetParent(Instance.transform);
    }

    public static void RemoveItem<T>(T item) where T : Component
    {
        if (!Instance) return;

    }

}


