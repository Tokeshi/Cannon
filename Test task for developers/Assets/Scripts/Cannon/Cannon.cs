using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private Vector3 direction;
    public float moveSpeed;
    private Rigidbody rb;
    public float rotationSpeed;
    public GameObject [] cannonParts;
	private Vector3 mousePosition;
	//управление курсором запуск луча
	private Ray ray;
	private RaycastHit hit;
	//замена текстур курсора
	private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;
    public Texture2D cursorTextureGreen, cursorTextureRed;
    //стрельба
    public Transform SpawnTransform; //Позиция спавна снаряда
    public float AngleInDegrees; // Угол пушки
    private float g = Physics.gravity.y; // ускорение свободного падения
    public GameObject Bullet; //префаб снаряда
    public bool canShoot = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();  
    }

    void Update () 
    {
        SpawnTransform.localEulerAngles = new Vector3(-AngleInDegrees, 0f, 0f); //угол наклона орудия
        CreateRay(); 
        Shooting();  
    }

    void FixedUpdate()
    {
    	MoveCannon();    
    }
    //Движение самоходки
    void MoveCannon ()																		
    {    
    	float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
		float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
		transform.rotation *= Quaternion.Euler(0f, rotation, 0f);
		rb.MovePosition(transform.position + transform.forward * move);   
	}
	void Shooting()
	{
		if (Input.GetMouseButtonDown(1)) 
        {
        	if(canShoot)
        	{
        		Shot();	
        	}    
        }
	}
	//Выстрел
	void Shot() 
	{
		//Вычисление направления выстрела
		Vector3 posToAttack = hit.point;
		Vector3 fromTo = hit.point - transform.position;
        Vector3 fromToXZ = new Vector3(fromTo.x, 0f, fromTo.z);
        //поворот "башни" в сторону выстрела
        cannonParts[1].transform.rotation = Quaternion.LookRotation(fromToXZ);
        //перевод в переменные для уравнения для вычисления падения тела, брошенного под углом к горизонту
        float x = fromToXZ.magnitude;
        float y = fromTo.y;
        //перевод угла из градусов в радианы
        float AngleInRadians = AngleInDegrees * Mathf.PI / 180;
        //Вычисление скорости снаряда
        float v2 = (g * x * x) / (2 * (y - Mathf.Tan(AngleInRadians) * x) * Mathf.Pow(Mathf.Cos(AngleInRadians), 2));
        float speedBullet = Mathf.Sqrt(Mathf.Abs(v2));
        //Создание снаряда
        GameObject newBullet = Instantiate(Bullet, SpawnTransform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().velocity = SpawnTransform.forward * speedBullet;
        
    }
    //проецирование луча на игровую плоскость
	void CreateRay()
	{
    	ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {   
        	int layer_mask = LayerMask.GetMask("Offside");
        	if(hit.transform.tag != "offside")
        	{
	     		Cursor.SetCursor(cursorTextureGreen, hotSpot, cursorMode);
	     		canShoot = true;
        	}
        	if(hit.transform.tag == "offside")
        	{
         		Cursor.SetCursor(cursorTextureRed, hotSpot, cursorMode);
         		canShoot = false;              
        	}  
        } 
	}
}	
