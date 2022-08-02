# Harumachi Cup Omega Client

## Info
This project uses **2020.3.16f1**, though you can most likely downgrade it.

This project works by streaming the camera into a render texture, then using [UnityCapture](https://github.com/schellingb/UnityCapture) to stream it into a virtual webcam, which can then be used on Discord or in our case OBS. We do this so the window can be resized to an ultra small resolution, to take up as little space on a streamer's desktop as possible, whilst still sending a full 1080p to OBS.

We use [uWindowCapture](https://github.com/hecomi/uWindowCapture) to capture the two osu! windows (in a 1V1). This is provided alongside UnityCapture in the repository.


## Setup
To use this, you'll need to install drivers for UnityCapture: https://github.com/schellingb/UnityCapture

After that, you'll need to make sure you're running Unity 2020.3.16f1. If you are not running this version, it's likely the project will not work properly.

Add virtual camera 1 to OBS, and set the resolution to 1920x1080. If you do not explicitly set this, it will error out with invalid resolution.

You can then play the project in Unity, or open a build, and it should stream to OBS. We will not help you if you cannot figure this out.


## Usage

### Paths

This client has alot of paths hardcoded, such as for the window mover, ipc/chat, and more. It's recommended you look through every C# file and change things to your respective locations.

### Presets

We have provided two preset "presets" files. We recommend you use these to set the values for each round, as it's much more time consuming otherwise.

### Mappools

For mappools, you can see how we did it [here](https://docs.google.com/spreadsheets/d/1VrnKZF5Cq4O33ME8vm_s-kiWVsG0VCHuxBUIFEfFo5w/). You must use this exact layout, else the code will not work. You can of course, edit the code.

### Basic Usage

After you start the project, begin by putting the two User IDs for each user into the boxes, then clicking Update User Info. Then select your preset by clicking it in the Presets list. At this point you're practically good to go.

You'll need to go into the options menu of both osu! clients, and set them to the smallest Widescreen resolution listed. There is code provided in the WindowMover folder to get you started moving the osu! clients to a seperate window so they do not take up space nor collide with eachother.

### Mappool Screen

You can start by clicking the Red or Blue button, to decide who picks first. Then, assuming the mappool is already loaded, put in the name of the map (hd1, dt1, fm2, etc), then click the Select button. This will select it in the mappool screen (assuming you've clicked the "mappool screen" button), and then switch the Blue and Red buttons around.

When switching from play screen and back, it will grey out any existing selected ones, similar to lazer's tournament client. The greyout and select graphics aren't the best, I'd recommend replacing them.

### Winning

Click the declare win button to decide who wins. The animation doesn't last forever, and will repeat after a minute or two, so recommended to click "hide winner screen" shortly after


## Updates & Support

This code will not be supported, nor will you get any support or help using it. If you decide to use this code for your own projects, it will be your responsibiltiy to make it work for your use case.

We will not fix bugs, add features, or update this to any capacity. It's job is done for us.

Good luck!