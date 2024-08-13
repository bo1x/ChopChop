using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;

/**
 * This class is an example of how to setup a cutting Plane from a GameObject
 * and how to work with coordinate systems.
 * 
 * When a Place slices a Mesh, the Mesh is in local coordinates whilst the Plane
 * is in world coordinates. The first step is to bring the Plane into the coordinate system
 * of the mesh we want to slice. This script shows how to do that.
 */
public class SliceObjectClickTest : MonoBehaviour
{
	[SerializeField] private Transform PlaneObj;
	[SerializeField] private Vector3 InitialPoint = new Vector3(0, 0, 0);
	[SerializeField] private Vector3 FinalPoint = new Vector3(0, 5, 0);

	[SerializeField] private Material crossMat;

	private bool preparingAttack = false;
	public LayerMask layerMask;
	private int LayerMaskint;

	private Rigidbody rbParentHull;
	private Rigidbody childRB;


	[Header("Fragments Settings")]
	[SerializeField] private float fragmentSpawnForce;
	private void Awake()
    {
		LayerMaskint = LayerMask.NameToLayer("Slice");

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
			preparingAttack = false;
			RotatePlane(InitialPoint,FinalPoint);
			Slice(crossMat);




		}

		if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
			//Slice(target, crossMat);
		}

		if (Keyboard.current.rKey.wasPressedThisFrame)
		{
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		}
	}


	public void RotatePlane(Vector3 p1, Vector3 p2)
    {


		PlaneObj.position = (p1 + p2) / 2;
		Vector3 direction = p2 - p1;
		PlaneObj.transform.right = direction.normalized;
	}

	public void Slice(Material mat)
	{

		Collider[] hits = Physics.OverlapBox(PlaneObj.position, new Vector3(5, 0.1f, 5), PlaneObj.rotation, layerMask);

		if (hits.Length <= 0)
			return;

		for (int i = 0; i < hits.Length; i++)
		{
			rbParentHull = hits[i].gameObject.GetComponent<Rigidbody>();
			SlicedHull hull = SliceObject(hits[i].gameObject, mat);
			if (hull != null)
			{
				GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, mat);
				GameObject top = hull.CreateUpperHull(hits[i].gameObject, mat);
				AddComponents(bottom);
				AddComponents(top);

				ObjectSpawner.instance.RemoveObjectFromList(hits[i].gameObject.GetComponent<Rigidbody>());
				Destroy(hits[i].gameObject);
			}
		}


	}

	void AddComponents(GameObject obj)
	{
		obj.AddComponent<MeshCollider>().convex = true;
		childRB = obj.AddComponent<Rigidbody>();

		childRB.linearVelocity = rbParentHull.linearVelocity/8;
		childRB.AddRelativeForce(Random.insideUnitCircle * fragmentSpawnForce);
		ObjectSpawner.instance.AddObjectToList(childRB);
		obj.layer = LayerMaskint;
	}
	public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
	{
		// slice the provided object using the transforms of this object
		if (obj.GetComponent<MeshFilter>() == null)
			return null;

		return obj.Slice(PlaneObj.position, PlaneObj.up, crossSectionMaterial);
	}


#if UNITY_EDITOR
	/**
	 * This is for Visual debugging purposes in the editor 
	 */
	public void OnDrawGizmos()
	{
		
	}

#endif


}
