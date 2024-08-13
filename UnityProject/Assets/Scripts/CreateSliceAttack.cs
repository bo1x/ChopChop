using UnityEngine;
using EzySlice;
using System.Collections.Generic;
using System.Collections;

public class CreateSliceAttack : MonoBehaviour
{
    bool preparingAttack = false;
    [SerializeField] private Vector3 InitialPoint = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 FinalPoint = new Vector3(5, 5, 0);

    public Transform planeDebug;
    public List<GameObject> Target = new List<GameObject>();
    public Material crossSectionMaterial;
    public List<GameObject> Parts = new List<GameObject>();

    public LayerMask layerMask;

    private Rigidbody rbParentHull;
    private void Awake()
    {

    }
    private void Update()
    {
        if (ClickInput.Instance.GetAttack() && preparingAttack == false)
        {
            preparingAttack = true;
            InitialPoint = ClickInput.Instance.GetMousePos();
            InitialPoint.z = 0;
        }
        else if (preparingAttack && !ClickInput.Instance.GetAttack())
        {
            FinalPoint = ClickInput.Instance.GetMousePos();
            FinalPoint.z = 0;

            RaycastHit[] hits;
            Vector3 dir = FinalPoint - InitialPoint;
            dir.Normalize();
            float dist = Vector3.Distance(InitialPoint, FinalPoint);
            hits = Physics.SphereCastAll(InitialPoint, 1f, dir, dist, layerMask);
            //hits = Physics.RaycastAll(InitialPoint, dir, dist,layerMask);
            if(hits.Length>0)
            {
                foreach (var hit in hits)
                {
                    Slice(hit.collider.gameObject);
                }
            }

            preparingAttack = false;

        }


    }

    private void FixedUpdate()
    {

    }



    public void Slice(GameObject _target)
    {
        rbParentHull = _target.GetComponent<Rigidbody>();

        Vector3 direction = FinalPoint - InitialPoint;
        
        //Vector3 localDirection = _target.transform.InverseTransformDirection(direction);
        Vector3 localPoint1 = _target.transform.InverseTransformPoint(InitialPoint);

        Vector3 right = Vector3.Cross(direction, _target.transform.up);
        Vector3 localUp = Vector3.Cross(right, direction).normalized;
        EzySlice.Plane slicingPlane = new EzySlice.Plane(localPoint1, localUp);
        SlicedHull slicedObject = _target.Slice(slicingPlane);

        if (slicedObject != null)
        {
            // Crear los dos nuevos objetos a partir del corte
            GameObject upperHull = slicedObject.CreateUpperHull(_target, crossSectionMaterial);
            GameObject lowerHull = slicedObject.CreateLowerHull(_target, crossSectionMaterial);

            
            Parts.Add(upperHull);
            Parts.Add(lowerHull);

           // upperHull.transform.position += localUp * 0.5f;
           // lowerHull.transform.position += -localUp * 0.5f;

            // Agregar colliders y rigidbodies si es necesario
            AddComponents(upperHull);
            AddComponents(lowerHull);

            // Desactivar el objeto original
            _target.SetActive(false);
        }
        else
        {
            Debug.Log("NO DETECTO OBJETO PARA CORTAR");
        }

    }

    public void Slice2(GameObject _target)
    {

    }

    IEnumerator AddComponentsAll()
    {
        yield return new WaitForSeconds(10f);
        foreach (GameObject item in Parts)
        {
            item.AddComponent<Rigidbody>();
            item.GetComponent<Rigidbody>().AddRelativeForce(Random.onUnitSphere * 10f);
        }
    }


    void AddComponents(GameObject obj)
    {
        obj.AddComponent<MeshCollider>().convex = true;
       // obj.AddComponent<Rigidbody>();
        obj.layer = LayerMask.NameToLayer("Slice");
    }


    
    public void OnDrawGizmos()
    {
        Vector3 direction = FinalPoint - InitialPoint;
        Vector3 right = Vector3.Cross(direction, Vector3.up); 
        Vector3 localUp = Vector3.Cross(right, direction).normalized;
        

        Gizmos.color = Color.red;
        Gizmos.DrawLine(InitialPoint, FinalPoint);
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(InitialPoint, 1f);
        Gizmos.DrawSphere(FinalPoint, 1f);
        Gizmos.color = Color.green;
        Gizmos.DrawRay((InitialPoint+FinalPoint)/2,localUp);


    }

}
