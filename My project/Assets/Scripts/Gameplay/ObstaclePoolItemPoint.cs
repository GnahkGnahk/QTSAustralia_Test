using System;
using UnityEngine;

public class ObstaclePoolItemPoint : PoolItem, IObsticelFeature 
{
    MeshRenderer meshRenderer = null;
    int point = 0;

    bool firstSetup = true;

    public static event Action<int> OnAddpoint;

    internal override Type GetItemType()
    {
        return typeof(ObstaclePoolItemPoint);
    }

    public void Setup()
    {
        this.point = UnityEngine.Random.Range(5, 10);
        if (firstSetup)
        {
            meshRenderer = gameObject.GetComponent<MeshRenderer>();

            OnAddpoint += GameManager.Instance.AddPoints;

            firstSetup = false;
        }
        Utility.ChangeColorMateMesh(Color.yellow, meshRenderer);
    }

    public void UpdateDestination(Vector3 position)
    {
        throw new NotImplementedException();
    }

    public void OnTouchPlayer(Collision collision = default)
    {
        GameManager.Instance.AddPoints(point);


        PoolSystem.ReturnItem(this);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Helper.TAG_PLAYER)
        {
            Debug.Log("Eat point !");
            OnTouchPlayer();
        }
    }
}
