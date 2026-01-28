using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public enum Turno { Jugador, Enemigo }
    public static TurnManager Instance;
    public Turno turnoActual = Turno.Jugador;

    public ShipMovement playerMovement;
    public ShipMovement enemyMovement;
    public ShipCombat playerCombat;

    private bool flechasLanzadasEsteTurno = false;

    // Variables para la memoria de la IA
    private int flanqueoRestante = 0;
    private int fDirX = 0;
    private int fDirY = 0;

    void Awake() { if (Instance == null) Instance = this; }
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
                flechasLanzadasEsteTurno = false;
                flanqueoRestante = 0; // Resetear memoria al empezar turno
                StartCoroutine(IATurnoEnemigo());
            }
            else CambiarTurno(Turno.Jugador);
        }
        else if (playerMovement != null)
        {
            playerMovement.movesLeft = 10;
            playerMovement.UpdateUI();
        }
    }

    IEnumerator IATurnoEnemigo()
    {
        ShipStats statsEnemigo = enemyMovement.GetComponent<ShipStats>();

        while (enemyMovement.movesLeft > 0)
        {
            yield return new UnityEngine.WaitForSeconds(0.4f);
            if (playerMovement == null) break;

            int diffX = playerMovement.gridX - enemyMovement.gridX;
            int diffY = playerMovement.gridY - enemyMovement.gridY;
            int distAbs = Mathf.Abs(diffX) + Mathf.Abs(diffY);
            bool modoHuida = statsEnemigo != null && statsEnemigo.ObtenerPorcentajeVida() < 0.3f;

            // 1. ATAQUE
            if (distAbs == 1)
            {
                ShipStats pStats = playerMovement.GetComponent<ShipStats>();
                if (pStats) pStats.RecibirDanio(UnityEngine.Random.Range(10f, 26f));
                enemyMovement.Retroceder(-diffX, -diffY, 2);
                enemyMovement.ConsumirMovimientos(enemyMovement.ObtenerCosteAccion(1));
                flanqueoRestante = 0;
                continue;
            }

            // 2. FLECHAS
            int costeF = enemyMovement.ObtenerCosteAccion(2);
            if (!flechasLanzadasEsteTurno && enemyMovement.movesLeft >= costeF)
            {
                if ((diffX == 0 || diffY == 0) && (distAbs / enemyMovement.spacing) <= 4.1f)
                {
                    ShipStats pStats = playerMovement.GetComponent<ShipStats>();
                    if (pStats) pStats.RecibirDanio(UnityEngine.Random.Range(10f, 21f));
                    enemyMovement.ConsumirMovimientos(costeF);
                    flechasLanzadasEsteTurno = true;
                    flanqueoRestante = 0;
                    continue;
                }
            }

            // 3. MOVIMIENTO
            bool movido = false;

            // Si estamos flanqueando, seguimos la dirección guardada
            if (flanqueoRestante > 0)
            {
                if (enemyMovement.TryMove(fDirX, fDirY))
                {
                    flanqueoRestante--;
                    movido = true;
                }
                else flanqueoRestante = 0; // Si chocamos flanqueando, abortamos
            }

            if (!movido)
            {
                int stepX = (diffX != 0) ? (int)Mathf.Sign(diffX) : 0;
                int stepY = (diffY != 0) ? (int)Mathf.Sign(diffY) : 0;
                if (modoHuida) { stepX *= -1; stepY *= -1; }

                // Intentar camino directo
                if (stepX != 0 && enemyMovement.TryMove(stepX, 0)) movido = true;
                else if (stepY != 0 && enemyMovement.TryMove(0, stepY)) movido = true;

                // Si falla y no estamos huyendo, iniciamos maniobra de flanqueo
                if (!movido && !modoHuida)
                {
                    flanqueoRestante = 2; // Mantener dirección 2 pasos
                    if (stepX != 0) // Bloqueado en X, desviamos en Y
                    {
                        fDirX = 0;
                        fDirY = (diffY >= 0) ? 1 : -1;
                        if (!enemyMovement.TryMove(fDirX, fDirY))
                        {
                            fDirY *= -1;
                            movido = enemyMovement.TryMove(fDirX, fDirY);
                        }
                        else movido = true;
                    }
                    else if (stepY != 0) // Bloqueado en Y, desviamos en X
                    {
                        fDirY = 0;
                        fDirX = (diffX >= 0) ? 1 : -1;
                        if (!enemyMovement.TryMove(fDirX, fDirY))
                        {
                            fDirX *= -1;
                            movido = enemyMovement.TryMove(fDirX, fDirY);
                        }
                        else movido = true;
                    }
                }
            }

            if (!movido) break;
        }

        yield return new WaitForSeconds(0.5f);
        CambiarTurno(Turno.Jugador);
    }
}