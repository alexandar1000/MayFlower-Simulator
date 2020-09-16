**_Note:This repository uses GitLFS, to use this repo you need to pull via Git and make sure GitLFS is installed locally_**

# Boat Attack
###### Demo Project using the Universal RP from Unity3D

The original Unity project has been created to aid the testing and development of Universal RP. The project is a small vertical slice of a boat racing game, complete with race-able boats and island environment.

**Project Features**:
  * Uses Universal RP from Unity
  * Mobile optimized, low poly, LODs, no compute
  * C# Jobs buoyancy system
  * Cinemachine camera setups
  * Shader Graph usage
  * Post-processing v3 with Volume blending
  * Addressables asset management package
  * Custom Universal RP rendering for planar reflections via [SRP callbacks](https://docs.unity3d.com/ScriptReference/Rendering.RenderPipelineManager.html)
  * Custom SciptableRenderPass usage for WaterFX and Caustics
  * Gerstner based water system in local package(WIP)
  * Much more..

# Usage

#### Getting the project
via Git:
  1. Make sure you have GitLFS installed, check [here](https://git-lfs.github.com) for details.
  2. Clone the repo as usual via cmd/terminal or in your favourite Git GUI software.
  3. Checkout the branch that matches the Unity verison you are using, eg `release/2019.3`

Unity Version: 2019.4.3f1

#### Load the project:

Scenes worth noting:
 - `Mayflower/dennis_canal.unity` - represents the shape of the sheffield
 - `Mayflowe/Updated_Main.unity` 

   

#### Build the project:
One thing to make sure you do before building is make sure to build the addressable assets, this can be done via the addressables window, for more information please checkout the addressables [package documentation](https://docs.unity3d.com/Packages/com.unity.addressables@latest).
Once the addressable assets are built you can continue to build a player as usual.

# Mayflower Overview

Scripts are placed under the `Mayflower/Scripts` folder

**Features include**:

1. Environement

* Battery
* Temperature

2. Navigation
3. Sensors

* 2D Lidar and 3D Lidar
* IMU
* Camera
* GPS
* Compass



**ROS**

To communicate to Rosbridge, use the scripts in the `ROS_Sharp` folder 



# Credits

[Andre McGrail](http://www.andremcgrail.com) - Design, Programming, Modeling, Textures, SFX

[Alex Best](https://big_ally.artstation.com) - Modeling, Textures

[Stintah](https://soundcloud.com/stintah) - Soundtrack

Special thanks to:

[Felipe Lira](https://github.com/phi-lira) - For Making Universal RP & LWRP

[Tim Cooper](https://github.com/stramit) - Assorted SRP code help

And thanks to many more who have helped with suggestions and feedback!

# Notes

*Make sure you clone the repo as downloading the zip will not contain the GitLFS files(all textures/meshes etc)
