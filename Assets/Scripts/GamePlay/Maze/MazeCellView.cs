using UnityEngine;

public class MazeCellView : MonoBehaviour
{
    [SerializeField] private GameObject leftWall, rightWall, upWall, downWall;
    public MazeCellItemData Data { get; private set; }


    public void Init(MazeCellItemData data)
    {
        Data = data;
        leftWall.gameObject.SetActive(data.HasLeftWall);
        rightWall.gameObject.SetActive(data.HasRightWall);
        upWall.gameObject.SetActive(data.HasUpWall);
        downWall.gameObject.SetActive(data.HasDownWall);
    }
}