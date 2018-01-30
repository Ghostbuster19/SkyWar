*Used technologies:*

The main technology used is the Unity game engine accompanied by C#-Scripts, it controls
physics calculation, collision detection , object rendering and much more. The C#-Scripts
are used for implementing game logik like the ressource system.

The game will be divided into two layers, a data and a view layer. This segregation has
the following benefits:
It cuts the execution time by having the view layer handle the rendering and the data 
layer handling the game logic making the game much smoother and offering a better gaming
experience.
It also speerates the access of the components to different points, making it a lot easier
to make changes and modifications to specific components without effecting anything else.
It divides the work to both the CPU and the GPU reliefing both for better efficiency and
game performance.

The data layer holds the game logic and the information about the game objects, even the 
ones which are not visible or rendered. 
The view layer contains all the visible game objects with their textures, materials, effects
and animations.
