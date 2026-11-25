using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class WeaponManager : MonoBehaviour
{
    private float startYScale;
    public float weaponHeight;
    public int SelectedWeapon = 0;

    public MovementAlternate movementalternate;


    void Start()
    {
        SelectWeapon();
        startYScale = transform.localScale.y;
    }

    void Update()
    {

        if (movementalternate._croch != 0)
        {
            //transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        if (movementalternate._croch != 1)
        {
            //transform.localScale = new Vector3(transform.localScale.x, weaponHeight, transform.localScale.z);
        }

        int previousSelectedWeapon = SelectedWeapon;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (SelectedWeapon >= transform.childCount - 1)
                SelectedWeapon = 0;
            else
                SelectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (SelectedWeapon <= 0)
                SelectedWeapon = transform.childCount - 1;
            else
                SelectedWeapon--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)&& transform.childCount>=2)
        {
            SelectedWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            SelectedWeapon = 2;
        }

        if (previousSelectedWeapon != SelectedWeapon)
        {
            SelectWeapon();
        }
    }
    void SelectWeapon()
    {
        int i = 0;
        foreach ( Transform weapon in transform)
        {
            if (i == SelectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
