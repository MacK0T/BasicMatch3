using UnityEngine;
using System.Collections;

public class UnityPoolObject : MonoBehaviour, IPoolObject<string>
{
    public virtual string Group { get { return name; } } // та самая группа
    public Transform MyTransform { get { return myTransform; } }

    protected Transform myTransform;

    protected virtual void Awake()
    {
        myTransform = transform;
    }

    public virtual void SetTransform(Vector3 position, Quaternion rotation)
    {
        myTransform.position = position;
        myTransform.rotation = rotation;
    }

    public virtual void Create() // конструктор для пула
    {
        gameObject.SetActive(true);
    }

    public virtual void OnPush() // деструктор для пула
    {
        gameObject.SetActive(false);
    }

    public virtual void Push() // вызов деструктора
    {
        UnityPoolManager.Instance.Push(Group, this);
    }

    public void FailedPush() // не возможно попасть в пул
    {
        Debug.Log("FailedPush"); // !!!
        Destroy(gameObject);
    }
}
