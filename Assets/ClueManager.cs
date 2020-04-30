using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Clues
{
    public string key;
    public List<string> clues;
    public int maxClueIndex = 0;
    public int currentClueIndex = 0;
}


public class ClueManager : MonoBehaviour
{
    public float m_timeToHandleNewClue = 60.0f;
    public float m_chrono;

    private bool m_newClueHasBeenChecked = false;

    private bool m_cluePanelIsOn = false;
    private bool m_isLoadingComplete = false;

    public Image m_backgroundClueButtonImage;
    public Toggle m_clueToggle;
    public Color m_newClueColor;
    private Color m_regularClueColor;

    public List<Clues> m_cluesList;

    public Animator m_cluePanelAnimator;

    public Text m_clueText;

    // Start is called before the first frame update
    void Start()
    {
        m_regularClueColor = m_backgroundClueButtonImage.color;
        m_cluesList = new List<Clues>();

        ResetChrono();

        // FOR TEST ONLY, TO BE REMOVES SOON
        List<string> testList = new List<string>();
        testList.Add("a");
        testList.Add("b");
        testList.Add("c");

        List<string> testList2 = new List<string>();
        testList2.Add("d");
        testList2.Add("e");
        testList2.Add("f");

        AddClues("TEST", testList);
        AddClues("TEST", testList2);
        AddClues("TEST2", testList);

    }

    // Update is called once per frame
    void Update()
    {
        if (m_cluesList.Count == 0)
        {
            m_clueToggle.interactable = false;
        } else if (m_cluesList[0].maxClueIndex == 0)
        {
            m_clueToggle.interactable = false;
        } else
        {
            m_clueToggle.interactable = true;
        }

        if (ChronoMustBeRunning())
        {
            m_chrono += Time.deltaTime;
        }
       
        
        if (m_chrono >= m_timeToHandleNewClue)
        {        
            HandleNewClue();
        }

        if (m_cluesList.Count > 0)
        {
            m_clueText.text = "Indice " + (m_cluesList[0].currentClueIndex + 1) + " : " + m_cluesList[0].clues[m_cluesList[0].currentClueIndex];
        }


    }

    private void HandleNewClue()
    {
        if (m_cluesList.Count > 0)
        {
            m_backgroundClueButtonImage.color = m_newClueColor;
            m_cluesList[0].maxClueIndex++;
            m_newClueHasBeenChecked = false;
        }
       

         

        ResetChrono();
    }

    private void ResetChrono()
    {
        m_chrono = 0f;
    }

    public void AddClues(string key, List<string> cluesToAdd)
    {
        bool cluesHaveBeenAdded = false;

        foreach (Clues currentClues in m_cluesList)
        {
            if (currentClues.key == key)
            {
                currentClues.clues.AddRange(cluesToAdd);
                cluesHaveBeenAdded = true;
            }
        }

        if (!cluesHaveBeenAdded)
        {
            Clues newClues = new Clues();
            newClues.key = key;
            newClues.clues = new List<string>();
            newClues.maxClueIndex = 0;
            newClues.currentClueIndex = 0;
            newClues.clues.AddRange(cluesToAdd);

            if (m_cluesList.Count == 0)
            {
                m_newClueHasBeenChecked = true;
            }

            m_cluesList.Add(newClues);
        }
    }

    public void RemoveClues(string key)
    {
        int index = -1;
        int indexToRemove = -1;

        foreach (Clues currentClues in m_cluesList)
        {
            index++;
            if (currentClues.key == key)
            {
                indexToRemove = index;
            }
        }

        if (indexToRemove != -1)
        {
            m_cluesList.RemoveAt(indexToRemove);
        }

        if (indexToRemove == 0)
        {
            CluePanelOff();

            m_newClueHasBeenChecked = true;
        }
    }

    public void ToggleCluePanel()
    {
        if (m_cluePanelIsOn)
        {
            m_cluePanelAnimator.SetBool("MustBeOnScreen", false);
        } else
        {
            m_cluePanelAnimator.SetBool("MustBeOnScreen", true);
            m_backgroundClueButtonImage.color = m_regularClueColor;
            m_newClueHasBeenChecked = true;

            if (m_cluesList.Count > 0)
            {
                m_cluesList[0].currentClueIndex = m_cluesList[0].maxClueIndex - 1;
            }
        }

        m_cluePanelIsOn = !m_cluePanelIsOn;
    }

    public void CluePanelOff()
    {
        if (m_cluePanelIsOn)
        {
            m_clueToggle.isOn = false;
        }
    }

    private bool ChronoMustBeRunning()
    {
        if (!m_isLoadingComplete)
        {
            return false;
        }

        if (m_cluesList.Count == 0)
        {
            return false;
        }

        if (m_cluesList[0].maxClueIndex == m_cluesList[0].clues.Count)
        {
            return false;
        }

        if (!m_newClueHasBeenChecked)
        {
            return false;
        }


        if (m_cluePanelIsOn)
        {
            return false;
        }
        

        return true;
    }

    public void SceneLoaded()
    {
        m_isLoadingComplete = true;
    }

    public void NextClue()
    {
        if (m_cluesList.Count > 0)
        {
            m_cluesList[0].currentClueIndex = (m_cluesList[0].currentClueIndex + 1) % m_cluesList[0].maxClueIndex;
        }
    }

    public void PreviousClue()
    {
        if (m_cluesList.Count > 0)
        {
            m_cluesList[0].currentClueIndex = (m_cluesList[0].currentClueIndex + m_cluesList[0].maxClueIndex - 1) % m_cluesList[0].maxClueIndex;
        }
    }
}
