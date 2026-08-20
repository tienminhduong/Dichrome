using UnityEngine;

public class CommonSetting : Singleton<CommonSetting>
{
    [SerializeField] private float moveAnimTime = 0.2f;
    public float MoveAnimTime => moveAnimTime;


}