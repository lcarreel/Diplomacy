using UnityEngine;
using System.Collections;

public class cursorCreator : MonoBehaviour {

    [SerializeField]
    private GameObject cursorTargetPrefab;
    private cursorTarget cursorTarget;

    public void Create()
    {
        if (cursorTarget == null)
            cursorTarget = Instantiate(cursorTargetPrefab).GetComponent<cursorTarget>();
        cursorTarget.transform.position = (Vector2)GameMaster.Instance.currentCamera.ScreenToWorldPoint(Input.mousePosition);
        cursorTarget.ActivateCollider();
    }

    public void UpdatePosition()
    {
        cursorTarget.transform.position = (Vector2)GameMaster.Instance.currentCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public GameObject ReturnTargetAndDisappear()
    {
        GameObject res = cursorTarget.calculateTarget();
        cursorTarget.disappear();
        cursorTarget = null;
        return res;
    }
}
