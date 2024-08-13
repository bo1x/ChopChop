using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;

public class SliceObject : MonoBehaviour
{
    public Transform planeDebug;
    public GameObject Target;
    public Material crossSectionMaterial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Slice(Target);
        }
    }

    public void Slice(GameObject _target)
    {
        SlicedHull hull = _target.Slice(planeDebug.position, planeDebug.up);

        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(Target, crossSectionMaterial);
            GameObject lowerHull = hull.CreateLowerHull(Target, crossSectionMaterial);

            Destroy(Target);
        }
    }
}
