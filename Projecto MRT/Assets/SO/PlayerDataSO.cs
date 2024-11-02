using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerDataSO", order = 1)]
public class PlayerDataSO : ScriptableObject
{

    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;

}