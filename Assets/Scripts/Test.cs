//This script adds force to a Rigidbody. The kind of force is determined by which buttons you click.

//Create a Sprite and attach a Rigidbody2D component to it
//Attach this script to the Sprite

using UnityEngine;
using UnityEngine.EventSystems;

public class AddingForce : MonoBehaviour
{
    //Use to switch between Force Modes
    enum ModeSwitching { Start, Impulse, Force };
    ModeSwitching m_ModeSwitching;

    //Use this to change the different kinds of force
    ForceMode2D m_ForceMode;
    //Start position of the RigidBody, use to reset
    Vector2 m_StartPosition;

    //Use to apply force to RigidBody
    Vector2 m_NewForce;

    //Use to manipulate the RigidBody of a GameObject
    Rigidbody2D m_Rigidbody;

    void Start()
    {
        //Fetch the RigidBody component attached to the GameObject
        m_Rigidbody = GetComponent<Rigidbody2D>();
        //Start at first mode (nothing happening yet)
        m_ModeSwitching = ModeSwitching.Start;

        //Initialising the force to use on the RigidBody in various ways
        m_NewForce = new Vector2(-5.0f, 1.0f);

        //This is the RigidBody's starting position
        m_StartPosition = m_Rigidbody.transform.position;
    }

    void Update()
    {
        //Switching modes depending on button presses
        switch (m_ModeSwitching)
        {
            //This is the starting mode which resets the GameObject
            case ModeSwitching.Start:

                //Reset to starting position of RigidBody
                m_Rigidbody.transform.position = m_StartPosition;
                //Reset the velocity of the RigidBody
                m_Rigidbody.linearVelocity = new Vector2(0f, 0f);
                break;

            //This is the Force Mode
            case ModeSwitching.Force:
                //Make the GameObject travel upwards
                m_NewForce = new Vector2(0, 1.0f);
                //Use Force mode as force on the RigidBody
                m_Rigidbody.AddForce(m_NewForce, ForceMode2D.Force);
                break;

            //This is the Impulse Mode
            case ModeSwitching.Impulse:
                //Make the GameObject travel upwards
                m_NewForce = new Vector2(0f, 1.0f);
                //Use Impulse mode as a force on the RigidBody
                m_Rigidbody.AddForce(m_NewForce, ForceMode2D.Impulse);
                break;
        }
    }

    //These are the Buttons for telling what Force to apply as well as resetting
    void OnGUI()
    {
        //If reset button pressed
        if (GUI.Button(new Rect(100, 0, 150, 30), "Reset"))
        {
            //Switch to start/reset case

            m_ModeSwitching = ModeSwitching.Start;
        }

        //Impulse button pressed
        if (GUI.Button(new Rect(100, 60, 150, 30), "Apply Impulse"))
        {
            //Switch to Impulse mode (apply impulse forces to GameObject)

            m_ModeSwitching = ModeSwitching.Impulse;
        }

        //Force Button Pressed
        if (GUI.Button(new Rect(100, 90, 150, 30), "Apply Force"))
        {
            //Switch to Force mode (apply force to GameObject)
            m_ModeSwitching = ModeSwitching.Force;
        }
    }
}

