using UnityEngine;

public class NumGen : MonoBehaviour, IInteractable
{
   public Vector3 rotationAxis = new Vector3(0, 1, 0);
   public float degreesPerActivation = 30f;

   public void Interact() {
      Debug.Log(Random.Range(0, 100));
      Vector3 rotationAmount = rotationAxis * degreesPerActivation;
      transform.Rotate(rotationAmount);
   }
}
