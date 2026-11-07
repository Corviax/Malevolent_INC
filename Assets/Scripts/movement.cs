using UnityEngine;

public class movement : MonoBehaviour
{
    private void Start()
    {
        //Posicion Inicial
        transform.position = new Vector3(0, 1, 0);

    }
    public float speed = 3.5f;

    void Update()
    {
        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");
        //float flyInput = Input.GetAxis("Jump");

        transform.Translate(Vector3.right * horInput * speed * Time.deltaTime);
        transform.Translate(Vector3.forward * verInput * speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verInput * speed * Time.deltaTime);

    }
}
