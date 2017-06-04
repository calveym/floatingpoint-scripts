using UnityEngine;
using System.Collections;
using VRTK;

public class HiResPhoto : MonoBehaviour
{
    private int resWidth = 4920;
    private int resHeight = 3264;
    private Camera cam;
    public GameObject target;
    VRTK_ControllerEvents events;
    int numShots = 0;

    private void Start()
    {
        events = ReferenceManager.instance.rightEvents;
    }

    private void Update()
    {
        if(target)
        {
            transform.LookAt(target.transform);
        }
    }

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screen_{1}x{2}_{3}{4}.png",
                             Application.dataPath,
                             width, height, gameObject.name, numShots.ToString());
    }

    public void TakeHiResShot()
    {
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = ScreenShotName(resWidth, resHeight);
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
        numShots++;
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown("k"))
        {
            TakeHiResShot();
        }
        else if (events.touchpadPressed)
        {
            TakeHiResShot();
        }
    }
}