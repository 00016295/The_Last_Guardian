//using UnityEngine;
//using System.Collections;

//public class Movement_Sound : MonoBehaviour
//{

//    public AudioClip footStepSFX;

//    private Player movement;

//    private void Start()
//    {
//        movement = GetComponent<Player>();
//        StartCoroutine(PlayFootSteps());
//    }

//    IEnumerator PlayFootSteps()
//    {
//        while (true)
//        {
//            if (Mathf.Abs(movement.movement) > 0f && movement.isGround)
//            {
//                {
//                    AudioManager.instance.PlaySFX(footStepSFX);

//                }
//                yield return new WaitForSeconds(0.5f);
//            }

//        }
//    }
//}
