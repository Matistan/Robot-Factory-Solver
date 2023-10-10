using UnityEngine;

public class Blocks : MonoBehaviour {
    bool entered = false;
    readonly float metallic = 0.2f;
    public GameManager gameManager;

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !entered)
        {
            Material material = GetComponent<MeshRenderer>().material;
            material.SetFloat("_Metallic", 0);
            if (gameObject == GameManager.selectedObject)
            {
                GameManager.selected = false;
            }
        }
        if(gameObject == GameManager.selectedObject && GameManager.selected)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.Rotate(0, 0, -90);
                Vector3 v = GameManager.selectedObject.transform.position;
                GameManager.r[(int) (v.z / -1.41f+ 1), (int) (v.x / 1.41f + 1)] += 3;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                transform.Rotate(0, 0, 90);
                Vector3 v = GameManager.selectedObject.transform.position;
                GameManager.r[(int)(v.z / -1.41f + 1), (int)(v.x / 1.41f + 1)] += 1;
            }
            else if (Input.GetKeyDown(KeyCode.Minus))
            {
                KeyDownMaterial(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                KeyDownMaterial(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                KeyDownMaterial(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                KeyDownMaterial(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                KeyDownMaterial(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                KeyDownMaterial(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                KeyDownMaterial(6);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                KeyDownMaterial(7);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                KeyDownMaterial(8);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                KeyDownMaterial(9);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                //if (transform.position.z != 1.41f)
                //{
                //    gameObject.transform.position += new Vector3(0, 0, 1.41f);
                //}
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                //if (transform.position.x != -1.41f)
                //{
                    //gameObject.transform.position += new Vector3(-1.41f, 0, 0);
                //}
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                //if (transform.position.z != -1.41f)
                //{
                //    gameObject.transform.position += new Vector3(0, 0, -1.41f);
                //}
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                //if (transform.position.x != 1.41f)
                //{
                //    gameObject.transform.position += new Vector3(1.41f, 0, 0);
                //}
            }
        }
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    if (GameManager.selected)
        //    {
        //        if (GameManager.selectedObject.transform.position.z == transform.position.z - 1.41f && GameManager.selectedObject.transform.position.x == transform.position.x)
        //        {
        //            //Debug.Log(GameManager.selectedObject.transform.position.z + " " + transform.position.z);
        //            gameObject.transform.position += new Vector3(0, 0, -1.41f);
        //        }
        //        else if (GameManager.selectedObject == gameObject && transform.position.z != 1.41f)
        //        {
        //            gameObject.transform.position += new Vector3(0, 0, 1.41f);
        //        }
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.A))
        //{
        //    if (GameManager.selected)
        //    {
        //        if (GameManager.selectedObject.transform.position.x == transform.position.x + 1.41f && GameManager.selectedObject.transform.position.z == transform.position.z)
        //        {
        //            gameObject.transform.position += new Vector3(1.41f, 0, 0);
        //        }
        //        else if (GameManager.selectedObject == gameObject && transform.position.x != -1.41f)
        //        {
        //            gameObject.transform.position += new Vector3(-1.41f, 0, 0);
        //        }
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.S))
        //{
        //    if (GameManager.selected)
        //    {
        //        if (GameManager.selectedObject.transform.position.z == transform.position.z + 1.41f && GameManager.selectedObject.transform.position.x == transform.position.x)
        //        {
        //            gameObject.transform.position += new Vector3(0, 0, 1.41f);
        //        }
        //        else if (GameManager.selectedObject == gameObject && transform.position.z != -1.41f)
        //        {
        //            gameObject.transform.position += new Vector3(0, 0, -1.41f);
        //        }
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.D))
        //{
        //    if (GameManager.selected)
        //    {
        //        if (GameManager.selectedObject.transform.position.x == transform.position.x - 1.41f && GameManager.selectedObject.transform.position.z == transform.position.z)
        //        {
        //            gameObject.transform.position += new Vector3(-1.41f, 0, 0);
        //        }
        //        else if (GameManager.selectedObject == gameObject && transform.position.x != 1.41f)
        //        {
        //            gameObject.transform.position += new Vector3(1.41f, 0, 0);
        //        }
        //    }
        //}

    }

    private void OnMouseEnter()
    {
        entered = true;
        Material material = GetComponent<MeshRenderer>().material;
        material.SetFloat("_Metallic", (float) (material.GetFloat("_Metallic") + metallic));
    }

    private void OnMouseExit()
    {
        entered = false;
        Material material = GetComponent<MeshRenderer>().material;
        material.SetFloat("_Metallic", (float)(material.GetFloat("_Metallic") - metallic));
    }

    private void OnMouseDown()
    {
        Material material = GetComponent<MeshRenderer>().material;
        if (material.GetFloat("_Metallic") == metallic)
        {
            GameManager.selected = true;
            material.SetFloat("_Metallic", (float)(material.GetFloat("_Metallic") + metallic * 2));
            GameManager.selectedObject = gameObject;
        }
    }
    private void KeyDownMaterial(int index)
    {
        GameManager.ButtonClicked(gameManager.Materials[index]);
        Material material = gameObject.GetComponent<MeshRenderer>().material;
        material.SetFloat("_Metallic", metallic * 2);
        if (entered)
        {
            material.SetFloat("_Metallic", metallic * 3);
        }
    }
}