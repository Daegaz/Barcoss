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
                enemyMovement.movesLeft = 10; // Ahora el enemigo tiene 10 movimientos
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
        UnityEngine.Debug.Log("IA pensando...");

        while (enemyMovement.movesLeft > 0)
        {
            yield return new WaitForSeconds(0.7f);

            int diffX = playerMovement.gridX - enemyMovement.gridX;
            int diffY = playerMovement.gridY - enemyMovement.gridY;

            // Si está al lado, ejecuta Ramming
            if (Mathf.Abs(diffX) + Mathf.Abs(diffY) == 1)
            {
                UnityEngine.Debug.Log("IA ejecuta RAMMING");
                ShipStats playerStats = playerMovement.GetComponent<ShipStats>();
                if (playerStats) playerStats.RecibirDanio(20f);

                // El enemigo rebota 2 casillas hacia atrás
                enemyMovement.Retroceder(-diffX, -diffY, 2);
                enemyMovement.ConsumirMovimientos(1);
            }
            else
            {
                // Moverse hacia el jugador
                int stepX = diffX != 0 ? (int)Mathf.Sign(diffX) : 0;
                int stepY = (stepX == 0 && diffY != 0) ? (int)Mathf.Sign(diffY) : 0;

                enemyMovement.gridX += stepX;
                enemyMovement.gridY += stepY;

                enemyMovement.SnapToGrid();
                enemyMovement.ConsumirMovimientos(1);
            }
        }

        yield return new WaitForSeconds(0.7f);
        CambiarTurno(Turno.Jugador);
    }
}