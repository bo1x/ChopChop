using UnityEngine;

public class ObjectKillzone : MonoBehaviour
{
    private int layerInt;
    [SerializeField] private int partsDestroyed;

    //gizmos
    [SerializeField] private BoxCollider BC;

    private void Awake()
    {
        BC = GetComponent<BoxCollider>();
        partsDestroyed = 0;
        layerInt = LayerMask.NameToLayer("Slice");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layerInt)
        {
            ObjectTouchedDeathZone(other.gameObject);
        }
    }
    private void ObjectTouchedDeathZone(GameObject go)
    {
        ObjectSpawner.instance.RemoveObjectFromList(go.GetComponent<Rigidbody>());
        Destroy(go);
        partsDestroyed += 1;
        UImanager.instance.UpdateText(partsDestroyed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(BC.center, BC.size);
    }

    
}
