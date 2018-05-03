using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetroPixelCameraController : MonoBehaviour
{
    public bool debugDrawOn = true;
    public bool debugDrawWorld = false;
    public bool boundByWorld = false;
    public bool smoothFollow = false;

    public GameObject focusGameObject;
    public float cameraSpeed;
    public int cameraPixelWidth;
    public int cameraPixelHeight;
    public int cameraScale = 1;
    public float tileSize;
    public float mapX = 0;
    public float mapY = 0;
    public float mapWidth;
    public float mapHeight;
    public float deadzoneWidth;
    public float deadzoneWidthOffset;

    private float minBoundX;
    private float maxBoundX;
    private float minBoundY;
    private float maxBoundY;
    private Camera mainCamera;
    private Rect deadzone = new Rect();
    private float focusDestinationX;
    private float focusDestinationY;
    private float cameraPositionX;
    private float cameraPositionY;

    //Debug draw textures
    private Texture2D deadzoneDebugTexture;
    private Texture2D worldBoundsDebugTexture;
    private Texture2D cameraDebugTexture;
    private Texture2D worldDebugTexture;
    private Rect cameraDebugRect;
    private Rect worldDebugRect;


    public void OnAwake()
    {
        deadzone = new Rect();
        //Debug
        cameraDebugRect = new Rect();
        worldDebugRect = new Rect();

    }

    //If you want to set the camera in script instead of in the editor, call this function after you use AddComponent
    public void SetCamera(GameObject focusObjectGO, float cameraSpeed, int cameraPixelWidth, int cameraPixelHeight, int tileSize, int cameraScale, float deadzoneW, float deadzoneOffset, bool isLerping = false, float mapX = 0, float mapY = 0, float mapWidth = 0, float mapHeight = 0)
    {
        this.focusGameObject = focusObjectGO;
        this.cameraSpeed = cameraSpeed;
        this.smoothFollow = isLerping;
        this.tileSize = tileSize;
        this.cameraPixelWidth = cameraPixelWidth;
        this.cameraPixelHeight = cameraPixelHeight;
        this.cameraScale = cameraScale;
        this.deadzoneWidth = deadzoneW;
        this.deadzoneWidthOffset = deadzoneOffset;
        this.mapX = mapX;
        this.mapY = mapY;
        this.mapWidth = mapWidth/tileSize;
        this.mapHeight= mapHeight/tileSize;

        OnValidate();
    }

    public void OnValidate()
    {

        if (mapX != mapWidth || mapY != mapHeight) boundByWorld = true;
        minBoundX = mapX + (cameraPixelWidth / 2) / tileSize;
        maxBoundX = mapX + mapWidth - cameraPixelWidth / (2 * tileSize);
        minBoundY = mapY - mapHeight + cameraPixelHeight / (2 * tileSize);
        maxBoundY = mapY - (cameraPixelHeight / 2) / tileSize;
        //Grab camera and set properties
        mainCamera = gameObject.GetComponent<Camera>();
        mainCamera.cameraType = CameraType.Game;
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = (cameraPixelHeight) / (2 * tileSize);
        mainCamera.pixelRect.size.Set(cameraPixelWidth, cameraPixelHeight);
        mainCamera.nearClipPlane = 0.3f;
        mainCamera.farClipPlane = 1000f;
        mainCamera.depth = -2;
        //Sets the build resolution of the display based on camera dimensions and scale factor
        //Note: You still have to manually set the display in the editor itself
        Screen.SetResolution(cameraPixelWidth* cameraScale, cameraPixelHeight* cameraScale, false);

        //Setup deadzone: while player moves in this area, camera won't move find a new location to focus to
        deadzone.Set((cameraPixelWidth - deadzoneWidth) / 2 * tileSize, cameraPixelHeight / tileSize * 0.25f, deadzoneWidth / tileSize, cameraPixelHeight / tileSize);

        cameraDebugRect.Set(mainCamera.transform.position.x, mainCamera.transform.position.y, cameraPixelWidth / tileSize, cameraPixelHeight / tileSize);
        worldDebugRect.Set(mapX, mapY + mapHeight, mapWidth, mapHeight);
    }

    void OnDrawGizmos()
    {
        //Handy debug draw calls to visualize camera and deadzone in Scene view. Toggle on off with the Debug Draw On

        if (debugDrawOn == true )
        {
            //Deadzone
            if (deadzone != null && deadzoneDebugTexture != null) {
                Gizmos.DrawGUITexture(deadzone, deadzoneDebugTexture);
            }
            else
            {
                Color colorGreen = Color.green;
                colorGreen.a = 0.5f;
                deadzoneDebugTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                deadzoneDebugTexture.SetPixel(0, 0, colorGreen);
                deadzoneDebugTexture.Apply();
                if (focusGameObject != null)
                {
                    deadzone.position = new Vector3(focusGameObject.transform.position.x, focusGameObject.transform.position.y, 0);
                }
            }
            //Camera
            if (cameraDebugRect != null && cameraDebugTexture != null)
            {
                cameraDebugRect.position = new Vector3(mainCamera.transform.position.x - cameraDebugRect.width * .5f, mainCamera.transform.position.y - cameraDebugRect.height * .5f, 0);
                Gizmos.DrawGUITexture(cameraDebugRect, cameraDebugTexture);
            } else
            {
                Color colorBlue = Color.blue;
                colorBlue.a = 0.25f;
                cameraDebugTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                cameraDebugTexture.SetPixel(0, 0, colorBlue);
                cameraDebugTexture.Apply();
            }            
            Debug.DrawRay(new Vector2(focusDestinationX, focusDestinationY), Vector3.left, Color.red, 1 / 60);
            Debug.DrawRay(new Vector2(focusDestinationX, focusDestinationY), Vector3.right, Color.red, 1 / 60);
            Debug.DrawRay(new Vector2(focusDestinationX, focusDestinationY), Vector3.down, Color.red, 1 / 60);
            Debug.DrawRay(new Vector2(focusDestinationX, focusDestinationY), Vector3.up, Color.red, 1 / 60);
        }
        //Worldbounds -- not ideal to have on, but handy to make sure you've set the right world dimensions
        if (debugDrawWorld == true)
        {
            if (worldDebugRect != null && worldBoundsDebugTexture != null)
            {
                worldDebugRect.position = new Vector3(mapX, mapY - mapHeight, 0);
                Gizmos.DrawGUITexture(worldDebugRect, worldBoundsDebugTexture);
            }
            else
            {
                Color colorRed = Color.red;
                colorRed.a = 0.25f;
                worldBoundsDebugTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                worldBoundsDebugTexture.SetPixel(0, 0, colorRed);
                worldBoundsDebugTexture.Apply();
            }
        }
    }

    private void FixedUpdate()
    {
        float cameraMoveSpeedDelta = Time.fixedDeltaTime * cameraSpeed;

        cameraPositionX = mainCamera.transform.position.x;
        cameraPositionY = mainCamera.transform.position.y;

        focusDestinationY = focusGameObject.transform.position.y;

        //check if player is inside the deadzone
        if ( !deadzone.Contains(focusGameObject.transform.position ))
        {
            focusDestinationX = focusGameObject.transform.position.x;

            //Set cameraFocus to edge of deadzone left or right
            if (deadzone.center.x < focusDestinationX )
            {
                deadzone.position = new Vector3(focusDestinationX-deadzone.width, cameraPositionY - deadzone.height / 2, 0);
                //Find new camera focus destination in relation to deadzone position
                focusDestinationX = deadzone.position.x + deadzoneWidth/tileSize + deadzoneWidthOffset / tileSize;
            }
            else if (focusDestinationX < deadzone.center.x)
            {
                deadzone.position = new Vector3(focusDestinationX, cameraPositionY - deadzone.height / 2, 0);
                //Find new camera focus destination in relation to deadzone position
                focusDestinationX = deadzone.position.x - deadzoneWidthOffset / tileSize;
            }
        }

        if (smoothFollow == true)
        {
            //Ease in/out from current camera position towards focus destination
            cameraPositionX = Mathf.SmoothStep(cameraPositionX, focusDestinationX, cameraMoveSpeedDelta);
            cameraPositionY = Mathf.SmoothStep(cameraPositionY, focusDestinationY, cameraMoveSpeedDelta);
        }
        else
        {
            //Or move without easing
            cameraPositionX = Mathf.MoveTowards(cameraPositionX, focusDestinationX, cameraMoveSpeedDelta);
            cameraPositionY = Mathf.MoveTowards(cameraPositionY, focusDestinationY, cameraMoveSpeedDelta);
        }

        //Checks if camera should be bounded, and if so bounds it to the level's width and height
        if (boundByWorld == true)
        {
            cameraPositionX = Mathf.Clamp(cameraPositionX, minBoundX, maxBoundX);
            cameraPositionY = Mathf.Clamp(cameraPositionY, minBoundY, maxBoundY);
        }

        //Finally we need to round to nearest pixel to make it not have glitchy pixel weirdness (professional game dev term)
        cameraPositionX = Mathf.Round(cameraPositionX * tileSize) / tileSize;
        cameraPositionY = Mathf.Round(cameraPositionY * tileSize) / tileSize;

        //And now actually transform the camera position to the derived focusDestination
        mainCamera.transform.position = new Vector3(cameraPositionX, cameraPositionY, -2);
    }

}