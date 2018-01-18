using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InstantiateBackgrounds : MonoBehaviour {
    private float sizeBackgroundFront = 100f;

    public GameObject frontBackground;
    public GameObject middleBackground;
    public GameObject backBackground;

    public GameObject unionFrontObject;
    public GameObject unionMiddleObject;
    public GameObject unionBackObject;


    public GameObject[] FrontBackgrounds;
    public GameObject[] MiddleBackgrounds;
    public GameObject[] BackBackgrounds;


    private GameObject unionFrontObjectAux;
    private GameObject unionMiddleObjectAux;
    private GameObject unionBackObjectAux;

    private Transform[] children;

    public int numCopies = 3;
    // Use this for initialization


    private void Awake()
    {
        children = GetComponentsInChildren<Transform>();
        
    }

    void Start () {

        FrontBackgrounds = new GameObject[numCopies];
        MiddleBackgrounds = new GameObject[numCopies];
        BackBackgrounds = new GameObject[numCopies];

        

        setInitBackgrounds();
        
        if (numCopies >= 3)
        {
            numCopies = 3;
        }
        this.gameObject.transform.position = new Vector3(this.transform.position.x + 60f, this.transform.position.y + 40, this.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void changeBackgrounds(string background)
    {
        if (background == "frontObject")
        {
            Debug.Log("we are on changeFrontBackgrounds()");


            //trasladamos el de mas a la izquierda al tope de la derecha (position.x += 2*size

            FrontBackgrounds[0].transform.position += new Vector3(3 * sizeBackgroundFront, 0, 0);
            Debug.Log("lo movemos");
            unionFrontObjectAux.transform.position += new Vector3(sizeBackgroundFront, 0, 0);


            //hacemos las asignaciones necesarias
            GameObject save0 = FrontBackgrounds[0];
            GameObject save1 = FrontBackgrounds[1];
            GameObject save2 = FrontBackgrounds[2];

            FrontBackgrounds[0] = save1;
            FrontBackgrounds[1] = save2;
            FrontBackgrounds[2] = save0;

        }
        
        if (background == "middleObject")
        {
            Debug.Log("we are on changeFrontBackgrounds()");


            //trasladamos el de mas a la izquierda al tope de la derecha (position.x += 2*size

            MiddleBackgrounds[0].transform.position += new Vector3(3 * sizeBackgroundFront, 0, 0);
            unionMiddleObjectAux.transform.position += new Vector3(sizeBackgroundFront, 0, 0);


            //hacemos las asignaciones necesarias
            GameObject save0 = MiddleBackgrounds[0];
            GameObject save1 = MiddleBackgrounds[1];
            GameObject save2 = MiddleBackgrounds[2];

            MiddleBackgrounds[0] = save1;
            MiddleBackgrounds[1] = save2;
            MiddleBackgrounds[2] = save0;


        }
        if (background == "backObject")
        {
            Debug.Log("we are on changeFrontBackgrounds()");


            //trasladamos el de mas a la izquierda al tope de la derecha (position.x += 2*size

            BackBackgrounds[0].transform.position += new Vector3(3 * sizeBackgroundFront, 0, 0);
            unionBackObjectAux.transform.position += new Vector3(sizeBackgroundFront, 0, 0);


            //hacemos las asignaciones necesarias
            GameObject save0 = BackBackgrounds[0];
            GameObject save1 = BackBackgrounds[1];
            GameObject save2 = BackBackgrounds[2];

            BackBackgrounds[0] = save1;
            BackBackgrounds[1] = save2;
            BackBackgrounds[2] = save0;

        }

        

        





    }

    public void setInitBackgrounds()
    {
        

        for (int i = 0; i < numCopies;i++)
        {
            /*
            GameObject aux = frontBackground;
            //lo instanciamos uno seguido de otro
            Instantiate(aux, new Vector3(20 + i * sizeBackgroundFront, -3, 0), aux.transform.rotation);
            FrontBackgrounds[i] = aux;
            */
            FrontBackgrounds[i] = Instantiate(frontBackground, new Vector3(20 + i * sizeBackgroundFront, Random.Range(10,20), 0), frontBackground.transform.rotation, children[1]);
            MiddleBackgrounds[i] = Instantiate(middleBackground, new Vector3(30 + i * sizeBackgroundFront, 0, 0), middleBackground.transform.rotation, children[2]);
            BackBackgrounds[i] = Instantiate(backBackground, new Vector3(40 + i * sizeBackgroundFront, 4, 0), backBackground.transform.rotation, children[3]);

        }
        

        unionFrontObjectAux = Instantiate(unionFrontObject, new Vector3(FrontBackgrounds[1].transform.position.x + sizeBackgroundFront / 2, -3,0), unionFrontObject.transform.rotation, children[1]);
        unionMiddleObjectAux = Instantiate(unionMiddleObject, new Vector3(MiddleBackgrounds[1].transform.position.x + sizeBackgroundFront / 2, -3, 0), unionMiddleObject.transform.rotation, children[2]);
        unionBackObjectAux = Instantiate(unionBackObject, new Vector3(BackBackgrounds[1].transform.position.x + sizeBackgroundFront / 2, -3, 0), unionBackObject.transform.rotation, children[3]);


    }
}
