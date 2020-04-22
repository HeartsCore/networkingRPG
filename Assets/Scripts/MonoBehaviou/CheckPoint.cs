using UnityEngine;


public class CheckPoint : MonoBehaviour
{
    #region Private Data
    // this indicate if this checkpoint was already used
    private bool _notUsed = true;
    #endregion


    #region Unity Methods
    //verify if the player collides with the checkpoint area
    private void OnTriggerEnter(Collider collision)
    {
        var player = collision.gameObject.GetComponent<Character>();
        if (player != null)
        {
            if (_notUsed)
            {                
                player.RespawnPosition = transform.position;
            }
        }
    }
    #endregion
}
