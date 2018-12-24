using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour
{

    public float devHeight = 19.2f;
    public float devWidth = 10.8f;

    // Use this for initialization
    void Start()
    {

        float screenHeight = this.GetComponent<Camera>().pixelHeight;
        float width = this.GetComponent<Camera>().pixelWidth;

//        Debug.Log("screenHeight = " + screenHeight);
//        Debug.Log("screenwidth = " + Screen.width);

        //this.GetComponent<Camera>().orthographicSize = screenHeight / 200.0f;

        float orthographicSize = this.GetComponent<Camera>().orthographicSize;

        float aspectRatio = width * 1.0f / screenHeight;

        float cameraWidth = orthographicSize * 2 * aspectRatio;

//        Debug.Log("cameraWidth = " + cameraWidth);

        if (cameraWidth < devWidth)
        {
            orthographicSize = devWidth / (2 * aspectRatio);
            Debug.Log("new orthographicSize = " + orthographicSize);
            this.GetComponent<Camera>().orthographicSize = orthographicSize;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}