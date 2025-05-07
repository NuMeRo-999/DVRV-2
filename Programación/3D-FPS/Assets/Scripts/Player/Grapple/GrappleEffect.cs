using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrappleEffect : MonoBehaviour
{
	[Header("References")]
	public LineRenderer lr;
	public Transform gunTip, cam, player;
	public LayerMask whatIsGrappleable;
	public PlayerMovement pm;

	[Header("Swinging")]
	private float maxSwingDistance = 25f;
	private Vector3 swingPoint;
	private SpringJoint joint;

	[Header("Grapple Effect")]
	public float Speed = 3f;
	public float SpiralSpeed = 4f;
	public float DistanceSpeed = 2f;
	public float Gravity = 0.5f;
	public int Segments = 100;
	public Vector2 Magnitude = Vector2.one;
	public float Frequency = 0.5f;
	public float HorizontalOffset = 0.25f;
	public float Strength = 0.5f;
	public float Scale = 0.25f;
	public AnimationCurve Curve = new AnimationCurve();
	public AnimationCurve MagnitudeOverTime = new AnimationCurve();
	public AnimationCurve MagnitudeOverDistance = new AnimationCurve();
	public AnimationCurve GravityOverDistance = new AnimationCurve();
	public AnimationCurve GravityOverTime = new AnimationCurve();

	private float scaledTimeOffset = 0f;
	private float spiralTimeOffset = 0f;
	private float lastGrappleTime = 0f;
	private Vector3 currentGrapplePosition;

	void LateUpdate()
	{
		if (joint != null)
		{
			DrawRope();
		}
	}

	public void StartSwing(Vector3 targetPoint)
	{
		pm.isSwinging = true;

		swingPoint = targetPoint;
		joint = player.gameObject.AddComponent<SpringJoint>();
		joint.autoConfigureConnectedAnchor = false;
		joint.connectedAnchor = swingPoint;

		float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

		// The distance grapple will try to keep from grapple point.
		joint.maxDistance = distanceFromPoint * 0.8f;
		joint.minDistance = distanceFromPoint * 0.25f;

		// Customize values as you like
		joint.spring = 4.5f;
		joint.damper = 7f;
		joint.massScale = 4.5f;

		lr.positionCount = Segments;
		currentGrapplePosition = gunTip.position;

		// Initialize grapple effect
		scaledTimeOffset = spiralTimeOffset = 0f;
		lastGrappleTime = Time.time * 10f;
	}

	public void StopSwing()
	{
		pm.isSwinging = false;

		lr.positionCount = 0;
		Destroy(joint);
	}

	void DrawRope()
	{
		if (!pm.isSwinging) return;

		var difference = swingPoint - gunTip.position;
		var direction = difference.normalized;
		var distanceMultiplier = Mathf.Clamp01(scaledTimeOffset * DistanceSpeed);
		var distance = difference.magnitude * distanceMultiplier;

		scaledTimeOffset += Speed * Time.deltaTime;
		if (distanceMultiplier < 1f)
			spiralTimeOffset += Speed * SpiralSpeed * Time.deltaTime;

		for (int i = 0; i < lr.positionCount; i++)
		{
			var t = (float)i / lr.positionCount;
			var position = gunTip.position;
			var forwardOffset = direction * (t * distance);
			position += forwardOffset;

			var curveSamplePosition = forwardOffset.magnitude * Frequency - spiralTimeOffset;

			var verticalOffset = transform.up * Curve.Evaluate(curveSamplePosition);
			var horizontalOffset = transform.right * Curve.Evaluate(curveSamplePosition + HorizontalOffset);

			verticalOffset *= Magnitude.y;
			horizontalOffset *= Magnitude.x;

			var noiseSamplePosition = -t * Scale + scaledTimeOffset + lastGrappleTime;
			verticalOffset += transform.up * ((Mathf.PerlinNoise(0f, noiseSamplePosition) - 0.5f) * 2f * Strength);
			horizontalOffset += transform.right * ((Mathf.PerlinNoise(noiseSamplePosition, 0f) - 0.5f) * 2f * Strength);

			var magnitude = MagnitudeOverTime.Evaluate(scaledTimeOffset) * MagnitudeOverDistance.Evaluate(t);
			verticalOffset *= magnitude;
			horizontalOffset *= magnitude;

			position += verticalOffset;
			position += horizontalOffset;

			position += Vector3.up * (GravityOverDistance.Evaluate(t) * GravityOverTime.Evaluate(scaledTimeOffset) * Gravity);

			lr.SetPosition(i, position);
		}
	}
}