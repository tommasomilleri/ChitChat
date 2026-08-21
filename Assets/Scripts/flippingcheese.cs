using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class flippingcheese : MonoBehaviour, IPointerClickHandler
{

    public GameObject NextLevel;
    public GameObject CurrentLevel; 
    public GameObject canvas;
    public GameObject check;
   

       public float rotationStep;
    private bool levelCompleted = false;

   
    void Update()
    {
       
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!levelCompleted)
        {
            transform.Rotate(0f , 0f, rotationStep);
            check.SetActive(true);

        }

    }

    void GoToNextLevel()
    {

        if (NextLevel != null && canvas != null)
        {
            var newLevel = Instantiate(NextLevel);

            // Passing 'false' prevents the UI from scaling weirdly when parented
            newLevel.transform.SetParent(canvas.transform, false);
        }

        if (CurrentLevel != null)
        {
            Destroy(CurrentLevel);
        }
        //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        //Cursor.visible = true;
    }
}