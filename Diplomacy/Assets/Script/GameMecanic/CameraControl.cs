using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    private Camera _camera;

    public int maxSize = 20;
    public int minSize = 4;

    public float camaeraSpeedMultiplier = 1.5f;
    private float cameraSpeed = 5;

    public Vector2 screenBorder = new Vector2(29,16);
    private Vector2 positionLimit = new Vector2(29,16);
    private Vector2 stepForBorder;

	// Use this for initialization
	void Start () {
        _camera = this.GetComponent<Camera>();
        float zoomGapMinMax = maxSize - minSize;
        stepForBorder = new Vector2(screenBorder.x/zoomGapMinMax, screenBorder.y / zoomGapMinMax );
        positionLimit = stepForBorder * (maxSize - Camera.main.orthographicSize);
    }
	
	// Update is called once per frame
	void Update () {

        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue > 0) // forward
        {
            _camera.orthographicSize = Mathf.Max(Camera.main.orthographicSize - 0.5f, minSize);
            float zoomGap = maxSize - Camera.main.orthographicSize;
            cameraSpeed = zoomGap * camaeraSpeedMultiplier;
            positionLimit = stepForBorder * zoomGap;
        } else if(scrollValue < 0)
        {
            _camera.orthographicSize = Mathf.Min(Camera.main.orthographicSize + 0.5f, maxSize);
            float zoomGap = maxSize - Camera.main.orthographicSize;
            cameraSpeed = zoomGap * camaeraSpeedMultiplier;
            positionLimit = stepForBorder * zoomGap;
        }

        float posX = Input.mousePosition.x;
        float posY = Input.mousePosition.y;

        if (posX > (Screen.width * 0.9) && this.transform.position.x < positionLimit.x)
        {
            this.transform.position += (Vector3)(Vector2.right * Time.deltaTime * cameraSpeed);
        }
        if (posX < (Screen.width * 0.1f) && this.transform.position.x > - positionLimit.x)
        {
            this.transform.position += (Vector3)(Vector2.left * Time.deltaTime * cameraSpeed);
        }
        if (posY > (Screen.height * 0.9f) && this.transform.position.y < positionLimit.y)
        {
            this.transform.position += (Vector3)(Vector2.up * Time.deltaTime * cameraSpeed);
        }
        if (posY < (Screen.height * 0.1f) && this.transform.position.y > - positionLimit.y)
        {
            this.transform.position += (Vector3)(Vector2.down * Time.deltaTime * cameraSpeed);
        }

        Vector3 corrigedPosition = this.transform.position;
        if(this.transform.position.x > positionLimit.x)
        {
            corrigedPosition.x = positionLimit.x;
        }
        if (this.transform.position.x < -positionLimit.x)
        {
            corrigedPosition.x = -positionLimit.x;
        }
        if (this.transform.position.y > positionLimit.y)
        {
            corrigedPosition.y = positionLimit.y;
        }
        if (this.transform.position.y < -positionLimit.y)
        {
            corrigedPosition.y = -positionLimit.y;
        }
        this.transform.position = corrigedPosition;
    }

    public void UnenableAnimator()
    {
        print("Znimator");
        GetComponent<Animator>().enabled = false;
    }



}
