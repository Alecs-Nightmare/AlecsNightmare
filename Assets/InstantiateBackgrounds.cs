using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InstantiateBackgrounds : MonoBehaviour {
    private float sizeBackgroundFront = 34.15f;

    public GameObject frontBackground;
    public GameObject unionObject;


    public GameObject[] FrontBackgrounds;
    public GameObject[] MiddleBackgrounds;
    public GameObject[] BackBackgrounds;



	// Use this for initialization
	void Start () {

        FrontBackgrounds = new GameObject[3];
        setInitFrontBackgrounds();
        

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void changeFrontBackgrounds()
    {


        GameObject frontBackground = FrontBackgrounds[0];
        Vector3 position = frontBackground.transform.position;
        //trasladamos el de mas a la izquierda al tope de la derecha (position.x += 2*size

        FrontBackgrounds[0].transform.position += new Vector3(2 * sizeBackgroundFront, 0, 0);
        unionObject.transform.position += new Vector3(sizeBackgroundFront, 0, 0);


        //hacemos las asignaciones necesarias
        FrontBackgrounds[2] = FrontBackgrounds[0];
        FrontBackgrounds[1] = FrontBackgrounds[2];
        FrontBackgrounds[0] = FrontBackgrounds[1];

       
        





    }

    public void setInitFrontBackgrounds()
    {

        for (int i = 0; i < FrontBackgrounds.Length;i++)
        {
            FrontBackgrounds[i] = frontBackground;
            //lo instanciamos uno seguido de otro
            Instantiate(FrontBackgrounds[i], new Vector3(20 + i * sizeBackgroundFront, 0, 0), FrontBackgrounds[0].transform.rotation);

        }
        Instantiate(unionObject, new Vector3(FrontBackgrounds[1].transform.position.x + sizeBackgroundFront / 2, 0,0), unionObject.transform.rotation);

    }
}
