using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ResourcesUI : MonoBehaviour {

    [SerializeField]
    private Image slot1;
    [SerializeField]
    private Image slot2;
    [SerializeField]
    private Image slot3;

    public void UpdateIcon(List<UtilType.Supply> supplyList)
    {
        slot1.sprite = GameMaster.Instance.getResourcesIcon(supplyList[0]);
        slot2.sprite = GameMaster.Instance.getResourcesIcon(supplyList[1]);
        slot3.sprite = GameMaster.Instance.getResourcesIcon(supplyList[2]);
    }
}
