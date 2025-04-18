using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageItemView : MonoBehaviour
{
    [SerializeField] private Image imgTutorial, imgLocked, imgLevelTransitionVer, imgLevelTransitionHoz;
    [SerializeField] private List<Image> imgStars;
    [SerializeField] private TextMeshProUGUI txtStage;
    [SerializeField] private Button btnSelectLevel;
    [SerializeField] private GameModel gameModel;
    private StageSaveData _item;

    private void Start()
    {
        btnSelectLevel.onClick.AddListener(() =>
        {
            if (!_item.IsUnlocked) return;
            gameModel.OpenStage(_item.Stage);
        });
    }

    public void Init(StageSaveData item)
    {
        _item = item;
        for (var i = 0; i < imgStars.Count; i++)
        {
            var imgStar = imgStars[i];
            imgStar.gameObject.SetActive(i < item.Star);
        }


        if (item.Stage == 1)
        {
            imgTutorial.Show();
            txtStage.Hide();
        }
        else
        {
            imgTutorial.Hide();
            txtStage.Show();
            txtStage.text = _item.Stage.ToString();
        }

        imgLocked.gameObject.SetActive(!item.IsUnlocked);
        imgLevelTransitionHoz.gameObject.SetActive(item.Stage % 8 is 1 or 0);
        imgLevelTransitionVer.gameObject.SetActive(item.Stage % 4 == 0);
    }
}