using UnityEngine;

public class MazeCellView : MonoBehaviour
{
    [SerializeField] private GameObject leftWall, rightWall, upWall, downWall;
    private MazeCellItemData _data;


    public void Init(MazeCellItemData data)
    {
        _data = data;
        leftWall.gameObject.SetActive(data.HasLeftWall);
        rightWall.gameObject.SetActive(data.HasRightWall);
        upWall.gameObject.SetActive(data.HasUpWall);
        downWall.gameObject.SetActive(data.HasDownWall);
    }
}