using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ObstaclePoolItemA : PoolItem, IObsticelFeature {
    [SerializeField] NavMeshAgent agent;

    bool isChasingPlayer = true;
    MeshRenderer meshRenderer;

    internal override Type GetItemType()
    {
        return typeof(ObstaclePoolItemA);
    }

    public void Setup()
    {
        if (!meshRenderer)
        {
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }
        isChasingPlayer = true;
        Utility.ChangeColorMateMesh(Color.black, meshRenderer);
    }

    public void UpdateDestination(Vector3 position)
    {
        if (gameObject.activeInHierarchy && isChasingPlayer)
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
