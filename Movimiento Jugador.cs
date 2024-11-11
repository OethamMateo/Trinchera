using System.Collections;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Configuración de Velocidad")]
    public float velocidad = 5f;
    public float velocidadAcelerada = 10f;

    [Header("Configuración de Interacción")]
    public float tiempoInmovilizacion = 3f;

    private float tiempoPresionado = 0f;
    private float velocidadActual;
    private bool puedeMoverse = true;
    private bool estaInteraccionando = false;

    private AudioSource audioSource;

    void Start()
    {
        velocidadActual = velocidad;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!puedeMoverse) return;

        // Mover a la derecha mientras se presiona la tecla
        if (Input.GetKey(KeyCode.RightArrow))
        {
            tiempoPresionado += Time.deltaTime;

            // Aumentar la velocidad si se mantiene presionado por más de 3 segundos
            if (tiempoPresionado >= 3f)
            {
                velocidadActual = velocidadAcelerada;
                audioSource.pitch = 1.5f; // Aumentar la velocidad del sonido
            }
            else
            {
                velocidadActual = velocidad;
                audioSource.pitch = 1.0f; // Velocidad normal del sonido
            }

            // Reproducir sonido si no está ya reproduciéndose
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            // Mover el jugador
            transform.Translate(Vector2.right * velocidadActual * Time.deltaTime);
        }
        else
        {
            // Restablecer velocidad y tiempo cuando se suelta la tecla
            tiempoPresionado = 0f;
            velocidadActual = velocidad;
            audioSource.pitch = 1.0f; // Restablecer velocidad normal del sonido

            // Detener el sonido cuando se suelta la tecla
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    // Método para deshabilitar el movimiento temporalmente
    public void DeshabilitarMovimiento(float duracion)
    {
        if (!estaInteraccionando)
        {
            StartCoroutine(DeshabilitarMovimientoCoroutine(duracion));
        }
    }

    private IEnumerator DeshabilitarMovimientoCoroutine(float duracion)
    {
        puedeMoverse = false;
        audioSource.Stop(); // Detener el sonido al inmovilizar al jugador
        yield return new WaitForSeconds(duracion);
        puedeMoverse = true;
    }

    // Método para cambiar el estado de interacción
    public void EstablecerInteraccion(bool estado)
    {
        estaInteraccionando = estado;
        if (estado) DeshabilitarMovimiento(tiempoInmovilizacion);
    }
}
