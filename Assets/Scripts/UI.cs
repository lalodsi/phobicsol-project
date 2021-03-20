using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    //Paneles de navegación
    public GameObject menuPrincipal;
    public GameObject pantallaInstrucciones;
    public GameObject pantallaCreditos;
    public GameObject pantallaSalida;

    float tiempo, tiempoCarga;
    bool salir;
    bool YaSeTieneInfoDelDispositivo; //Determina si ya se tiene la info del dispositivo

    const string MENU_PRINCIPAL     = "Principal";
    const string ESCENA_DEL_JUEGO   = "EscenaJuego";
    // Start is called before the first frame update
    void Start()
    {
        salir = false;  
    }
    // Update is called once per frame
    void Update()
    {
        tiempoCarga = Time.time;
        GameInfo.haveGyroscope = SystemInfo.supportsGyroscope;
        if (GameInfo.haveGyroscope) Input.gyro.enabled = true;
        if (GameInfo.haveGyroscope)
        {
            //TextoGiro.text = "Tu dispositivo contiene giroscopio";
        }
        else
        {
            //TextoGiro.text = "Lamentablemente tu dispositivo no contiene giroscopio";
        }
        mostrarPantallaYSalir();
    }
    //Función MenuPrincipal_botonInstrucciones()
    public void MP_bInstrucciones()
    {
        cambiarPanel(pantallaInstrucciones, menuPrincipal);
    }
    public void MP_bCreditos()
    {
        cambiarPanel(pantallaCreditos, menuPrincipal);
    }
    public void MP_bSalir()
    {
        salir = true;
        cambiarPanel(pantallaSalida, menuPrincipal);
    }
    public void PI_bRegresar()
    {
        cambiarPanel(menuPrincipal, pantallaInstrucciones);
    }
    public void PC_bRegresar()
    {
        cambiarPanel(menuPrincipal, pantallaCreditos);
    }
    public void MP_bIniciar()
    {
        SceneManager.LoadScene(ESCENA_DEL_JUEGO);
    }
    private void cambiarPanel(GameObject PANEL_A_MOSTRAR, GameObject PANEL_A_OCULTAR)
    {
        PANEL_A_OCULTAR.SetActive(false);
        PANEL_A_MOSTRAR.SetActive(true);
    }
    private void mostrarPantallaYSalir()
    {
        if (salir)
        {
            tiempo += Time.deltaTime;
            if (tiempo > 5f) Application.Quit();
        }
    }
}
