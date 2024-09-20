using UnityEngine;
using System.Collections;

public class Pinche : SceneObject {

    public PincheDientes dientes1;
    public PincheDientes dientes2;
    public PincheDientes dientes3;
    public PincheDientes dientes4;
    public PincheDientes dientes5;

    public Mortal MortalTrap;

    private int idActive;
    private bool right;

    public override void OnRestart(Vector3 pos)
    {
        idActive = 0;
        right = true;
        base.OnRestart(pos);
        Change();
    }
    void Change()
    {
        activeNext();
        Invoke("Change", 0.6f);
    }
    void activeNext()
    {
        //dientes1.setOff();
        //dientes2.setOff();
        //dientes3.setOff();
        //dientes4.setOff();
        //dientes5.setOff();

        PincheDientes activeDientes;
        if (idActive == 4)
            right = false;
        else if (idActive == 0)
            right = true;

        if (right)
            idActive++;
        else
            idActive--;
        switch (idActive)
        {
            case 0: activeDientes = dientes1; break;
            case 1: activeDientes = dientes2; break;
            case 2: activeDientes = dientes3; break;
            case 3: activeDientes = dientes4; break;
            default: activeDientes = dientes5; break;
        }
        activeDientes.setOn();

        MortalTrap.transform.localPosition = new Vector3(activeDientes.transform.localPosition.x, MortalTrap.transform.localPosition.y, 0);
    }

    public override void OnPool()
    {
        base.OnPool();
    }
}
