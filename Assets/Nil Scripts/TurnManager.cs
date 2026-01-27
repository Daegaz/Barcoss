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

    void Awake() => Instance = this;

    void Start() => StartCoroutine(CheckTurno());

    IEnumerator CheckTurno()
    {
        while (true)
        {
            if (turnoActual == Turno.Jugador && playerMovement.movesLeft <= 0)
            {
                yield return new WaitForSeconds(0.7f);
                CambiarTurno(Turno.Enemigo);
            }
            yield return null;
        }
    }

    public void CambiarTurno(Turno nuevo)
    {
        turnoActual = nuevo;
        if (turnoActual == Turno.Enemigo)
        {
            enemyMovement.movesLeft = 3;
            StartCoroutine(IATurnoEnemigo());
        }
        else
        {
            playerMovement.movesLeft = 10;
            playerMovement.UpdateUI();
        }
    }

    IEnumerator IATurnoEnemigo()
    {
        Debug.Log("Turno IA: Moviendo...");

        while (enemyMovement.movesLeft > 0)
        {
            yield return new WaitForSeconds(0.6f);

            int diffX = playerMovement.gridX - enemyMovement.gridX;
            int diffY = playerMovement.gridY - enemyMovement.gridY;

            // Si está al lado, ataca (Ramming)
            if (Mathf.Abs(diffX) + Mathf.Abs(diffY) == 1)
            {
                ShipStats pStats = playerMovement.GetComponent<ShipStats>();
                if (pStats) pStats.RecibirDanio(15f);
                playerMovement.Retroceder(diffX, diffY, 1);
                enemyMovement.ConsumirMovimientos(1);
            }
            else
            {
                // Intentar moverse hacia el jugador
                int moveX = (diffX != 0) ? (int)Mathf.Sign(diffX) : 0;
                int moveY = (moveX == 0 && diffY != 0) ? (int)Mathf.Sign(diffY) : 0;

                int targetX = enemyMovement.gridX + moveX;
                int targetY = enemyMovement.gridY + moveY;

                // VALIDACIÓN DE COLISIÓN
                if (!enemyMovement.IsCellOccupied(targetX, targetY))
                {
                    enemyMovement.gridX = targetX;
                    enemyMovement.gridY = targetY;
                }
                else
                {
                    // Si el eje principal está bloqueado, intenta el otro (esquivar isla)
                    moveX = (moveX == 0 && diffX != 0) ? (int)Mathf.Sign(diffX) : 0;
                    moveY = (moveY == 0 && diffY != 0) ? (int)Mathf.Sign(diffY) : 0;

                    if (!enemyMovement.IsCellOccupied(enemyMovement.gridX + moveX, enemyMovement.gridY + moveY))
                    {
                        enemyMovement.gridX += moveX;
                        enemyMovement.gridY += moveY;
                    }
                    else
                    {
                        Debug.Log("IA bloqueada totalmente este paso.");
                    }
                }

                enemyMovement.SnapToGrid();
                enemyMovement.ConsumirMovimientos(1);
            }
        }

        yield return new WaitForSeconds(0.5f);
        CambiarTurno(Turno.Jugador);
    }
}