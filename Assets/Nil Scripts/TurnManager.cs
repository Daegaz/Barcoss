using UnityEngine;
using System.Collections;

public enum Turno { Jugador, Enemigo }

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public Turno turnoActual = Turno.Jugador;

    public ShipMovement playerMovement;
    public ShipMovement enemyMovement;
    public ShipCombat playerCombat;

    private int direccionEsquiveY = 0;
    private bool flechasLanzadasEsteTurno = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start() => StartCoroutine(CheckTurno());

    IEnumerator CheckTurno()
    {
        while (true)
        {
            if (turnoActual == Turno.Jugador && playerMovement != null && playerMovement.movesLeft <= 0)
            {
                yield return new WaitForSeconds(0.7f);
                CambiarTurno(Turno.Enemigo);
            }
            yield return null;
        }
    }

    public void CambiarTurno(Turno nuevoTurno)
    {
        turnoActual = nuevoTurno;
        if (turnoActual == Turno.Enemigo)
        {
            if (enemyMovement != null)
            {
                enemyMovement.movesLeft = 10;
                direccionEsquiveY = 0;
                flechasLanzadasEsteTurno = false;
                StartCoroutine(IATurnoEnemigo());
            }
            else CambiarTurno(Turno.Jugador);
        }
        else
        {
            if (playerMovement != null)
            {
                playerMovement.movesLeft = 10;
                playerMovement.UpdateUI();
            }
        }
    }

    IEnumerator IATurnoEnemigo()
    {
        UnityEngine.Debug.Log("IA: Iniciando turno táctico.");

        while (enemyMovement.movesLeft > 0)
        {
            yield return new WaitForSeconds(0.4f);
            if (playerMovement == null) break;

            int diffX = playerMovement.gridX - enemyMovement.gridX;
            int diffY = playerMovement.gridY - enemyMovement.gridY;

            // 1. RAMMING (Daño aleatorio 10-25)
            if (Mathf.Abs(diffX) + Mathf.Abs(diffY) == 1)
            {
                float danioRam = UnityEngine.Random.Range(10f, 26f);
                UnityEngine.Debug.Log($"IA: Ejecutando embestida. Daño: {danioRam:F1}");

                ShipStats playerStats = playerMovement.GetComponent<ShipStats>();
                if (playerStats) playerStats.RecibirDanio(danioRam);

                enemyMovement.Retroceder(-diffX, -diffY, 2);
                enemyMovement.ConsumirMovimientos(1);
                direccionEsquiveY = 0;
                continue;
            }

            // 2. ARROW SHOWER (Rango reducido a 5, daño 10-20)
            if (!flechasLanzadasEsteTurno && enemyMovement.movesLeft >= 2)
            {
                bool alineadoX = Mathf.Abs(diffY) < 0.1f;
                bool alineadoY = Mathf.Abs(diffX) < 0.1f;
                float dist = (Mathf.Abs(diffX) + Mathf.Abs(diffY)) / enemyMovement.spacing;

                // Rango ajustado a 5.1f para tolerar decimales del spacing
                if ((alineadoX || alineadoY) && dist <= 4.1f)
                {
                    float danioArrows = UnityEngine.Random.Range(10f, 21f);
                    UnityEngine.Debug.Log($"IA: Disparando flechas (Rango 4). Daño: {danioArrows:F1}");

                    ShipStats playerStats = playerMovement.GetComponent<ShipStats>();
                    if (playerStats) playerStats.RecibirDanio(danioArrows);

                    enemyMovement.ConsumirMovimientos(2);
                    flechasLanzadasEsteTurno = true;
                    yield return new WaitForSeconds(0.3f);
                    continue;
                }
            }

            // 3. NAVEGACIÓN
            int stepX = (diffX != 0) ? (int)Mathf.Sign(diffX) : 0;
            if (direccionEsquiveY != 0)
            {
                if (stepX != 0 && !enemyMovement.IsCellOccupied(enemyMovement.gridX + stepX, enemyMovement.gridY))
                {
                    enemyMovement.TryMove(stepX, 0);
                    direccionEsquiveY = 0;
                }
                else
                {
                    if (!enemyMovement.TryMove(0, direccionEsquiveY))
                    {
                        direccionEsquiveY *= -1;
                        if (!enemyMovement.TryMove(0, direccionEsquiveY)) break;
                    }
                }
            }
            else
            {
                if (stepX != 0 && !enemyMovement.IsCellOccupied(enemyMovement.gridX + stepX, enemyMovement.gridY))
                {
                    enemyMovement.TryMove(stepX, 0);
                }
                else
                {
                    direccionEsquiveY = (diffY != 0) ? (int)Mathf.Sign(diffY) : 1;
                    if (!enemyMovement.TryMove(0, direccionEsquiveY))
                    {
                        direccionEsquiveY *= -1;
                        if (!enemyMovement.TryMove(0, direccionEsquiveY)) break;
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        CambiarTurno(Turno.Jugador);
    }
}