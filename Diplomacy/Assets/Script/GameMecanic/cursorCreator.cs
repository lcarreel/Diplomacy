using UnityEngine;
using System.Collections;

public class cursorCreator : MonoBehaviour {

    [SerializeField]
    private GameObject cursorTargetPrefab;
    private cursorTarget cursorTarget;

    public void Create(GameObject origin)
    {
        if (cursorTarget == null)
            cursorTarget = Instantiate(cursorTargetPrefab).GetComponent<cursorTarget>();
        cursorTarget.transform.position = (Vector2)GameMaster.Instance.currentCamera.ScreenToWorldPoint(Input.mousePosition);
        cursorTarget.ActivateCollider();
        cursorTarget.origin = origin;
    }

    public void UpdatePosition()
    {
        cursorTarget.transform.position = (Vector2)GameMaster.Instance.currentCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public GameObject ReturnTargetAndDisappear()
    {
        GameObject res = cursorTarget.getTarget();
        cursorTarget.disappear();
        cursorTarget = null;
        return res;
    }
}
