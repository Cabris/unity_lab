using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Character/Rigidbody Pick-Up")]
public class RigidbodyPickUp : MonoBehaviour
{
    public string pickupButton = "Fire1"; //The name of the Pick Up button you are going to use.
    public bool togglePickUp = false; //Make picking up objects toggable.
    private bool objectIsToggled = false; //Check if the object is held //PRIVATE
    private float togg_time = 0.5f; //A short timer for when the object be allowed to press again //PRIVATE
    public GameObject playerCam; //Camera of player
    public float distance = 3f; //How far the object is being held away from you
    public float maxDistanceHeld = 4f; //How far the object can be held. If it goes past, it releases.
    public float maxDistanceGrab = 10f; //The maximum distance an object can be grabbed.

    private Ray playerAim; //Vector3 of main camera's direction //PRIVATE
    private GameObject objectHeld; // Object being held currently //PRIVATE
    private bool isObjectHeld; // is the object being held? //PRIVATE
    private bool objectCan; // Can the object the player is looking at be held? //PRIVATE
    private float timeHeld = 0.05f; // PRIVATE
    private float intTimeHeld; // PRIVATE
    private bool objectNullSet = false; // Makes a few sets and makes it disable once it picks up. // PRIVATE

    private GameObject[] pickableObjs; /*Objects that PlayerGun can pick up  //PRIVATE
    !LEAVE BLANK, TAG OBJECTS WITH "Pickable" TO ADD OBJECTS TO THIS LIST!*/
    public Shader pickableShader;
    public bool autoSetTransparentShader = true;

    public physicsSub physicsMenu = new physicsSub(); //Brings the Physic settings into the Inspector
    public crosshairSystem crosshairsSystem = new crosshairSystem(); //Bring the Crosshair System into the Inspector
    public audioSoundsSub audioSystem = new audioSoundsSub(); //Brings the audio menu into the Inspector
    public objectAlphaSub objectHoldingOpacity = new objectAlphaSub(); //Bring the Object Alpha system into the Inspector
    public throwingSystemMenu throwingSystem = new throwingSystemMenu(); //Bring the Throwing System into the Inspector
    public rotationSystemSub rotationSystem = new rotationSystemSub(); //Bring the Rotation System into the Inspector
    public objectZoomSub zoomSystem = new objectZoomSub(); //Brings the Object Zoom System into the Inspector
    public objectFreezing objectFreeze = new objectFreezing();

    //NOTE: There are more variables at the very bottom of the script.

    void Start()
    {
        //Set bools, objects, and floats to proper defaults.
        isObjectHeld = false;
        objectHeld = null;
        objectCan = false;
        intTimeHeld = timeHeld;
        //Screen.showCursor = false;
        //Screen.lockCursor = true;
        zoomSystem.intDistance = distance;
        zoomSystem.maxZoom = maxDistanceHeld - 0.7f;
        //pickableShader = Shader.Find("Transparent/Bumped Specular");
        if (rotationSystem.lockRotationToX && rotationSystem.lockRotationToY)
        {
            throw new System.Exception("[Rigidbody-Pickup] ERROR 1 Rotation System: Both 'X' and 'Y' rotations are locked, you cannot lock both at the same time.");
            throw new System.Exception("[Rigidbody-Pickup] ERROR 1 Rotation System: Please uncheck either X or Y in the rotation submenu in the inspector.");
        }
        if (physicsMenu.objectTurnsOnlyY && physicsMenu.objectFacesForward)
        {
            throw new System.Exception("[Rigidbody-Pickup] ERROR 2 Physics System: You cannot have both OnlyY and Forward checked. Only one should be selected.");
            throw new System.Exception("[Rigidbody-Pickup] ERROR 2 Physics System: Please uncheck either OnlyY or Forward in the Physics submenu in the inspector.");
        }
    }
    void Update()
    {
        //Finds all the objects with the tag "Pickable", and adds them to the GameObject list, 'pickableObjs'
        pickableObjs = GameObject.FindGameObjectsWithTag("Pickable");
        if (autoSetTransparentShader)
        {
            foreach (GameObject pickable in pickableObjs)
            {
                pickable.renderer.material.shader = pickableShader;
            }
        }

        //Crosshair Raycasting
        Ray playerAim = playerCam.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(playerAim, out hit, maxDistanceGrab - 0.8f))
        {
            objectCan = (hit.transform.tag == "Pickable");
        }
        else
        {
            objectCan = false;
        }

        //Check to see if the object held is deleted, if so, make it false.
        if (objectHeld == null && !objectNullSet)
        {
            isObjectHeld = false;
            ResetHeldTimer();
            objectNullSet = true;
            if (togglePickUp)
            {
                togg_time = 0.5f;
                objectIsToggled = false;
            }
            if (zoomSystem.enabled)
            {
                distance = zoomSystem.intDistance;
            }
        }

        //Check to see if throwing system is enabled, if an object is held, and if the throwing button is pressed
        if (isObjectHeld && throwingSystem.enabled && Input.GetButtonDown(throwingSystem.throwButton) && !throwingSystem.actionDone)
        {
            //Sets the object that is currently held to be the object to have force applied.
            throwingSystem.objectToThrow = objectHeld;
            //Sets a bool to continue with step 2 of the throwingSystem
            throwingSystem.aboutToThrow = true;
            //Drops the object so force can be applied
            isObjectHeld = false;

            if (togglePickUp)
            {
                togg_time = 0.5f;
                objectIsToggled = false;
            }
        }

        //Step 2 of throwingSystem
        if (throwingSystem.aboutToThrow)
        {
            //Reapplies gravity to the object
            throwingSystem.objectToThrow.rigidbody.useGravity = true;
            //Applies force to the objectt that was applied
            throwingSystem.objectToThrow.rigidbody.AddForce(playerAim.direction * throwingSystem.strength);
            //Set a bool to allow step 3 of throwingSystem
            throwingSystem.actionDone = true;
            //If audioSystem is also enabled, it will play the set audio file
            if (audioSystem.enabled)
            {
                audio.PlayOneShot(audioSystem.throwAudio);
            }
            ResetHeldTimer();
        }

        //Step 3 of throwingSystem
        if (throwingSystem.actionDone)
        {
            //Activates a timer for when you're allowed to pick up the object again.
            throwingSystem.thrownTimer = throwingSystem.thrownTimer - 1f * Time.deltaTime;
            //Sets the object that had forced applied to null for next object.
            throwingSystem.objectToThrow = null;
            //Disable Step 2
            throwingSystem.aboutToThrow = false;
        }

        //Throwing System timer
        if (throwingSystem.thrownTimer < 0)
        {
            throwingSystem.actionDone = false;
            throwingSystem.thrownTimer = 1f;
        }

        if (zoomSystem.enabled)
        {
            if (zoomSystem.useAxis)
            {
                if (Input.GetAxis(zoomSystem.zoomAxisButton) > 0 && isObjectHeld)
                {
                    distance = distance + Input.GetAxis(zoomSystem.zoomAxisButton);
                }
                else if (Input.GetAxis(zoomSystem.zoomAxisButton) < 0 && isObjectHeld)
                {
                    distance = distance + Input.GetAxis(zoomSystem.zoomAxisButton);
                }
            }
            else
            {
                if (Input.GetButton(zoomSystem.zoomInButton) && isObjectHeld)
                {
                    distance = distance + 0.05f;
                }
                else if (Input.GetButton(zoomSystem.zoomOutButton) && isObjectHeld)
                {
                    distance = distance - 0.05f;
                }
            }
        }

        if (distance < zoomSystem.minZoom)
        {
            distance = zoomSystem.minZoom + 0.1f;
        }
        else if (distance > zoomSystem.maxZoom)
        {
            distance = zoomSystem.maxZoom - 0.1f;
        }


        if (isObjectHeld && objectIsToggled)
        {
            holdObject();
            togg_time = togg_time - 1f * Time.deltaTime;
        }

        //Button toggles for Raycasting
        if (togglePickUp)
        {
            //Button toggles for Raycasting
            if (Input.GetButtonDown(pickupButton) && !Input.GetButton(throwingSystem.throwButton) && !isObjectHeld && !objectIsToggled && togg_time > 0.45f)
            {
                if (!isObjectHeld)
                /*If no object is held, try to pick up an object.
                  toggle so you have to press the button down*/
                {
                    tryPickObject();
                }
            }

            if (Input.GetButtonDown(pickupButton) && isObjectHeld && objectIsToggled && togg_time < 0.4)
            {
                isObjectHeld = false;
                objectHeld.rigidbody.useGravity = true;
                togg_time = 0.5f;
                objectHeld = null;
                objectIsToggled = false;
            }
        }
        //Non-Toggle
        else if (!togglePickUp)
        {
            //Button toggles for Raycasting
            if (Input.GetButton(pickupButton) && !Input.GetButton(throwingSystem.throwButton) && throwingSystem.thrownTimer >= 0.9)
            {
                if (!isObjectHeld)
                /*If no object is held, try to pick up an object.
                  works if you hold down the button as well.*/
                {
                    tryPickObject();
                }
                else if (isObjectHeld)
                {
                    holdObject();
                }
            }

            //If Pickup Button is up, reset object to original state
            if (Input.GetButtonUp(pickupButton) && isObjectHeld)
            {
                isObjectHeld = false;
                objectHeld.rigidbody.useGravity = true;
                objectHeld = null;
            }
        }

        //Object Rotation System
        if (rotationSystem.enabled && Input.GetButton(rotationSystem.rotateButton) && isObjectHeld)
        {
            for (int x = 0; x < rotationSystem.mouseScripts.Length; x++)
            {
                rotationSystem.mouseScripts[x].enabled = !rotationSystem.mouseScripts[x].enabled;
            }

            if (physicsMenu.objectFacesForward)
            {
                physicsMenu.objectRotated = true;
            }
            else if (physicsMenu.objectTurnsOnlyY)
            {
                physicsMenu.objectRotated = true;
            }
            if (rotationSystem.lockRotationToX && !rotationSystem.lockRotationToY)
            {
                objectHeld.transform.Rotate(new Vector3(0, -Input.GetAxis("Mouse X"), 0), Time.deltaTime * rotationSystem.rotationSpeed);
            }
            else if (rotationSystem.lockRotationToY && !rotationSystem.lockRotationToX)
            {
                objectHeld.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), 0, 0), Time.deltaTime * rotationSystem.rotationSpeed);
            }
            else
            {
                objectHeld.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0), Time.deltaTime * rotationSystem.rotationSpeed);
            }
        }
        else if (rotationSystem.enabled)
        {
            for (int x = 0; x < rotationSystem.mouseScripts.Length; x++)
            {
                rotationSystem.mouseScripts[x].enabled = true;
            }
        }
        if (physicsMenu.objectRotated && !isObjectHeld && physicsMenu.objectFacesForward)
        {
            physicsMenu.objectRotated = false;
        }
        if (physicsMenu.objectRotated && !isObjectHeld && physicsMenu.objectTurnsOnlyY)
        {
            physicsMenu.objectRotated = false;
        }
        
        //Object Freezing
        //Pressing the freeze button will freeze the object's rigidbody constraints. Picking it up again will let it go.
        if (objectFreeze.enabled)
        {
            if (Input.GetButtonDown(objectFreeze.freezeButton) && isObjectHeld)
            {
                objectFreeze.objectFrozen = true;
            }

            if (objectFreeze.objectFrozen && isObjectHeld)
            {
                objectHeld.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                isObjectHeld = false;
                objectHeld = null;
            }
            else if (!objectFreeze.objectFrozen && isObjectHeld)
            {
                objectHeld.rigidbody.constraints = RigidbodyConstraints.None;
            }
        }



        //Object Alpha System
        if (isObjectHeld && objectHoldingOpacity.enabled)
        {
            Color alpha = objectHeld.renderer.material.color;
            alpha.a = objectHoldingOpacity.transparency;
            objectHeld.renderer.material.color = alpha;
            objectHoldingOpacity.alphaObject = objectHeld;
            objectHoldingOpacity.alphaSet = true;
        }
        else if (!isObjectHeld && objectHoldingOpacity.alphaSet && objectHoldingOpacity.enabled)
        {
            Color alpha = objectHoldingOpacity.alphaObject.renderer.material.color;
            alpha.a = 1f;
            objectHoldingOpacity.alphaObject.renderer.material.color = alpha;
            objectHoldingOpacity.alphaObject = null;
            objectHoldingOpacity.alphaSet = false;
        }
    }

    //Will try to pick up the rigidbody in the 'pickableObjs' array.
    private void tryPickObject()
    {
        Ray playerAim = playerCam.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Physics.Raycast(playerAim, out hit);//Outputs the Raycast
        objectNullSet = false;

        foreach (GameObject pickable in pickableObjs)
        //For each 'Pickable', it will allow the object to be held depending on the collision of the object.
        {
            if (hit.collider != null)
            {
                if (pickable == hit.collider.gameObject && Vector3.Distance(hit.collider.gameObject.transform.position, playerCam.transform.position) < maxDistanceGrab)
                {
                    isObjectHeld = true; //If object is successfully held, turn on bool
                    objectHeld = pickable; //Makes the object that got hit by the raycast go into the gun's objectHeld
                    objectHeld.rigidbody.useGravity = false; //Disable gravity to fix a bug
                    if (audioSystem.enabled)
                    {
                        audio.PlayOneShot(audioSystem.pickedUpAudio);
                        audioSystem.letGoFired = false;
                    }
                    if (togglePickUp)
                    {
                        objectIsToggled = true;
                    }
                    if (objectFreeze.enabled)
                    {
                        objectFreeze.objectFrozen = false;
                    }
                }
            }
        }
    }

    private void holdObject()
    {
        Ray playerAim = playerCam.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        /*Finds the next position for the object held to move to, depending on the Camera's position
        ,direction, and distance the object is held between you two.*/
        Vector3 nextPos = playerCam.transform.position + playerAim.direction * distance;
        //Takes the current position of the object held
        Vector3 currPos = objectHeld.transform.position;
        timeHeld = timeHeld - 0.1f * Time.deltaTime;

        if (audioSystem.enabled)
        {
            audio.PlayOneShot(audioSystem.objectHeldAudio);
            audio.Play();
        }

        /*Checking the distance between the player and the object held.
         * If the distance exceeds the 'maxDistanceHeld', it will let the object go. This also
         * stops a bug that forces objects through walls if you move back too far with an object held
         * maxDistanceGrab is how far you are able to grab an object, if it exceeds the amount, it won't do anything
         */
        if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > maxDistanceGrab)
        {
            objectHeld.rigidbody.useGravity = true;
            isObjectHeld = false;
            objectHeld = null;
        }

        //If an object is held, apply the object's placement.
        else if (isObjectHeld)
        {
            if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > maxDistanceHeld && timeHeld < 0)
            {
                objectHeld.rigidbody.useGravity = true;
                isObjectHeld = false;
                objectHeld = null;
                ResetHeldTimer();
            }
            else
            {
                objectHeld.rigidbody.velocity = (nextPos - currPos) * 10;
                if (physicsMenu.objectFacesForward && !physicsMenu.objectRotated)
                {
                    //Rotation Forward
                    objectHeld.transform.eulerAngles = new Vector3(playerCam.transform.eulerAngles.x, playerCam.transform.eulerAngles.y, playerCam.transform.eulerAngles.z);
                }
                else if (physicsMenu.objectTurnsOnlyY && !physicsMenu.objectRotated)
                {
                    objectHeld.transform.eulerAngles = new Vector3(0, playerCam.transform.eulerAngles.y, 0);
                }
            }
        }
    }
    void OnGUI()
    {
        if (crosshairsSystem.enabled)
        {

            if (isObjectHeld) //Object Is Being Held Crosshair
            {
//                GUI.DrawTexture(new Rect(Screen.width / 2 - (crosshairsSystem.crosshairTextures[2].width / 2), Screen.height / 2 - (crosshairsSystem.crosshairTextures[2].height / 2),
//                                        crosshairsSystem.crosshairTextures[2].width,
//                                        crosshairsSystem.crosshairTextures[2].height),
//                                        crosshairsSystem.crosshairTextures[2]);
				crosshairsSystem.crosshair.index=2;
            }
            else if (objectCan) //Object Can Be Held Crosshair
            {
//                GUI.DrawTexture(new Rect(Screen.width / 2 - (crosshairsSystem.crosshairTextures[1].width / 2), Screen.height / 2 - (crosshairsSystem.crosshairTextures[1].height / 2),
//                                        crosshairsSystem.crosshairTextures[1].width,
//                                        crosshairsSystem.crosshairTextures[1].height),
//                                        crosshairsSystem.crosshairTextures[1]);
				crosshairsSystem.crosshair.index=1;
            }
            else if (!isObjectHeld && !objectCan) //Default Crosshair
            {
                if (crosshairsSystem.crosshairTextures[0] == null)
                {
                    throw new System.Exception("[Rigidbody-Pickup] ERROR 3 Crosshair System: Crosshair textures is null. Please put the size to 3 and put 3 textures inside.");
                }
                else
                {
//                    GUI.DrawTexture(new Rect(Screen.width / 2 - (crosshairsSystem.crosshairTextures[0].width / 2), Screen.height / 2 - (crosshairsSystem.crosshairTextures[0].height / 2),
//                                            crosshairsSystem.crosshairTextures[0].width,
//                                            crosshairsSystem.crosshairTextures[0].height),
//                                            crosshairsSystem.crosshairTextures[0]);
					crosshairsSystem.crosshair.index=0;
                }
            }
        }
    }

    void ResetHeldTimer()
    {
        timeHeld = intTimeHeld;
    }
}

[System.Serializable]
public class physicsSub
{
    public bool objectFacesForward = false;

    public bool objectTurnsOnlyY = true;

    [System.NonSerialized]
    public bool objectRotated = false;
}

[System.Serializable]
public class throwingSystemMenu //Throwing System
{
    public bool enabled = false;

    public string throwButton = "Fire2";

    public float strength = 250f;

    [System.NonSerialized]
    public bool aboutToThrow = false;

    [System.NonSerialized]
    public bool actionDone = false;

    [System.NonSerialized]
    public GameObject objectToThrow = null;

    [System.NonSerialized]
    public float thrownTimer = 1f;
}

[System.Serializable]
public class crosshairSystem //Crosshair System - You are no longer required to just remove the code, just disable it from the inspector!
{
    public bool enabled = true;
    public Texture2D[] crosshairTextures; //Array of textures to use for the crosshair
    //0 = default | 1 = Object can be held | 2 = Object is being held currently
	public Crosshair crosshair;
}

[System.Serializable]
public class rotationSystemSub
{
    public bool enabled = false;
    public string rotateButton = "Rotate"; //A Input name from the Input Manager.

    public float rotationSpeed = 3; //Rotation speed of the Rigidbody

    public bool lockRotationToX = false; // Do not have all set to true, or else it just breaks.
    public bool lockRotationToY = false;

    //Change "MouseLook" to your own Mouse Script name. The one currently used is from the default FPS controller package.
    public MouseLook[] mouseScripts;

    [System.NonSerialized]
    public float rotY = 0F;
}

[System.Serializable]
public class objectAlphaSub
{
    public bool enabled = true;
    public float transparency = 0.5f;

    [System.NonSerialized]
    public GameObject alphaObject;

    [System.NonSerialized]
    public bool alphaSet = false;
}

[System.Serializable]
public class audioSoundsSub
{
    public bool enabled = true;

    public AudioClip pickedUpAudio;
    public AudioClip objectHeldAudio;
    public AudioClip throwAudio;


    [System.NonSerialized]
    public bool letGoFired = false;
}

[System.Serializable]
public class objectZoomSub
{
    public bool enabled = false;

    public string zoomInButton;
    public string zoomOutButton;

    public bool useAxis = true; //if true, it will use the Axis Button for both zooming in and out. If alse, will use the two buttons instead
    public string zoomAxisButton = "Mouse ScrollWheel";

    public float minZoom = 1.5f; //Set the minimum amount for how close the object can be held. Will use maxDistanceHeld for maximum distance your object can be held.

    [System.NonSerialized]
    public float maxZoom; //Leave default to allow maxDistanceHeld to use the variable.

    [System.NonSerialized]
    public float intDistance;
}

[System.Serializable]
public class objectFreezing
{
    public bool enabled = false;

    public string freezeButton = "Freeze";

    [System.NonSerialized]
    public bool objectFrozen = false;
}