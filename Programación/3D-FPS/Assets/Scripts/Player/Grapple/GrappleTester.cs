using UnityEngine;

public class GrappleTester : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GrappleEffect GappleEffect;
	[SerializeField] private float maxRayDistance = 50f;

	private Camera mainCamera;

	private void Awake()
	{
		mainCamera = Camera.main;

		if (GappleEffect == null)
			GappleEffect = GetComponent<GrappleEffect>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			RaycastHit hit;
			if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, maxRayDistance))
			{
				// Apuntar el grappling hacia el punto de impacto
				GappleEffect.transform.LookAt(hit.point);
				// Iniciar el grappling
				GappleEffect.StartSwing(hit.point);
			}
		}

		if (Input.GetMouseButtonUp(1))
		{
			GappleEffect.StopSwing();
		}
	}
}