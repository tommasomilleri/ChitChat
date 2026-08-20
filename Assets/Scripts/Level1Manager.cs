using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Cheese Quality Settings")]
    public QualityBar qualityBar;
    public int maxQuality = 100;

    // Rendo la variabile STATICA in modo che sopravviva al cambio di scene.
    // La imposto a -1 come "trucco" per capire se il gioco è appena iniziato.
    public static int currentQuality = -1;

    public int wrongAnswerPenalty = 20;

    [Header("Level Settings")]
    public int correctImage;

    public GameObject Lvl2;
    public GameObject Lvl1;
    public GameObject canvas;

    void Start()
    {
        // Imposta il valore massimo della barra UI
        qualityBar.SetMaxQuality(maxQuality);

        // Se currentQuality è -1, significa che siamo al Livello 1 e la partita è appena iniziata
        if (currentQuality == -1)
        {
            currentQuality = maxQuality;
        }

        // Sincronizza subito la UI con il valore salvato (utile quando entri nel Livello 2, 3, ecc.)
        qualityBar.SetQuality(currentQuality);
    }

    public void selectImage(int imageNumber)
    {
        if (imageNumber == correctImage)
        {
            Debug.Log("Correct choice! The cheesemaking process continues.");
            GoToNextLevel();
        }
        else
        {
            Debug.Log("Oh no! Wrong ingredient, cheese quality drops.");
            DecreaseQuality(wrongAnswerPenalty);
        }
    }

    void DecreaseQuality(int damage)
    {
        currentQuality -= damage;

        if (currentQuality < 0)
        {
            currentQuality = 0;
        }

        qualityBar.SetQuality(currentQuality);

        if (currentQuality == 0)
        {
            CheeseSpoiled();
        }
    }

    void GoToNextLevel()
    {
        // Se userai le Scene di Unity in futuro, qui metterai: SceneManager.LoadScene("NomeScenaLivello2");
        var newLevel = Instantiate(Lvl2);
        newLevel.transform.SetParent(canvas.transform);
        Destroy(Lvl1);
    }

    void CheeseSpoiled()
    {
        Debug.Log("The cheese went bad! A Mold Monster is born!");
    }

    // Metodo bonus: chiamalo se il giocatore fa "Game Over" o "Ricomincia Partita" 
    // per resettare la qualità del formaggio
    public void ResetGame()
    {
        currentQuality = -1;
    }
}