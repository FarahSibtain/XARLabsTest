# XARLabsTest

This is a VR app created and tested on Meta Quest 2 using Unity 6000.0.36f1. It is compatible with newer Meta Quest devices and supports Unity 2023.3 or later.
These are the tools used to create the app:
- Unity 2023.3.0f1 or later
- Meta Quest 2 or later
- URP 17.2.3
- XR Interaction Toolkit 3.0.8
- XR Plugin Management 4.5.1
- OpenXR Plugin 1.14.3
- OpenAI 8.6.6 (For additonal AI features)

Implemented all the requirements mentioned in the test description that included:

- Procedural Mesh Creation of Object A as a sphere with a cone in front
- Secondary Object creation Object B as a sphere
- Both objects are animated using Lissajous Animation with different parameters
- Object A Rotates around Object B with some angular speed to keep facing it
- Color of object A changes based on the direction between Objects A & B
- Mesh Vertex Animation of Object A based on Perlin Noise

Bonus Part:
- Added support for Meta Quest 2 and later devices for VR integration
- Made sure that the animation stays positioned relative to the user in the front
- ObjectA is only attracted/grabbable by right hand/controller & ObjectB by left hand/controller

Additional Features:
- Added a AI feature that allows the user to ask questions as a Neurosurgeon to the AI Virtual Assistant
- User can ask AI to change visibility and angular speed of Object A. Mention something like "hide the object" or "change the angular speed to 100"

Setup Details:
- Clone the repository from GitHub: https://github.com/FarahSibtain/XARLabsTest.git
- Open the project in Unity 2023.3 or later.
- Hit Play in Unity Editor to test the app.

For AI features:
- You need an OpenAI API key to test the AI features. You can get it from https://platform.openai.com/signup
- Provide the API key in the OpenAIConfiguration file in foler .\Assets\Resources

For VR testing:
- connect your Meta Quest headset using Oculus Link or Air Link.
- Build and deploy the project using Build and Run.
Use the .apk file to test the app on your Meta Quest device from here: https://drive.google.com/file/d/1WbXsUCNigvWuZN3Aa1M-VDKVpDzP1dPE/view?usp=sharing

Demo Video:
- Demo with Controllers and AI Features: https://drive.google.com/file/d/1fVhTvFox_U8cRFv_2nmseDpZcDevcRDO/view?usp=sharing
(Note: My voice is not audible in the demo video where I'm instructing it to hide the object and change the angular speed of Object A)

- Demo with Hand Tracking: https://drive.google.com/file/d/19TVBBhIp6pvmDvIdXxJOM6T_2EqvYWHn/view?usp=sharing
