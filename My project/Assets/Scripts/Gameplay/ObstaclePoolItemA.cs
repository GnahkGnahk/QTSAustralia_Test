using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ObstaclePoolItemA : PoolItem, IObsticelFeature {
    [SerializeField] NavMeshAgent agent;
    [SerializeField] int damage;

    bool isChasingPlayer = true, firstSetup = true;
    MeshRenderer meshRenderer;

    public static event Action<int> OnLostHP;

    internal override Type GetItemType()
    {
        return typeof(ObstaclePoolItemA);
    }

    public void Setup()
    {
        if (firstSetup)
        {
            meshRenderer = gameObject.GetComponent<MeshRenderer>();

            OnLostHP += GameManager.Instance.LostHP;

            firstSetup = false;
        }
        isChasingPlayer = true;
        Utility.ChangeColorMateMesh(Color.black, meshRenderer);
    }

    public void UpdateDestination(Vector3 position)
    {
        if (gameObject.activeInHierarchy && isChasingPlayer && agent.isOnNavMesh)
        {
            agent.destination = position;
            float dist = Vector3.Distance(position, transform.position);
            if (dist < 1.1f)
            {
                //  Turn red
                Utility.ChangeColorMateMesh(Color.red, meshRenderer);
            }
        }
    }

    public void OnTouchPlayer(Collision collision)
    {
        isChasingPlayer = false;
        OnLostHP?.Invoke(damage);

        PoolSystem.ReturnItem(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == Helper.TAG_PLAYER)
        {
            Debug.Log("OnCollisionEnter A!");
            OnTouchPlayer(collision);
        }
    }


}
