using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public GameObject [] effects; 
    

    private void Start()
    {
    	CreateSmoke();
    }

    private void FixedUpdate()
    {
    	CreateTracer();
    }
    //эффект взрыва при соприкосновении с поверхностью земли
    private void CreateExplosion()
    {
    	Vector3 endPosition = transform.position;
		GameObject explosion = Instantiate(effects[1], endPosition, Quaternion.identity);
		Destroy(explosion, 1.5f);
    }
    //эффект дыма при выстреле
    private void CreateSmoke()
    {
    	Vector3 startPosition = transform.position;
    	GameObject smoke = Instantiate(effects[0], startPosition, Quaternion.identity);
    	Destroy(smoke, 1.5f);
    }
    //создание трассы снаряда
    private void CreateTracer()
    {
    	Vector3 positionTracer = transform.position;
    	GameObject tracer = Instantiate(effects[2], positionTracer, Quaternion.identity);
    	Destroy(tracer, 1.5f);
    }

    private void OnCollisionEnter(Collision coll) 
	{
		if (coll.gameObject.tag == "Terrian") 
	    {	
	    	CreateExplosion();
		    Destroy(gameObject, 0.2f);
		}    
	} 
    
}
