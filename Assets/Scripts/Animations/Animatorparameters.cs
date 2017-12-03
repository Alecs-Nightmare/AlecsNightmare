using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animatorparameters : MonoBehaviour {

    string saltar = "saltar";
    string correr = "correr";
    string enTierra = "en tierra";
    string pared = "pared";
    string paraguas = "paraguas";
    string planear = "planear";

    public string getSaltar() {
        return saltar;
    }
    public string getCorrer()
    {
        return correr;
    }
    public string getEnTierra()
    {
        return enTierra;
    }
    public string getPared()
    {
        return pared;
    }
    public string getParaguas()
    {
        return paraguas;
    }
    public string getPlanear()
    {
        return planear;
    }

}
