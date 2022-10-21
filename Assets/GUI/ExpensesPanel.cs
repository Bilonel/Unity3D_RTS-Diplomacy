using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpensesPanel : MonoBehaviour
{
    public GameObject build;
    [SerializeField] GameObject expense;
    [SerializeField] public List<Item> expenses = new List<Item>();
    [SerializeField] public List<int> counts= new List<int>();
    [SerializeField] int maxExpenseCount;
    int GridSpacing = 5;
    // Start is called before the first frame update
    void Start()
    {
        Rect expenseRect = expense.GetComponent<RectTransform>().rect;
        float expenseWidth = GetComponent<RectTransform>().rect.width / (maxExpenseCount + .2f);
        float expenseHeight = GetComponent<RectTransform>().rect.height - 6;
        int paddingSize = (int)((GetComponent<RectTransform>().rect.width) - ((expenseWidth + GridSpacing) * expenses.Count - GridSpacing))/2 ;

        GetComponent<GridLayoutGroup>().cellSize = new Vector2(expenseWidth, expenseHeight);
        GetComponent<GridLayoutGroup>().padding = new RectOffset(paddingSize, paddingSize, 3, 3);
        int i = 0; GameObject currentObject;
        foreach (var item in expenses)
        {
            currentObject = Instantiate(expense, transform);
            currentObject.GetComponentsInChildren<Image>()[1].sprite = item.icon;
            currentObject.GetComponentInChildren<Text>().text = counts[i++].ToString();
        }
               
    }

}
