using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{
    void OnTriggerStay(Collider col)
    {
        Player player = col.gameObject.GetComponent<Player>();
        if (player != null)
        {
            if (player.IsGrounded() && !player.HasLanded())
            {
                Vector3 landPosition = transform.position;
                landPosition.y += 2f;
                player.Land(landPosition);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        Player player = col.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.SetLanded(false);
        }
    }
}
