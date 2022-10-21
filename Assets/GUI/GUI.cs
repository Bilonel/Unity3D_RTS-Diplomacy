using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    [SerializeField] GameObject Map;


    public float Coin=50;
    public float Population=0;

    public CustomCursor customCursor;
    GameObject activatedPanel;
    private GameObject buildToPlace;
    private void Update()
    {
        PanelUpdate();
        if(Input.GetMouseButton(0)&& buildToPlace!=null)
        {
            //print(customCursor.GetComponentInChildren<Placement>().ValidPosition);         
            PlaceBuild();
        }
        else if(Input.GetKeyDown(KeyCode.M))
        {
            if(!Map.activeSelf)
            {
                Map.SetActive(true);
                Map.GetComponent<Map>().Activate();
            }
            else
            {
                Map.GetComponent<Map>().Deactivate();
            }
        }
    }
    void PanelUpdate()
    {
        transform.Find("gold").GetComponent<Text>().text = Coin.ToString();
        transform.Find("pop").GetComponent<Text>().text = Population.ToString();
    }
    public void PanelButtonClick(GameObject clickedPanel)
    {
        if (clickedPanel == null) return; 
        if (activatedPanel != clickedPanel && activatedPanel != null) activatedPanel.SetActive(false); //DEACTIVATE
        clickedPanel.SetActive(clickedPanel.activeSelf!=true);                                          // ACTIVATE
        activatedPanel = clickedPanel;                                                                // SET
    }
    public void ExitClick(GameObject exitedPanel)
    {
        exitedPanel.SetActive(false);
    }
    public void BuildButtonClick(GameObject button)
    {
        ExpensesPanel expensesPanel = button.GetComponentInChildren<ExpensesPanel>();
        GameObject build = button.GetComponentInChildren<ExpensesPanel>().build;

        int populationIndex = expensesPanel.expenses.FindIndex(x => x.Name == "Population");
        int goldIndex = expensesPanel.expenses.FindIndex(x => x.Name == "Gold");
        
        if (goldIndex >-1)
        {
            int gold = expensesPanel.counts[goldIndex]; 
            if (Coin >= gold)
                Coin -= gold;
            else { print("Not Enough Gold"); return; }
        }
        if(populationIndex >-1)
        {
            int population = expensesPanel.counts[populationIndex];
            build.GetComponent<Building>().PopulationExpense = population;
            if(Population >= population)
                Population -= population;
            else { print("Not Enough Population"); return; }
        }

        // FIND GOLD AND POPULATION
        ///
        // TRY SET NEW COUNTS
        int i = 0;
        foreach (var item in expensesPanel.expenses)
        {
            if(item.storable)
                if(!Inventory.instance.Add(item,-expensesPanel.counts[i]))
                {
                    print("Not Enough " + item.Name);
                    return;
                }
            i++;
        }

        customCursor.activateCursor(build);
        buildToPlace = build;
    }
    private void PlaceBuild()
    {
        Cursor.visible = true;
        customCursor.grid.enabled=(false);

        if (!customCursor.activeBuild.GetComponent<Building>().placement) return;

        Instantiate(buildToPlace, customCursor.transform.position, Quaternion.identity);

        buildToPlace = null;
        Destroy(customCursor.activeBuild);
        customCursor.gameObject.SetActive(false);
    }
}
