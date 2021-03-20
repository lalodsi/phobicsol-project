using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Camara;
    public GameObject Elemento;
    public Camera vision;
    float tiempo = 0f;
    public Text contadorReversa;
    public AudioSource efectoTerremoto;
    public AudioSource efectoPlatos;
    public AudioSource efectoCosas;
    public AudioSource alertaSismica;

    bool sonidoComenzado = false;
    bool alertaComenzada = false;
    public float volumen = 1;

    //Tiempos para los eventos
    const float TIEMPO_PARA_COMENZAR = 5f;
    const float TIEMPO_PARA_ALERTA = 5f;
    const float DURACION_TEMBLOR = 20f;
    const float TIEMPO_BAJAR_VOLUMEN = 10f;
    const float DURACION_EVENTOS = TIEMPO_PARA_COMENZAR + TIEMPO_PARA_ALERTA + DURACION_TEMBLOR 
        + TIEMPO_BAJAR_VOLUMEN;
    
    // Se crea instancia de jugador
    Player jugador = new Player();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tiempo += Time.deltaTime;

        if (GameInfo.haveGyroscope)
        {
            jugador.girarCamaraConLaMirada(Camara);
            jugador.interactuarConLaMirada(Camara);
        }
        else
        {
            jugador.obtenerNombreDelObjetoTocado(vision);
            jugador.girarCamaraLateral(Camara);
        }
        
        //Rutina de eventos
        activarAlertaSismica();
        activarSismo();
        desactivarSonidos();
    }

    private void activarAlertaSismica()
    {
        if (tiempo > TIEMPO_PARA_ALERTA)
        {
            if (!alertaComenzada)
            {
                alertaSismica.Play();
                alertaComenzada = true;
            }
        }
    }
    private void activarSismo()
    {
        if (tiempo > (TIEMPO_PARA_ALERTA + TIEMPO_PARA_COMENZAR))
        {
            if (!sonidoComenzado)
            {
                efectoTerremoto.Play();
                efectoPlatos.Play();
                efectoCosas.Play();
                sonidoComenzado = true;
            }
            //print("Temblor!");

            jugador.shakeObject(DURACION_TEMBLOR + TIEMPO_BAJAR_VOLUMEN, .005f, Elemento, volumen);
        }
    }
    private void desactivarSonidos()
    {
        if (tiempo > (TIEMPO_PARA_ALERTA + TIEMPO_PARA_COMENZAR + DURACION_TEMBLOR))
        {
            volumen = (DURACION_EVENTOS - tiempo) / TIEMPO_BAJAR_VOLUMEN;
            if(volumen >= 0)
            {
                efectoTerremoto.volume  = volumen;
                efectoPlatos.volume     = volumen;
                efectoCosas.volume      = volumen;
                alertaSismica.volume    = volumen;
            }
            else
            {
                efectoTerremoto.Stop();
                efectoPlatos.Stop();
                efectoCosas.Stop();
                alertaSismica.Stop();
            }
        }
    }
    public void regresarAlMenu()
    {
        SceneManager.LoadScene("Principal");
    }

}
