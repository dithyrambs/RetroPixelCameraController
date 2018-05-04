# RetroPixelCameraController

A drag and drop Unity component for handling camera settings and movement for 2D pixel art games.

Simply add this component to the same game object that the camera is on.

Set the Focus Game Object to the game object you want the camera to focus on.


Some features:

-automatically sets orthographic projection mode and calculates orthographic size

-reduces glitchy pixel effects by rounding camera position to nearest pixel

-calculates smooth movement 

-ability to set "deadzone" where focus destination isn't updated

-handy debug visualization in Scene view: blue is camera size, green is deadzone size, red cross-hair is the camera focus destination

-deadzone only accounts for  horizontal distance; vertical camera movement is one-to-one with focus object movement


Best practices for how to work with pixel art in Unity and how to use this component outlined here:

http://gamasutra.com/blogs/AlexBelzer/20180504/317472/Working_with_Pixel_Art_Sprites_in_Unity_The_Camera.php
