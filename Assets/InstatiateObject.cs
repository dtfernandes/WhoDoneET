using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstatiateObject : MonoBehaviour
{

    [System.Serializable]
    struct InstatiateObj
    {
        public GameObject Prefab;
        public Vector3 Position;
        public int Amount;
        public int MaxAmount;
    }

    [SerializeField]
    private List<InstatiateObj> _objectsToInstatiate;

    public void InstantiateObject(int index)
    {
        InstatiateObj obj = _objectsToInstatiate[index];
        
        if(obj.Amount < obj.MaxAmount )
            Instantiate(obj.Prefab, obj.Position, Quaternion.identity);
    }

    public void IterateAmount(int index)
    {
        InstatiateObj obj = _objectsToInstatiate[index];
        obj.Amount++;
    }

}
