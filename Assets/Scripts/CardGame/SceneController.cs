using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public int gridRows = 2;
    public int gridCols = 4;
    public float header = 2f;
    public float margin = 1f;

    [SerializeField] private MemoryCard originalCard;
    [SerializeField] private Sprite[] images;

    private MemoryCard _firstRevealed, _secondRevealed;

    public bool CanReveal => _secondRevealed == null && !paused;

    private int _score = 0, _pairsFound = 0;

    public float timeOutSeconds = 3.0f;
    float currentTime;

    private bool _paused;
    public bool paused
    {
        get => _paused;
        set
        {
            _paused = value;
            Time.timeScale = value ? 0.0f : 1.0f;
            ui.ShowPauseMenu(value);
        }
    }

    public int numberOfCards => gridCols * gridRows;

    public bool gameover => _pairsFound == numberOfCards / 2;

    private UIController ui;

    private void Awake()
    {
        ui = GetComponent<UIController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // The pause menu starts not visible
        ui.ShowPauseMenu(false);

        // Place the cards in the board

        float totalHeight = Camera.main.orthographicSize * 2;
        float totalWidth = totalHeight * Camera.main.aspect;
        float cardWidth = (totalWidth - 2 * margin) / gridCols;
        float cardHeight = (totalHeight - header - margin) / gridRows;

        // Ensure that there are only pairs in the board
        int[] ids = new int[numberOfCards];
        for (int i = 0; i < ids.Length; i++)
        {
            ids[i] = i / 2;
        }
        ids = ShuffleArray(ids);

        for (int j = 0; j < gridRows; j++)
        {
            for (int i = 0; i < gridCols; i++)
            {
                MemoryCard card = Instantiate(originalCard);

                int index = j * gridCols + i;
                int id = ids[index];

                card.SetCard(id, images[id]);

                float posX = (i + 0.5f) * cardWidth - totalWidth / 2 + margin;
                float posY = (j + 0.5f) * cardHeight - totalHeight / 2 + margin;
                card.transform.position = new Vector3(posX, posY, originalCard.transform.position.z);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }

        if (paused) return;

        currentTime -= Time.deltaTime;
        if (currentTime < 0)
        {
            if (_score > 0)
            {
                _score--; ui.SetScore(_score);
            }
            ResetTimeOut();
        }
        ui.SetTimeOutValue(currentTime / timeOutSeconds);
    }

    void ResetTimeOut()
    {
        currentTime = timeOutSeconds;
    }

    public void CardRevealed(MemoryCard card)
    {
        ResetTimeOut();
        if (_firstRevealed == null)
        {
            _firstRevealed = card;
        }
        else
        {
            _secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("CardGame");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private IEnumerator CheckMatch()
    {
        if (_firstRevealed.Id == _secondRevealed.Id)
        {
            _score++;
            ui.SetScore(_score);
            _pairsFound++;

            if (gameover)
            {
                ui.EnableTimeOutBar(false);
                paused = true;
                Debug.Log("CONGRATULATIONS! YOU WON");
            }
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            _firstRevealed.Unreveal();
            _secondRevealed.Unreveal();
        }
        _firstRevealed = _secondRevealed = null;
    }

    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length - 1; i++)
        {
            int r = Random.Range(i, newArray.Length);
            int tmp = newArray[i];
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }
}
