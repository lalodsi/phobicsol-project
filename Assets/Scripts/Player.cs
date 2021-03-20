using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Touch dedo1; //Pantalla Táctil
    private float tiempoDeToque;
    private float timeElapsed = 0.0f; //Tiempo temblando

    public Player()
    {
        //Constructor
    }
    public Quaternion leerGiroscopio()
    {
        /*
         * Lee el cambio en la rotación del giroscopio y lo devuelve de forma que unity lo pueda leer.
         * Servirá para rotar al personaje
         */
        Quaternion phoneAngles = new Quaternion();
        phoneAngles.eulerAngles = Input.gyro.rotationRate;
        return GyroToUnity(phoneAngles);
    }
    public static Quaternion GyroToUnity(Quaternion q)
    {
        /*
         * Corrije la diferencia del plano cartesiano entre unity y el celular
         */
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
    public void girarCamaraConLaMirada(GameObject camara)
    {
        /*
         * Lee la información del giroscopio del celular y actualiza la información de la cámara.
         * Sólo funciona en celulares que contengan giroscopio.
         */
        camara.transform.rotation *= leerGiroscopio();
    }
    public void interactuarConLaMirada(GameObject camara)
    {
        /*
         * Manda un rayo hacia donde mira el usuario. Servirá para moverse por el escenario utilizando la mirada
         */
        RaycastHit toque;
        Ray rayo = new Ray(camara.transform.position, camara.transform.forward);
        Debug.DrawRay(rayo.origin, 100 * rayo.direction, Color.yellow);
    }
    private float gestoUnDedoLateral()
    {
        /*
         * Algoritmo para mover la vista del personaje moviendo la pantalla táctil
         * La función devuelve un valor flotante correspondiente al movimiento horizontal
         */
        if (Input.touchCount == 1)
        {
            const float COMPLEMENTO_PARA_Y = .5f;
            const float ATENUAR_MOVIMIENTO = .1f;
            dedo1 = Input.GetTouch(0);
            if (Mathf.Abs(dedo1.deltaPosition.x) > (Mathf.Abs(dedo1.deltaPosition.y) + COMPLEMENTO_PARA_Y))
            {
                if (Mathf.Abs(dedo1.deltaPosition.x) < 5) return 0;
                else return dedo1.deltaPosition.x * ATENUAR_MOVIMIENTO;
            }
            else
            {
                return 0f;
            }
        }
        return 0f;
    }
    public void girarCamaraLateral(GameObject camara)
    {
        /*
         * Gira la cámara hacia los lados cuando se arrastra un dedo de forma lateral.
         * Funciona en celulares sin giroscopio
         */
        camara.transform.rotation *= Quaternion.Euler(0f, gestoUnDedoLateral(), 0f);
    }
    public void obtenerNombreDelObjetoTocado(Camera vision)
    {
        /* No terminada
         * El propósito de esta función es tener claro a dónde da click el jugador
         */
        if (huboClick() && Input.touchCount == 0)
        {
            RaycastHit toque;
            Ray rayo = vision.ScreenPointToRay(new Vector3(dedo1.position.x, dedo1.position.y, 0f));
            Debug.DrawRay(rayo.origin, 100 * rayo.direction, Color.green);
            if (Physics.Raycast(rayo, out toque))
            {

                //TODO: Probar esta función
                //print(toque.collider.name);
            }
            else
            {
                
            }
        }
        medirTiempoDeToque();
    }
    private bool huboClick()
    {
        /*
         * Determina si el usuario toco un objeto de forma intencional o si
         * tocó la pantalla táctil con un propósito diferente al de seleccionar algo
         */
        const float TIEMPO_MAXIMO_DE_UN_TOQUE_NORMAL = 0.3f; //Tiempo en tocar
        const float DISTANCIA_MAXIMA_DE_MOVIMIENTO_DE_UN_TOQUE = 0.5f; //A veces se mueve tantito el dedo

        return tiempoDeToque < TIEMPO_MAXIMO_DE_UN_TOQUE_NORMAL &&
            tiempoDeToque != 0f &&
            Mathf.Abs(dedo1.deltaPosition.x) < DISTANCIA_MAXIMA_DE_MOVIMIENTO_DE_UN_TOQUE &&
            Mathf.Abs(dedo1.deltaPosition.y) < DISTANCIA_MAXIMA_DE_MOVIMIENTO_DE_UN_TOQUE;
    }
    private void medirTiempoDeToque()
    {
        /*
         * Codigo para complementar la función de huboClick()
         * Mide el tiempo que una persona mantiene un dedo en la pantalla
         */
        if (Input.touchCount == 1)
        {
            tiempoDeToque += Time.deltaTime;
        }
        else
        {
            tiempoDeToque = 0f;
        }
    }
    public void shakeObject(float duration, float magnitude, GameObject objectToShake, float atenuacionSismo)
    {
        /*
         * Algoritmo para crear el efecto de terremoto en el escenario
         * Se agregó la función de atenuar el sismo con el tiempo
         */
        Vector3 originalPos = objectToShake.transform.position;
        
        float x = Random.Range(-atenuacionSismo, atenuacionSismo) * magnitude;
        float y = Random.Range(-atenuacionSismo, atenuacionSismo) * magnitude;
        float z = Random.Range(-atenuacionSismo, atenuacionSismo) * magnitude;

        timeElapsed += Time.deltaTime;

        if (timeElapsed < duration)
        {
            objectToShake.transform.position += new Vector3(x, y, z);
            objectToShake.transform.rotation = Quaternion.EulerAngles(x*2f, 0f, z*2f);
        }
    }
}