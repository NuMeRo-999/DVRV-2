using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BallPanel : MonoBehaviour
{
    private Camera camaraPpal;
    public Canvas canvas;

    void Start()
    {
        camaraPpal = Camera.main;
    }

    void Update()
    {
        if(camaraPpal != null){
            // transform.LookAt(camaraPpal.transform);
            // transform.Rotate(0,180,0);
            canvas.transform.rotation = Quaternion.LookRotation(camaraPpal.transform.forward);
        }
    }
}
