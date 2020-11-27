using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool instance = null;
    public static ObjectPool Instance
    {
        get {return instance;}
    }
    [Header("Seeting")]
    public ObjPoolSetting[] objPool;
    
    //local use
    private static Dictionary<GameObject, string> poolObjList = new Dictionary<GameObject, string>();
    private static Dictionary<string, ObjPoolInfo> poolInfo = new Dictionary<string, ObjPoolInfo>();
    void Start()
    {
        if(instance ==null)
            instance = this;

        poolObjList.Clear();
        poolInfo.Clear();
        CreatPoolObject();
    }

    private void CreatPoolObject()
    {
        if(objPool.Length == 0) return;

        foreach(ObjPoolSetting ops in objPool)
        {
            GameObject pool = new GameObject(ops.name);
            pool.transform.SetParent(transform);
            pool.transform.position = transform.position;
            poolInfo.Add(ops.name,new ObjPoolInfo(pool.transform, ops.prefab, ops.enableInPool));

            for(int i = 1; i<=ops.Quantity; i++)
            {
                GameObject newObj = poolInfo[ops.name].AddNewObj();
                poolObjList.Add(newObj,ops.name);
            }
        }
    }

    public static Transform TakeFromPool(string pool)
    {
        Transform t = poolInfo[pool].Take();
        if(poolInfo[pool].inObj < 10)
            instance.AddMore(pool);

        return t;

    }
    public static void ReturnToPool(GameObject obj)
    {
        poolInfo[poolObjList[obj]].Return(obj);
    }
    public static IEnumerator ReturnToPool(GameObject obj,float t)
    {
        yield return new WaitForSeconds(t);
        poolInfo[poolObjList[obj]].Return(obj);
        yield return null;
    }
    private void AddMore(string pool)
    {
        if(poolInfo[pool].corou == null)
            poolInfo[pool].corou = StartCoroutine(AddMoreProcess(pool));
    }
    private IEnumerator AddMoreProcess(string pool)
    {
        poolInfo[pool].addMoreCounter++;
        int addAmt = (int)(poolInfo[pool].totalObj * 0.2f);
        if(addAmt < 10) addAmt =10;
        
        for(int i = 0; i < addAmt;i++)
        {
            GameObject newObj = poolInfo[pool].AddNewObj();
            poolObjList.Add(newObj,pool);
            yield return null;
        }

        poolInfo[pool].corou = null;
    }
    private void OnApplicationQuit()
    {
        foreach(ObjPoolSetting ops in objPool)
        {
            string pool = ops.name;
            int maxUse = poolInfo[pool].maxOut;
            string recAmt = 
                poolInfo[pool].addMoreCounter > 0 || ops.Quantity - maxUse > 15 ? 
                (maxUse + 15).ToString() : "-";
                Debug.Log(string.Concat(
                "Pool [ ", pool, " ] max out value: ", maxUse, " (", recAmt, ")\n")
            );
        }
    }
}
