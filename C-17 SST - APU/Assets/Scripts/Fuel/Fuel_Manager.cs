using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel_Manager : MonoBehaviour
{
    // Debugging
    public bool Debug_Power_Avail = true;

    public Panel_Electric elec_manager;

    public Pwr_Tied_Light ind_Tank3_Pump_Fwd;
    public Pwr_Tied_Light ind_Tank3_Pump_Aft;

    bool sw_pump3_fwd = false;
    bool sw_pump3_aft = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateIndicators();
    }

    void UpdateIndicators()
    {
        if (elec_manager.sw_APU == 2 || elec_manager.sw_GPU == 1 || Debug_Power_Avail == true)
        {
            if (sw_pump3_fwd) ind_Tank3_Pump_Fwd.SetMaterial(1);
            else ind_Tank3_Pump_Fwd.SetMaterial(0);

            if (sw_pump3_aft) ind_Tank3_Pump_Aft.SetMaterial(1);
            else ind_Tank3_Pump_Aft.SetMaterial(0);
        }
        else
        {
            ind_Tank3_Pump_Fwd.SetMaterial(0);
            ind_Tank3_Pump_Aft.SetMaterial(0);
        }

        
    }

    public void ToggleTank3Fwd()
    {
        sw_pump3_fwd = !sw_pump3_fwd;
    }
    public void ToggleTank3Aft()
    {
        sw_pump3_aft = !sw_pump3_aft;
    }
}
