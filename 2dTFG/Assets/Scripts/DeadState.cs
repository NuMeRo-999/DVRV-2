using Cainos.CustomizablePixelCharacter;
using UnityEngine;

public class DeadState : MonoBehaviour
{

    private PixelCharacterController controller;
    private PixelCharacter character;

    void Start()
    {
        character = GetComponent<PixelCharacter>(); 
    }

    void Update()
    {
        character.IsDead = true;
    }
}
