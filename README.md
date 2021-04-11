# 3D Model Sprite and Animation Renderer

This Unity project allows rendering of your 3D model to a flat sprite, which can be used in your 2D game.
It also allows rendering a spritesheet of your 3D model during rotation.

I created models using ProBuilder and used renders in my Starbase Destroyer game which is available on:

* https://marot.itch.io/starbase-destroyer
* https://simmer.io/@Marot/starbase-destroyer

## Thing you may want to modify

### Scenes

Project contains 2 scenes with same functionality but using two types of camera:

* perspective
* ortographic 

For single rotating object perspective camera may be prefered but there are situations (like when creating tile palette)
where you need far elements to be the same size as ones just in front of a camera. Then use ortographic camera for rendering a sprite(sheet).

#### **Lights** object in scenes

Setup Directional Light:

* rotation, 
* intensity 
* indirect multiplier 

to achieve different results.

If render is too dark, increase intensity.
If it is overexposed, decrease light intensity.

#### RenderTextureSettings

RenderTextureSettings prefab is places in Assets/Prefabs/3dModels/renders/RenderTextureSettings

Following parameteres should be modified based on actual need:

* Size - size of target rendered texture in pixels or a single frame of animation in a spritesheet
* Anti-aliasing - specify if you prefer to have this filter applied to your render
* Filter Mode - specify which filter you prefer to have applied to your render (Point for no filter)

#### RenderTextureCamera in scenes

Model - game object you want to rotate

Rotate X/Y/Z - rotation axis

Frames per X/Y Side - number of images in spritesheet on specific axis. It is best for further use in Unity when render texture width multiplied by number of frames per X axis and render texture height multiplied by number of frames per Y axis is the power of 2. 