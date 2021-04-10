using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RenderCameraTextureToFile : MonoBehaviour
{
    public Transform Model;

    public bool RotateX, RotateY, RotateZ;

    private Camera cam;

    private bool processing = false;

    [Header("Optional")]
    public int framesPerXSide = 16;  // should be power of 2
    public int framesPerYSide = 16;  // should be power of 2
    public float totalAnimationTime = 2f;

    public void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    public void RenderSingleCameraViewToFile()
    {
        Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        image = MakeScreenShot(image);
        SaveRenderedImage(image);
        Destroy(image);
        Exit();
    }

    public void RenderSpritesheetWithRotationAnimation()
    {
        if (!processing)
        {
            processing = true;
            StartCoroutine(RotateModelAndRenderSpritesheet());
        }
        
    }

    private IEnumerator RotateModelAndRenderSpritesheet()
    {
        int frames = framesPerXSide * framesPerYSide;
        float timeStep = totalAnimationTime / frames;
        float rotationStep = 360 / frames;

        Texture2D image = new Texture2D(cam.targetTexture.width * framesPerXSide, cam.targetTexture.height * framesPerYSide);
        for (int i = 0; i < framesPerXSide; i++)
        {
            for (int j = 0; j < framesPerYSide; j++)
            {
                image = MakeScreenShot(image, i * cam.targetTexture.width, j * cam.targetTexture.height);
                yield return new WaitForSeconds(timeStep - Time.deltaTime);
                RotateModelInSelectedAxis(rotationStep);
            }
        }
        yield return new WaitForSeconds(timeStep);

        SaveRenderedImage(image);
        Destroy(image);
        Exit();
    }

    private void RotateModelInSelectedAxis(float rotationStep)
    {        
        Vector3 requestedRotation = new Vector3(RotateX ? rotationStep : 0, RotateY ? rotationStep : 0, RotateZ ? rotationStep : 0);        
        Model.Rotate(requestedRotation);
    }

    /**
     * <summary>
     * Switches camera view to render texture
     * then switches back
     * </summary>
     */
    private Texture2D MakeScreenShot(Texture2D image, int destX = 0, int destY = 0)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;

        cam.Render();
        
        image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), destX, destY);
        image.Apply();

        RenderTexture.active = currentRT;
        return image;
    }

    private static void SaveRenderedImage(Texture2D image)
    {
        var bytes = image.EncodeToPNG();
        
        string path = string.Format("{0}/Prefabs/3dModels/renders/render.png", Application.dataPath);
        File.WriteAllBytes(path, bytes);
        Debug.Log("Saved rendered image to " + path);
    }

    private void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
