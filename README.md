# RetroPixelCameraController

<p><strong>A Deep Dive into 2D Cameras</strong></p>

<p>Chances are, if you first worked on a pixel art game in Unity you ended up with some problems.</p>

<figure data-orig-height="370" data-orig-width="496"><img alt="image" data-orig-height="370" data-orig-width="496" src="https://78.media.tumblr.com/db066e3b4062d804ef90371f806842aa/tumblr_inline_p847k6n1Na1sph69g_540.gif" /></figure>

<p>I wish this gif was just a manufactured exaggeration. It&rsquo;s not.</p>

<p>Unity, being a 3D-first engine, presupposes that you are making a 3D game, or at best a 2D game with a 3D camera. Which is why for retro pixel-art games, you need to manually set the&nbsp;<a href="https://docs.unity3d.com/Manual/class-Camera.html">Projection</a>&nbsp;of the camera to &ldquo;Orthographic&rdquo; (more on&nbsp;<a href="https://en.wikipedia.org/wiki/Orthographic_projection">orthographic projection here</a>). Now all elements of the z-axis will be drawn &ldquo;flat&rdquo;, as if they are all at the same depth.&nbsp;</p>

<p>Whether or not you&rsquo;re making a tile-based game (classic Nintendo game worlds were made out of &ldquo;tiles&rdquo;,&nbsp;<a href="https://en.wikipedia.org/wiki/Tile-based_video_game">more on that here</a>), this is a good time to decide the Pixels to Units ratio. However if you are making a tile-based game, then you want the tile size and Pixels to Units ratio to be one and the same.&nbsp; (Actually, if you&rsquo;ve been following along in this series, you should have already decided that&nbsp;<a href="https://github.com/dithyrambs/SpriteProcessorScript">when importing the pixel art</a>). The pixel-size of our tiles is a number that&rsquo;s going to be super important for understanding how to adjust the camera so you don&rsquo;t end up with glitchy pixel weirdness (professional game dev term) like the above gif. My example world is made out of tiles of 16x16 pixels, and so is our frog character.&nbsp;</p>

<p>The first way tile-size comes into play is in setting the&nbsp;<a href="https://docs.unity3d.com/ScriptReference/Camera-orthographicSize.html">orthographic size</a>.&nbsp; A simple formula for calculating it is cameraSizeInPixels / ( 2 * tileSize). But in order for the camera to not cause glitchy weirdness, the following rule must be strictly obeyed:&nbsp;<strong>the resolution of the display window and the resolution of the camera must both be divisible by the Pixels to Units ratio.</strong>&nbsp;What this means is that if you are trying to have a classic Super Nintendo camera size of 256x224, but want it to play on a modern 16:9 screen (let&rsquo;s say 1920x1080 pixels), you are going to have glitchy pixels, even if you change the width to accommodate the 16:9 ratio. In fact, 16 just isn&rsquo;t a factor of 1080. But, if you set your Unity camera size to 256x144 (perfectly divisible by 16), you will end up with a good result if you display it at 1280x720 (divisible by 256x144, and 16).&nbsp;</p>

<p>Now if you want to display at 1920x1080 using 16x16 tiles&nbsp;<a href="https://www.youtube.com/watch?v=yI8JrBNTwkc">there are some clever solutions for handling this</a>, and even some drag and drop solutions on the Unity Asset Store, so no need to abandon hope, but that&rsquo;s a bit outside the scope of this article.&nbsp;</p>

<p>If you do plan on choosing tile-sizes that will scale naturally to hi-res displays, you might find&nbsp;<a href="https://github.com/dithyrambs/RetroPixelCameraController">RetroPixelCameraController&nbsp;</a>useful.&nbsp;<a href="https://github.com/dithyrambs/RetroPixelCameraController">I&rsquo;ve gone ahead and uploaded it on Github</a>. Just drag and drop it onto the same Game Object with your Camera, and fill in the rest of the parameters.</p>

<figure data-orig-height="472" data-orig-width="418"><img alt="image" data-orig-height="472" data-orig-width="418" src="https://78.media.tumblr.com/21ccf9982817dcebf013a8404d04d2f3/tumblr_inline_p84ma4HyKH1sph69g_540.png" /></figure>

<p>It&rsquo;ll calculate things like the orthographic size, and change a lot of the camera settings for you. You just need to drag the Focus Game Object (usually whatever game object the player character is attached to). Additionally, like in the previous example if you have a camera of 256x144 dimensions, but want to output to a display resolution of 1280x720, you would set Camera Scale as 5.</p>

<p>Speaking of the display resolution, here&rsquo;s one of those Unity &ldquo;gotchyas&rdquo;: it&rsquo;s important to note that&nbsp;<strong>the display window in the Unity game editor window is not the display window size when you properly build and run the game</strong>. While this script will set the build display resolution correctly, it won&rsquo;t change the game preview window aspect. In other words,&nbsp;<a href="https://docs.unity3d.com/Manual/GameView.html">you&rsquo;ll have to manually set the aspect when testing the game in the Unity editor itself</a>, which is easy enough. I recommend just setting it to your camera dimension for testing.</p>

<p><strong>Bonus Round: Camera Deadzones</strong><br />
<br />
One of the cool things about this script is it let&rsquo;s you define a&nbsp;&ldquo;deadzone&rdquo; in which, while moving the camera&rsquo;s focus object inside of it, the camera will not choose a new focus destination. If you go into Scene view (and make sure the Debug Draw On boolean is checked), you should see something like this:</p>

<figure data-orig-height="223" data-orig-width="482"><img alt="image" data-orig-height="223" data-orig-width="482" src="https://78.media.tumblr.com/d7f88388d0fecb5f663f69a9f3257375/tumblr_inline_p84o70XNw41sph69g_540.gif" /></figure>

<p>You&rsquo;ll see the red cross-hairs (the camera&rsquo;s focus destination) flip sides when the focus object (the frog) reaches the other side of the deadzone (the green area). Oh, and the blue area represents the camera&rsquo;s size.&nbsp;</p>

<p>One other things the script handles is smooth motion. Combines with the horizontal deadzone, the lack of one-to-one motion should make things feel a bit nicer. Feel free to play around with the deadzone parameters in real-time until you arrive at something you like. One last tip: the camera speed should be at least as fast as the player character, if you want the camera to keep up with the player&rsquo;s movement and not lag behind (unless that&rsquo;s your intention). But too fast of a camera speed and it will sharply jump across the deadzone when the focus object switches sides. With some tweaks you should end up with something you&rsquo;re happy with it.</p>

<figure data-orig-height="269" data-orig-width="482"><img alt="image" data-orig-height="269" data-orig-width="482" src="https://78.media.tumblr.com/4227e42ed3514d3296c362906ae90371/tumblr_inline_p84obgffJ01sph69g_540.gif" /></figure>

<p>One last thing: you can just as easily add this component in script. Simply use the SetCamera method after you use AddComponent and you should be honky-dory.</p>

<figure data-orig-height="46" data-orig-width="920"><img data-img-key="584" data-orig-height="46" data-orig-width="920" src="https://78.media.tumblr.com/b60cf457b4940111147295a263bd6b8b/tumblr_inline_p84q2ycUaZ1sph69g_540.png" /></figure>

<p>Special thanks to Luis Zuno (<a href="https://twitter.com/ansimuz">@ansimuz</a>) for his adorably rad pixel art&nbsp;(CC-BY-3.0 License&nbsp;<a href="http://creativecommons.org/licenses/by/3.0/">http://creativecommons.org/licenses/by/3.0/</a>). You can find all of this art and more at&nbsp;<a href="http://pixelgameart.org/web/">http://pixelgameart.org/</a></p>

Original post:
http://gamasutra.com/blogs/AlexBelzer/20180504/317472/Working_with_Pixel_Art_Sprites_in_Unity_The_Camera.php
