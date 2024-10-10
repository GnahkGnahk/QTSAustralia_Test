using UnityEngine;

public interface IObsticelFeature{
    public void Setup();
    public void UpdateDestination(Vector3 position);
    public void OnTouchPlayer(Collision collision = default);
}